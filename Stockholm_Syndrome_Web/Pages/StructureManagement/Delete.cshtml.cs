using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Pages.StructureManagement
{
	[Authorize(Roles = "Admin,Director")]
	public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Structure Structure { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Structure = await _context.Structures.FirstOrDefaultAsync(m => m.Id == id);

            if (Structure == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Structure = await _context.Structures.FindAsync(id);
			var StructureExtraction = await _context.ExtractionData
				.FirstOrDefaultAsync(s => s.Structure_id == Structure.StructureId);

			if(StructureExtraction != null)
			{
				_context.ExtractionData.Remove(StructureExtraction);
			}

            if (Structure != null)
            {

                _context.Structures.Remove(Structure);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./FuelLevels");
        }
    }
}
