using Microsoft.AspNetCore.Mvc;

namespace RestAPI_Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatronOperationController : Controller
    {
        public static AppDbContextDbContext _context;

        public PatronOperationController(AppDbContextDbContext DB)
        {
            _context = DB;
        }

    }
}
