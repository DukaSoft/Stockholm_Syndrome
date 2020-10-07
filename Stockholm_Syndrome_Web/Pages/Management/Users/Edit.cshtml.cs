using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SSDataLibrary;
using Stockholm_Syndrome_Web.Helpers;
using Stockholm_Syndrome_Web.Models;

namespace Stockholm_Syndrome_Web.Pages.Management.Users
{
    [Authorize(Roles = "Admin,Director")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
		private readonly Microsoft.AspNetCore.Identity.RoleManager<ApplicationRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;

		private UserRoleHelper userRoleHelper { get; set; }

        public EditModel(ApplicationDbContext context, 
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
			_roleManager = roleManager;
			_userManager = userManager;
			userRoleHelper = new UserRoleHelper(_context);
            applicationRoles = userRoleHelper.GetRoles();

            RoleItems = new List<RoleItem>();
        }

        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public int UserId { get; set; }

        [BindProperty]
        public string UserName { get; set; }

        public UserRoles _userRoles { get; set; }
        public List<ApplicationRole> applicationRoles { get; set; }

       
        public List<RoleItem> RoleItems { get; set; }

        [BindProperty]
        public List<int> AreChecked { get; set; } = new List<int>();

        public class RoleItem
        {
            public int RoleId;
            public string RoleName;
            public bool IsChecked;
            public bool IsManaged;
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            UserId = ApplicationUser.Id;
            UserName = ApplicationUser.UserName;

            if (ApplicationUser == null)
            {
                return NotFound();
            }
            
            _userRoles = userRoleHelper.GetRoles(ApplicationUser.Id);

            foreach (var role in applicationRoles)
            {
                RoleItem roleItem = new RoleItem();

                roleItem.RoleId = role.Id;
                roleItem.RoleName = role.Name;
                roleItem.IsManaged = role.AutoManaged;

                if(_userRoles.Roles.Contains(role.Id))
				{
                    roleItem.IsChecked = true;
                    AreChecked.Add(role.Id);
				}
				else
				{
                    roleItem.IsChecked = false;
                }
                   
                RoleItems.Add(roleItem);
            }

            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApplicationUser applicationUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == UserId);

            var roles = await _roleManager.Roles.ToListAsync();

            foreach(var role in roles)
			{
                if(AreChecked.Contains(role.Id))
				{
                    await _userManager.AddToRoleAsync(applicationUser, role.Name);
				}
                else
				{
                    if (role.AutoManaged == true || role.Id == 1)
					{
                        // Skip if role is AutoManaged or Admin
						continue;
					}

					if (await _userManager.IsInRoleAsync(applicationUser, role.Name))
					{
                        await _userManager.RemoveFromRoleAsync(applicationUser, role.Name);
					}
				}

			}

            await Task.CompletedTask;

            return RedirectToPage("./Index");
        }

        private bool ApplicationUserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
