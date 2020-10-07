using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDataLibrary;

namespace Stockholm_Syndrome_Web.Areas.Identity.Pages.Account.Manage.EveToons
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EveCharacter EveCharacter { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EveCharacter = await _context.EveCharacters.FirstOrDefaultAsync(m => m.Id == id);

            if (EveCharacter == null)
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

            EveCharacter = await _context.EveCharacters.FindAsync(id);

            if (EveCharacter != null)
            {
                _context.EveCharacters.Remove(EveCharacter);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}