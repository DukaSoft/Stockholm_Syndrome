using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Pages.Operations
{
    [Authorize(Roles = "OpsCreate,OpsManager")]
    public class DeleteModel : PageModel
    {
        private readonly Stockholm_Syndrome_Web.Data.ApplicationDbContext _context;
        private readonly ILogger<Ops> _logger;

        public DeleteModel(Stockholm_Syndrome_Web.Data.ApplicationDbContext context, ILogger<Ops> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Ops Ops { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ops = await _context.Ops.FirstOrDefaultAsync(m => m.Id == id);

            if (Ops == null)
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

            Ops = await _context.Ops.FindAsync(id);

            // Check if the user is allowed to delete this OP
            if(!User.IsInRole("OpsManager"))
            { 
				if (Ops.Creator == null || Ops.Creator != User.Identity)
				{
                    return RedirectToPage("./Index");
                }
			}

            if (Ops != null)
            {
                _context.Ops.Remove(Ops);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Ops Deleted", Ops);
            }

            return RedirectToPage("./Index");
        }
    }
}
