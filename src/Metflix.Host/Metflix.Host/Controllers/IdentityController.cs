using Metflix.BL.Services.Contracts;
using Metflix.Models.DbModels;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Metflix.Models.Requests.Identity;
using Microsoft.Extensions.Options;
using Metflix.Models.Configurations;
using FluentValidation.AspNetCore;
using Metflix.Host.Validators.IdentityValidators;
using Metflix.Models.Responses;
using Utils;
using System.Net;
using Metflix.Host.Extensions;
using Metflix.Models.Common;
using Metflix.Host.Common.Jwt;

namespace Metflix.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IOptionsMonitor<Jwt> _configuration;
        private readonly IIDentityService _identityService;
        public IdentityController(IOptionsMonitor<Jwt> configuration, IIDentityService identityService)
        {
            _configuration = configuration;
            _identityService = identityService;
        }

        [AllowAnonymous]
        [HttpPost(nameof(Register))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest user,CancellationToken cancellationToken)
        {          
            var result = await _identityService.CreateAsync(user,cancellationToken);
            if (result.HttpStatusCode == HttpStatusCode.Created)
            {
                return CreatedAtAction((nameof(Login)), new { Message = result.Message });
            }

            return this.ProduceResponse(result);
        }

        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([CustomizeValidator(Skip = true)] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            LoginRequestValidator loginValidator = new();
            var validatorResult = loginValidator.Validate(loginRequest);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = ResponseMessages.InvalidCredentials,
                });
            }

            var user = await _identityService.CheckUserAndPassword(loginRequest.Email, loginRequest.Password, cancellationToken);

            if (user != null)
            {

                var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration.CurrentValue.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToString()),
                        new Claim(JwtClaims.Id, user.Id.ToString()),
                        new Claim(JwtClaims.Name, user.Name ?? string.Empty),                        
                        new Claim(JwtClaims.Email, user.Email ?? string.Empty),
                        new Claim(user.Role,user.Role),
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.CurrentValue.Key));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration.CurrentValue.Issuer,
                    _configuration.CurrentValue.Audience,
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(_configuration.CurrentValue.DurationInMinutes),
                    signingCredentials: signIn);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }

            else
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = ResponseMessages.InvalidCredentials,
                });
            }



        }
    }
}
