using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Pages.EveGuides
{
    public class DetailsModel : PageModel
    {
        private readonly Stockholm_Syndrome_Web.Data.ApplicationDbContext _context;

        public DetailsModel(Stockholm_Syndrome_Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
