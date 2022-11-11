using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Metflix.Models.Responses;
using Metflix.Models.Responses.Movies.MovieDtos;
using Microsoft.AspNetCore.Mvc;

namespace Metflix.Tests.Helpers
{
    public static class ControllerAsserts
    {
        public static void AssertOkObjectResult<TValue>(IActionResult result, TValue expectedModel)
            where TValue : class
        {
            var okObjectResult = result as OkObjectResult;
            var resultValue = okObjectResult!.Value as TValue;
            Assert.NotNull(result);
            Assert.NotNull(resultValue);
            expectedModel.Should().BeEquivalentTo(resultValue);
        }

        public static void AssertNotFoundObjectResult(IActionResult result, string errorMessage)
        {
            var errorResponse = new ErrorResponse() { Error = errorMessage };
            var notFoundObjectResult = result as NotFoundObjectResult;
            var resultValue = notFoundObjectResult!.Value as ErrorResponse;
            Assert.NotNull(result);
            Assert.NotNull(resultValue);
            errorResponse.Should().BeEquivalentTo(resultValue);
        }

        public static void AssertBadRequestObjectResult(IActionResult result, string errorMessage)
        {
            var errorResponse = new ErrorResponse() { Error = errorMessage };
            var badRequestObjectResult = result as BadRequestObjectResult;
            var resultValue = badRequestObjectResult!.Value as ErrorResponse;
            Assert.NotNull(result);
            Assert.NotNull(resultValue);
            errorResponse.Should().BeEquivalentTo(resultValue);
        }

        public static void AssertNoContentResult(IActionResult result)
        {
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
        }
    }
}
