using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Pages.EveGuides
{
    [Authorize(Roles = "MemberOfAlliance")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
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
