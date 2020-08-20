using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : Controller
    {
        private IAppCache _cache;
        protected IAppCache Cache => _cache ??= HttpContext.RequestServices.GetService<IAppCache>();
    }
}
