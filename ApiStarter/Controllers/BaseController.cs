using ApiStarter.Environment;
using Microsoft.AspNetCore.Mvc;

namespace ApiStarter.Controllers
{
    /// <summary>
    /// Ancestor of Controller for working with Base objects
    /// </summary>
    public class BaseController : ControllerBase
    {
        protected readonly WebEnvironment WebEnvironment;

        public BaseController(WebEnvironment webEnvironment)
        {
            WebEnvironment = webEnvironment;
        }
    }
}
