using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Data
{
	public class ApplicationRole : IdentityRole<int>
	{
		[MaxLength(2000)]
		public string Description { get; set; }

		public bool AutoManaged { get; set; }

		public ApplicationRole() { }

		public ApplicationRole(string name)
		{
			Name = name;
		}

		public ApplicationRole(string name, string description)
		{
			Name = name;
			Description = description;
		}

	}
}
