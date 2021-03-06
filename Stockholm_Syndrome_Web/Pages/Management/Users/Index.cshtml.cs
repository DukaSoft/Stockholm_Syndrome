using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;
using Stockholm_Syndrome_Web.Models;

namespace Stockholm_Syndrome_Web.Pages.Management.Users
{
    [Authorize(Roles = "Admin,Director")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;

        }

        public List<UserRoles> userRoles = new List<UserRoles>();

        public void OnGetAsync()
        {
			foreach (var user in _context.Users)
			{
				var roles = from ur in _context.UserRoles
							join r in _context.Roles on ur.RoleId equals r.Id
							where ur.UserId == user.Id
							select new { ur.UserId, ur.RoleId, r.Name };

				List<int> rolesInt = new List<int>();
				foreach (var role in roles)
				{
					rolesInt.Add(role.RoleId);
				}

				var evecharacter = _context.EveCharacters.Where(e => e.User == user).FirstOrDefault(c => c.DefaultToon == true);
				string ec = "";
				if (evecharacter != null)
				{
					ec = evecharacter.CharacterName;
				}

				userRoles.Add(new UserRoles
				{
					UserId = user.Id,
					UserName = user.UserName,
					Roles = rolesInt,
					EveCharacter = ec
				});
			}
		}

		public async Task<string> RoleName(int roleId)
		{
			string roleName;

			roleName = (await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId)).Name;

			return roleName;
		}

		public string RoleDescription(int roleId)
		{
			string roleDescription;

			roleDescription = _context.Roles.FirstOrDefault(r => r.Id == roleId).Description;//.Replace(" ", "\u2800");

			return roleDescription;
		}
	}
}
