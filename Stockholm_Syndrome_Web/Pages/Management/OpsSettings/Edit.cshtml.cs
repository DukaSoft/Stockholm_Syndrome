using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Pages.Management.OpsSettings
{
    [Authorize(Roles = "Admin,Director")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OpsTag OpsTag { get; set; }

        [BindProperty]
		public Color TagColor { get; set; }

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

            TagColor = ColorTranslator.FromHtml(OpsTag.Color);

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            OpsTag.Color = ColorTranslator.ToHtml(TagColor);

            _context.Attach(OpsTag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpsTagExists(OpsTag.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OpsTagExists(int id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }
    }
}
