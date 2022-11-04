using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.DbModels;

namespace Metflix.Models.Responses
{
    public abstract class BaseResponse<T>
    {
        public HttpStatusCode HttpStatusCode { get; init; }

        public string Message { get; set; } = null!;

        public T? Model { get; set; } = default(T)!;
    }
}
