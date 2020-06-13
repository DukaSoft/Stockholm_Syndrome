using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Pages.Operations
{
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
                FCs.Insert(0, new SelectListItem("TBD", "", true));
            }
			else
			{
                FCs.Insert(0, new SelectListItem("TBD", ""));
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

            return RedirectToPage("./Index");
        }

        private bool OpsExists(int id)
        {
            return _context.Ops.Any(e => e.Id == id);
        }
    }
}
