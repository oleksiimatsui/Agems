using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpamController : Controller
    {
        private readonly AgemsSoundsContext _context;

        public SpamController(AgemsSoundsContext context)
        {
            _context = context;
        }

        // GET: api/Spam
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sound>>> GetSpam()
        {
            var _spam = _context.Sounds;
            return await _spam.ToListAsync();
        }
    }
}
