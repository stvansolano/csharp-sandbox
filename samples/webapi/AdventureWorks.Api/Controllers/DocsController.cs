using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AdventureWorks.Api.Controllers
{
	/// <summary>
    /// Controller to display API documentation in Swagger format
    /// </summary>
    [Route("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DocsController : Controller
    {
        [Route(""), HttpGet]
        [AllowAnonymous]
        public IActionResult ReDoc()
        {
            return Redirect("~/swagger");
        }
    }
}