using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scanner.Models;
using System.Xml;

namespace Scanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GnEntFileExtController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        public GnEntFileExtController(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        //get{id}
        [HttpGet("{EntityKey}")]
        public async Task<ActionResult<GN_ENT_FILE_EXT>> GetGnEntFileExt(string GnModule, string EntityCode, string EntityKey)//,string FileTypeCd,  string Teur1)
        {
            if (_dbcontext.GN_ENT_FILE_EXTs == null)
            {
                return NotFound();
            }

            var gnEntFileExt = await _dbcontext.GN_ENT_FILE_EXTs
                                .Where(t => t.GN_MODULE_ID == GnModule && t.ENTITY_CODE == EntityCode && t.ENTITY_KEY == EntityKey)
                                .FirstOrDefaultAsync();

            if (gnEntFileExt == null)
            {
                return NotFound();
            }

            return gnEntFileExt;
        }

        [HttpPost]
        public async Task<ActionResult<GN_ENT_FILE_EXT>> PostMyEntity(GN_ENT_FILE_EXT gnEntFileExt)
        {
            //   _dbcontext.GN_ENT_FILE_EXTs.AddRange(gnEntFileExt);
            try
            {
                _dbcontext.GN_ENT_FILE_EXTs.Add(gnEntFileExt);
                await _dbcontext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetGnEntFileExt), new { GnModule = gnEntFileExt.GN_MODULE_ID, EntityCode = gnEntFileExt.ENTITY_CODE, EntityKey = gnEntFileExt.ENTITY_KEY }, gnEntFileExt);
            }
            catch (DbUpdateException ex)
            {
                // התרחשה שגיאה בזמן הוספת הנתונים למסד הנתונים
                // ניתן לקרוא את השגיאה מתוך הפרופרטי InnerException של ה DbUpdateException
                return BadRequest(ex.InnerException);
            }
        }
    }
}
