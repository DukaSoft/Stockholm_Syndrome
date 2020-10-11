using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDataLibrary;

namespace Stockholm_Syndrome_Web.Pages.Management.Users
{
    [Authorize(Roles="Admin,Director")]
    public class DeleteModel : PageModel
    {
        private readonly SSDataLibrary.ApplicationDbContext _context;
        private readonly UserManager<SSDataLibrary.ApplicationUser> _userManager;

		public DeleteModel(SSDataLibrary.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		[BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (ApplicationUser == null)
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

            ApplicationUser = await _context.Users.FindAsync(id);

            if (ApplicationUser != null)
            {
                if(await _userManager.IsInRoleAsync(ApplicationUser, "Admin") || await _userManager.IsInRoleAsync(ApplicationUser, "Director"))
				{
                    return RedirectToPage("./Index");
                }


                foreach(var ops in _context.Ops)
				{
                    if(ops.Creator == ApplicationUser)
					{
                        ops.Creator = null;
					}
				}


                _context.Users.Remove(ApplicationUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
