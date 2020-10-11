using Stockholm_Syndrome_Web.Data;
using Stockholm_Syndrome_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Helpers
{
	public class UserRoleHelper
	{
        private readonly ApplicationDbContext _context;

        public UserRoleHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ApplicationRole> GetRoles()
        {
            var roles = _context.Roles.ToList();

            //List<int> rolesInt = new List<int>();
            //foreach (var role in roles)
            //{
            //    rolesInt.Add(role.RoleId);
            //}

            //UserRoles userRoles = new UserRoles()
            //{
            //    UserId = userId,
            //    UserName = _context.Users.FirstOrDefault(c => c.Id == userId).UserName,
            //    Roles = rolesInt
            //};

            
            return roles;
        }

        public UserRoles GetRoles(int userId)
		{
            var roles = from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where ur.UserId == userId
                        select new { ur.UserId, ur.RoleId, r.Name };

            List<int> rolesInt = new List<int>();
            foreach (var role in roles)
            {
                rolesInt.Add(role.RoleId);
            }
            
            UserRoles userRoles = new UserRoles()
            {
                UserId = userId,
                UserName = _context.Users.FirstOrDefault(c=> c.Id == userId).UserName,
                Roles = rolesInt
            };
            

            return userRoles;
        }
	}
}
