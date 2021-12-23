using Microsoft.AspNetCore.Mvc;
using Studentenbeheer.Areas.Identity.Data;
using Studentenbeheer.Data;
using Studentenbeheer.Models;

namespace Studentenbeheer.Controllers
{
    public class AppController : Controller
    {
        protected readonly AppUser _user;
        protected readonly AppDataContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ILogger<AppController> _logger;

        protected AppController(AppDataContext context,
                                IHttpContextAccessor httpContextAccessor,
                                ILogger<AppController> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            _user = SessionUser.GetUser(httpContextAccessor.HttpContext);
        }
    }
}
