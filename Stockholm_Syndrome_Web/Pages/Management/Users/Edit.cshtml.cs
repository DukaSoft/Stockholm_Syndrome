using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;
using Stockholm_Syndrome_Web.Helpers;
using Stockholm_Syndrome_Web.Models;

namespace Stockholm_Syndrome_Web.Pages.Management.Users
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly Stockholm_Syndrome_Web.Data.ApplicationDbContext _context;

        private UserRoleHelper userRoleHelper { get; set; }

        public EditModel(Stockholm_Syndrome_Web.Data.ApplicationDbContext context)
        {
            _context = context;
            userRoleHelper = new UserRoleHelper(_context);
            applicationRoles = userRoleHelper.GetRoles();
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        public UserRoles _userRoles { get; set; }
        public List<ApplicationRole> applicationRoles { get; set; }

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

            
            _userRoles = userRoleHelper.GetRoles(ApplicationUser.Id);
            
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.Attach(ApplicationUser).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ApplicationUserExists(ApplicationUser.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return RedirectToPage("./Index");
        }

        private bool ApplicationUserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
