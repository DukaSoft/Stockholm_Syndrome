using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Pages.StructureManagement
{
	[Authorize(Roles="Admin,Director")]
    public class EditStructureModel : PageModel
    {
        private readonly Stockholm_Syndrome_Web.Data.ApplicationDbContext _context;

        public EditStructureModel(Stockholm_Syndrome_Web.Data.ApplicationDbContext context)
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

		public async Task<IActionResult> OnPostAddRole(long id, string role, string returnUrl)
		{
			// Add role to the provided structure
			var structure = await _context.Structures.FirstOrDefaultAsync(s => s.Id == id);

			if (structure != null)
			{
				structure.RoleNeededToManage = role;

				_context.Structures.Update(structure);
				await _context.SaveChangesAsync();
			}

			return RedirectToPage("./FuelLevels");
		}
    }
}
