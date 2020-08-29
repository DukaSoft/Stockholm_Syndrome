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
    public class IndexModel : PageModel
    {
        private readonly Stockholm_Syndrome_Web.Data.ApplicationDbContext _context;

        public IndexModel(Stockholm_Syndrome_Web.Data.ApplicationDbContext context)
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
