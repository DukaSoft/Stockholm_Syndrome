using Microsoft.AspNetCore.Identity;
using Stockholm_Syndrome_Web.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Data
{
	public class ApplicationUser : IdentityUser<int>
	{
		public DateTime CreationDate { get; set; }
		public List<EveCharacter> EveCharacter { get; set; }
		[MaxLength(2000)]
		public string DiscordId { get; set; }
		[MaxLength(2000)]
		public string DiscordName { get; set; }

	}
}
