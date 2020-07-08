using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Pages.Operations
{
	[Authorize(Roles = "MemberOfAlliance")]
	public class CompletedModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public CompletedModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public IList<Ops> Ops { get; set; }

		public async Task OnGetAsync()
		{
			Ops = await _context.Ops.OrderBy(d => d.OpsTime).ToListAsync();
		}

		public async Task<string> Creator(int id)
		{
			var ops = await _context.Ops.Include(u => u.Creator).ThenInclude(e => e.EveCharacter).FirstOrDefaultAsync(m => m.Id == id);
			string creator = ops.Creator.DiscordName;

			foreach (var character in ops.Creator.EveCharacter)
			{
				if (character.DefaultToon == true)
				{
					creator = character.CharacterName;
				}
			}

			return creator;
		}
	}
}
