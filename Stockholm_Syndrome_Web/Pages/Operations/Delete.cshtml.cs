using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using SSDataLibrary;

namespace Stockholm_Syndrome_Web.Pages.Operations
{
    [Authorize(Roles = "OpsCreate,OpsManager")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Ops> _logger;
        public readonly UserManager<ApplicationUser> _userManager;

        public DeleteModel(ApplicationDbContext context, ILogger<Ops> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        [BindProperty]
        public Ops Ops { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ops = await _context.Ops.Include(c => c.Creator).FirstOrDefaultAsync(m => m.Id == id);

            if (User.IsInRole("OpsCreate"))
            {
                // Check to see if the user is allowed to delete this op
                if (Ops.Creator != await _userManager.GetUserAsync(User))
                {
                    return Forbid();
                }
            }

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

            Ops = await _context.Ops.Include(c => c.Creator).FirstOrDefaultAsync(m => m.Id == id);

            // Check if the user is allowed to delete this OP
            if (!User.IsInRole("OpsManager"))
            { 
				if (Ops.Creator == null || Ops.Creator != await _userManager.GetUserAsync(User))
				{
                    return RedirectToPage("./Index");
                }
			}

            Ops = await _context.Ops.FindAsync(id);

            if (Ops != null)
            {
                _context.Ops.Remove(Ops);
                await _context.SaveChangesAsync();

                Log.Information("Ops {@Ops} Deleted by {Username}", Ops, User.Identity.Name);
            }

            return RedirectToPage("./Index");
        }
    }
}
