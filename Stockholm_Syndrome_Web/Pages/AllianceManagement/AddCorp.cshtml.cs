﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Pages.AllianceManagement
{
	[Authorize(Roles = "Admin")]
	public class AddCorpModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddCorpModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Corp Corp { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Alliance.Add(Corp);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}