using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Data
{
	public class Structure
	{
		[Key]
		public long Id { get; set; }

		[Display(Name = "Structure Id")]
		public long StructureId { get; set; }

		[Display(Name="Structure Name")]
		public string StructureName { get; set; }

		[Display(Name = "Structure Type")]
		public int TypeId { get; set; }

		[Display(Name = "Role needed to manage")]
		public string RoleNeededToManage { get; set; }

		[Display(Name = "Fuel Expire Date")]
		public string FuelExpires { get; set; }
	}
}
