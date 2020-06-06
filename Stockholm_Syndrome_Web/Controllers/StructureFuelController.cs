using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;
using Stockholm_Syndrome_Web.Models;

namespace Stockholm_Syndrome_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StructureFuelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StructureFuelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/StructureFuel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StructureFuelModel>>> GetStructures(string token)
        {
            if (token != "somesecurityhere!")
			{
				return NotFound();
			}

			List<StructureFuelModel> structureFuel = new List<StructureFuelModel>();

            var allStructures = await _context.Structures.ToListAsync();

            foreach(var structure in allStructures)
			{
                DateTime? date;
                try
                {
                    date = DateTime.Parse(structure.FuelExpires, new System.Globalization.CultureInfo("en-US"));
                }
                catch
                {
                    // Its ok if this fails
                    date = null;
                }

                StructureFuelModel fm = new StructureFuelModel();

                fm.Name = structure.StructureName;
                if(date != null)
				{
                    fm.Expire = (DateTime)date;
				}

                structureFuel.Add(fm);
			}

            return structureFuel;
        }
    }
}
