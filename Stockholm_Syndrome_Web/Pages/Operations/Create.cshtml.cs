using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESIHelperLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Serilog;
using SSDataLibrary;
using Stockholm_Syndrome_Web.Models;

namespace Stockholm_Syndrome_Web.Pages.Operations
{
    [Authorize(Roles = "OpsCreate,OpsManager")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager { get; set; }

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            Tags = _context.Tags.ToList();
            _userManager = userManager;
            FCs = new List<SelectListItem>();
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

            OpsStatus = new List<SelectListItem>();
            foreach(var opsStatus in OpsHelper.OpsStatus)
			{
                var item = new SelectListItem();
                item.Text = opsStatus;
                item.Value = opsStatus;
                if(opsStatus == "Bravo")
				{
                    item.Selected = true;
				}
                OpsStatus.Add(item);
			}

            StructureLayer = new List<SelectListItem>();
            foreach (var structureLayer in OpsHelper.StructureLayer)
            {
                StructureLayer.Add(new SelectListItem()
                {
                    Text = structureLayer,
                    Value = structureLayer
                });

            }

            StructureType = new List<SelectListItem>();
            foreach (var structureType in OpsHelper.StructureType)
			{
                StructureType.Add(new SelectListItem()
                {
                    Text = structureType,
                    Value = structureType 
                });

            }

            StructureStatus = new List<SelectListItem>();
            foreach(var structureStatus in OpsHelper.StructureStatus)
			{
                StructureStatus.Add(new SelectListItem()
                {
                    Text = structureStatus,
                    Value = structureStatus
                });
			}

            return Page();
        }

        [BindProperty]
        public Ops Ops { get; set; }

        public List<OpsTag> Tags { get; set; }

        //[BindProperty]
        //public List<SelectListItem> UiTagsChecked { get; protected set; }

        public List<SelectListItem> FCs { get; set; }

        public List<SelectListItem> OpsStatus { get; set; }

        public List<SelectListItem> StructureLayer { get; set; }

        public List<SelectListItem> StructureType { get; set; }

		public List<SelectListItem> StructureStatus { get; set; }

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
