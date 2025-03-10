using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public JsonResult GetResult(string errorMessages, HttpStatusCode response, object result)
    {
        if (!string.IsNullOrEmpty(errorMessages))
        {
            return new JsonResult(new { message = errorMessages }) { StatusCode = Convert.ToInt32(response) };
        }
        return new JsonResult(result) { StatusCode = 200 };
    }
}
