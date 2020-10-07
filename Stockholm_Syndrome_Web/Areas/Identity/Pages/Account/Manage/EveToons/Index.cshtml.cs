using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDataLibrary;

namespace Stockholm_Syndrome_Web.Pages.YourUser
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<EveCharacter> EveCharacter { get; set; }
        public string State { get; set; }

        public async Task OnGetAsync(string state = null)
        {
            State = state;
            EveCharacter = await _context.EveCharacters.Where(c => c.User.UserName == User.Identity.Name).ToListAsync();
        }
    }
}
