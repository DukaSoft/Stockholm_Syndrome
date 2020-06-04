using Stockholm_Syndrome_Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Models
{
	public class UserRoles
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string EveCharacter { get; set; }
		public List<int> Roles { get; set; }
	}
}
