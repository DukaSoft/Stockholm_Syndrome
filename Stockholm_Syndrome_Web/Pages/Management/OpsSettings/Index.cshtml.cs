using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SSDataLibrary;

namespace Stockholm_Syndrome_Web.Pages.Management.OpsSettings
{
    [Authorize(Roles = "Admin,Director")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

		public List<OpsTag> Tags { get; set; }

		public IndexModel(ApplicationDbContext context)
		{
            _context = context;
            Tags = _context.Tags.ToList();
        }

        public void OnGet()
        {

        }
    }
}