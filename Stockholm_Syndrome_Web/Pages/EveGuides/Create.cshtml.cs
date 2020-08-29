using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Pages.EveGuides
{
    public class CreateModel : PageModel
    {
        private readonly Stockholm_Syndrome_Web.Data.ApplicationDbContext _context;

        public CreateModel(Stockholm_Syndrome_Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public EveGuide EveGuides { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.EveGuides.Add(EveGuides);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
