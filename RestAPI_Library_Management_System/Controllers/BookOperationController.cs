using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI_Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookOperationController : ControllerBase
    {
        public static AppDbContextDbContext _context;

        public BookOperationController(AppDbContextDbContext DB)
        {
            _context = DB;
        }
    }
}
