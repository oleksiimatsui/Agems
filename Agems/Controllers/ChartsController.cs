using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Agems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly AgemsSoundsContext _context;
        private object text;

        public ChartsController(AgemsSoundsContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var categories = _context.Categories.Include(x => x.Sounds).ToList();
            List<object> CategorySounds = new List<Object>();
            //StringBuilder sb = new StringBuilder();
            //sb.Append("[");
            foreach (var d in categories)
            {
                string color = string.Format("#{0:X6}", new Random().Next(0x1000000));
                CategorySounds.Add(new { text = d.Name, value = d.Sounds.Count, color = color });
            }
            //sb = sb.Remove(sb.Length - 1, 1);
            //sb.Append("]");
            return new JsonResult(CategorySounds);
        }

    }
}
