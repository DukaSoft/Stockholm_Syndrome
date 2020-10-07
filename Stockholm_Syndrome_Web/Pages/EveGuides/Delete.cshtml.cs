using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDataLibrary;

namespace Stockholm_Syndrome_Web.Pages.EveGuides
{
    [Authorize(Roles = "OpsManager")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EveGuide EveGuides { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EveGuides = await _context.EveGuides.FirstOrDefaultAsync(m => m.Id == id);

            if (EveGuides == null)
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

            EveGuides = await _context.EveGuides.FindAsync(id);

            if (EveGuides != null)
            {
                _context.EveGuides.Remove(EveGuides);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
