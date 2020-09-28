using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESIHelperLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;
using Stockholm_Syndrome_Web.Data;
using Stockholm_Syndrome_Web.Models;

namespace Stockholm_Syndrome_Web.Pages.Operations
{
    [Authorize(Roles = "OpsCreate,OpsManager")]
    public class EditModel : PageModel
    {
        private readonly Stockholm_Syndrome_Web.Data.ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager { get; set; }

        public EditModel(Stockholm_Syndrome_Web.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            FCs = new List<SelectListItem>();
        }

        [BindProperty]
        public Ops Ops { get; set; }

        public List<SelectListItem> FCs { get; set; }

        public List<SelectListItem> OpsStatus { get; set; }

        public List<SelectListItem> StructureLayer { get; set; }

        public List<SelectListItem> StructureType { get; set; }

        public List<SelectListItem> StructureStatus { get; set; }

        static bool allowEdit = false;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ops = await _context.Ops.Include(c => c.Creator).Include(t => t.OpTags).FirstOrDefaultAsync(m => m.Id == id);

            if (Ops == null)
            {
                return NotFound();
            }

            if(User.IsInRole("OpsCreate"))
			{
                // Check to see if the user is allowed to edit this op
                if(Ops.Creator != await _userManager.GetUserAsync(User))
				{
                    return Forbid();
				}
				else
				{
                    allowEdit = true;
                }
			}

            var userlist = _context.Users.Include(e => e.EveCharacter).ToList();
            var DedGr = new SelectListGroup
            {
                Name = "Dedicated FC's"
            };
            var RestGr = new SelectListGroup
            {
                Name = "Everyone else"
            };

            bool HasUserSelected = false;
            foreach (var usr in userlist)
            {
                if (await _userManager.IsInRoleAsync(usr, "FC"))
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

                        if(Ops.FcName == item.Value)
						{
                            item.Selected = true;
                            HasUserSelected = true;
                        }
						else
						{
                            item.Selected = false;
						}

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

                        if (Ops.FcName == item.Value)
                        {
                            item.Selected = true;
                            HasUserSelected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }

                        FCs.Add(item);
                    }
                }
            }
            FCs = FCs.OrderBy(g => g.Group.Name).ThenBy(v => v.Value).ToList();
            if(HasUserSelected != true)
			{
                FCs.Insert(0, new SelectListItem("TBD", "TBD", true));
            }
			else
			{
                FCs.Insert(0, new SelectListItem("TBD", "TBD"));
			}

            OpsStatus = new List<SelectListItem>();
            foreach (var opsStatus in OpsHelper.OpsStatus)
            {
                var item = new SelectListItem();
                item.Text = opsStatus;
                item.Value = opsStatus;
                if (opsStatus == Ops.OpStatus)
                {
                    item.Selected = true;
                }
                OpsStatus.Add(item);
            }

            StructureLayer = new List<SelectListItem>();
            foreach (var structureLayer in OpsHelper.StructureLayer)
            {
                var item = new SelectListItem();
                item.Text = structureLayer;
                item.Value = structureLayer;
                if(structureLayer == Ops.StructureLayer)
				{
                    item.Selected = true;
				}
                StructureLayer.Add(item);
            }

            StructureType = new List<SelectListItem>();
            foreach (var structureType in OpsHelper.StructureType)
            {
                var item = new SelectListItem();
                item.Text = structureType;
                item.Value = structureType;
                if(structureType == Ops.StructureType)
				{
                    item.Selected = true;
				}
                StructureType.Add(item);

            }

            StructureStatus = new List<SelectListItem>();
            foreach (var structureStatus in OpsHelper.StructureStatus)
            {
                var item = new SelectListItem();
                item.Text = structureStatus;
                item.Value = structureStatus;
                if(structureStatus == Ops.StructureStatus)
				{
                    item.Selected = true;
				}
                StructureStatus.Add(item);
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (User.IsInRole("OpsCreate"))
            {
                // Check to see if the user is allowed to edit this op

                if (allowEdit == false)
                {
                    return Forbid();
                }
            }

            _context.Attach(Ops).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpsExists(Ops.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Log.Information("Ops {@Ops} Edited by {Username}", Ops, User.Identity.Name);

            return RedirectToPage("./Index");
        }

        private bool OpsExists(int id)
        {
            return _context.Ops.Any(e => e.Id == id);
        }
    }
}
