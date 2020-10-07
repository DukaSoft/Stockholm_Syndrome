using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDataLibrary;

namespace Stockholm_Syndrome_Web.Pages.AllianceManagement
{
	[Authorize(Roles = "Admin")]
	public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Corp> Corp { get;set; }

        public async Task OnGetAsync()
        {
            Corp = await _context.Alliance.ToListAsync();
        }
    }
}
