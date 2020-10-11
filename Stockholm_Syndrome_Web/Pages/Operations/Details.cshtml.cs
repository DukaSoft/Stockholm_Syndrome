using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Pages.Operations
{
    [Authorize(Roles = "MemberOfAlliance")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Ops Ops { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ops = await _context.Ops.Include(u => u.Creator).FirstOrDefaultAsync(m => m.Id == id);

            if (Ops == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<string> Creator(int id)
		{
            var ops = await _context.Ops.Include(u => u.Creator).ThenInclude(e => e.EveCharacter).FirstOrDefaultAsync(m => m.Id == id);
            string creator = "";

            if (ops.Creator != null)
            {
                creator = ops.Creator.DiscordName;

                foreach (var character in ops.Creator.EveCharacter)
                {
                    if (character.DefaultToon == true)
                    {
                        creator = character.CharacterName;
                    }
                }
            }

            return creator;
		}
    }
}
