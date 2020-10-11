using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDataLibrary;

namespace Stockholm_Syndrome_Web.Pages.Operations
{
    [Authorize(Roles = "MemberOfAlliance")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Ops> Ops { get;set; }

        public async Task OnGetAsync()
        {
            Ops = await _context.Ops.OrderBy(d => d.OpsTime).Where(t=> t.OpsTime.AddMinutes(10) > DateTime.UtcNow).ToListAsync();
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
