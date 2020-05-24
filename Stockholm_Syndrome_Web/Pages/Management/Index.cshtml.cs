﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;
using Stockholm_Syndrome_Web.Models;

namespace Stockholm_Syndrome_Web.Pages.Management
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly Stockholm_Syndrome_Web.Data.ApplicationDbContext _context;
        
        public IndexModel(Stockholm_Syndrome_Web.Data.ApplicationDbContext context)
        {
            _context = context;
            foreach(var user in _context.Users)
            {
                var roles = from ur in _context.UserRoles
                            join r in _context.Roles on ur.RoleId equals r.Id
                            where ur.UserId == user.Id
                            select new { ur.UserId, ur.RoleId, r.Name };

                List<int> rolesInt = new List<int>();
                foreach(var role in roles)
                {
                    rolesInt.Add(role.RoleId);
                }

                userRoles.Add(new UserRoles
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = rolesInt
                });
            }
        }

        public List<UserRoles> userRoles = new List<UserRoles>();

        public void OnGetAsync()
        {

        }
    }
}