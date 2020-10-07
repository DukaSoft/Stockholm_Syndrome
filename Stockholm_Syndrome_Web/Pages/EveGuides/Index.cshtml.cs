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
    [Authorize(Roles = "MemberOfAlliance")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<EveGuide> EveGuides { get;set; }

        public async Task OnGetAsync()
        {
            EveGuides = await _context.EveGuides.ToListAsync();
        }
    }
}
