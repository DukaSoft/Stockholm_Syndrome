using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Pages.Management.OpsSettings
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
        public OpsTag OpsTag { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OpsTag = await _context.Tags.FirstOrDefaultAsync(m => m.Id == id);

            if (OpsTag == null)
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

            OpsTag = await _context.Tags.FindAsync(id);

            if (OpsTag != null)
            {
                _context.Tags.Remove(OpsTag);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
