using API.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        public IActionResult Error(int code)
        {
            HttpStatusCode httpCode = (HttpStatusCode)code;
            return new ObjectResult(new ApiErrorResponse(httpCode));
        }
    }
}
