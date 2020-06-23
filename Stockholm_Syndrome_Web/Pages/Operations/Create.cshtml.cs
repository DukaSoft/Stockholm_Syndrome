using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Serilog;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Pages.Operations
{
    [Authorize(Roles = "OpsCreate,OpsManager")]
    public class CreateModel : PageModel
    {
        private readonly Stockholm_Syndrome_Web.Data.ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager { get; set; }

        public CreateModel(Stockholm_Syndrome_Web.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            Tags = _context.Tags.ToList();
            _userManager = userManager;
            FCs = new List<SelectListItem>();

            Ops = new Ops();

            Ops.Description= @"Fleet Type: Alpha/Bravo/Charlie
Corp Name: 
Structure Type: 
Timer: Shield / Armor Hull
Structure Name: System - Structure name
Location: planet / moon / other celestial";

   //         UiTagsChecked = new List<SelectListItem>();
   //         foreach (var tag in _context.Tags.ToList())
			//{
   //             SelectListItem item = new SelectListItem
   //             {
   //                 Text = tag.Name,
   //                 Value = tag.Name,
   //             };

   //             UiTagsChecked.Add(item);
			//}
        }

        public async Task<IActionResult> OnGet()
        {
            var userlist = _context.Users.Include(e => e.EveCharacter).ToList();
            var DedGr = new SelectListGroup
            {
                Name = "Dedicated FC's"
            };
            var RestGr = new SelectListGroup
            {
                Name = "Everyone else"
            };

            foreach (var usr in userlist)
			{
                if(await _userManager.IsInRoleAsync(usr, "FC"))
				{
                    var defToon = usr.EveCharacter.FirstOrDefault(d => d.DefaultToon == true);
                    if (defToon != null)
					{
                        var toon = usr.EveCharacter.FirstOrDefault(d => d.DefaultToon == true).CharacterName;

						var item = new SelectListItem
                        {
                            Value = toon,
                            Text = toon,
                            Group = DedGr
					    };

					    FCs.Add(item);
					}
				}
				else
				{
                    var defToon = usr.EveCharacter.FirstOrDefault(d => d.DefaultToon == true);
                    if (defToon != null)
                    {
                        var toon = usr.EveCharacter.FirstOrDefault(d => d.DefaultToon == true).CharacterName;

                        var item = new SelectListItem
                        {
                            Value = toon,
                            Text = toon,
                            Group = RestGr
                        };

                        FCs.Add(item);
                    }
                }
			}
            FCs = FCs.OrderBy(g => g.Group.Name).ThenBy(v => v.Value).ToList();
            FCs.Insert(0, new SelectListItem("TBD", "TBD", true));

            return Page();
        }

        [BindProperty]
        public Ops Ops { get; set; }

        public List<OpsTag> Tags { get; set; }

        //[BindProperty]
        //public List<SelectListItem> UiTagsChecked { get; protected set; }

        public List<SelectListItem> FCs { get; set; }

        

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Set the creator of the Op
            Ops.Creator = await _userManager.GetUserAsync(User);

            _context.Ops.Add(Ops);

            // Add Tags

            await _context.SaveChangesAsync();

            Log.Information("Ops {@Ops} Created by {Username}", Ops, User.Identity.Name);

            return RedirectToPage("./Index");
        }
    }
}
