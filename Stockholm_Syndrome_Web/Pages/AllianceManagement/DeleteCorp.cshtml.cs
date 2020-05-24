using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Pages.AllianceManagement
{
	[Authorize(Roles = "Admin")]
	public class DeleteCorpModel : PageModel
    {
        private readonly Stockholm_Syndrome_Web.Data.ApplicationDbContext _context;

        public DeleteCorpModel(Stockholm_Syndrome_Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Corp Corp { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Corp = await _context.Alliance.FirstOrDefaultAsync(m => m.Id == id);

            if (Corp == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Corp = await _context.Alliance.FindAsync(id);

            if (Corp != null)
            {
                _context.Alliance.Remove(Corp);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
