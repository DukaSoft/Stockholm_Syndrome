using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Data
{
	public class OpsTag
	{
		[Key]
		public int Id { get; set; }

		[MaxLength(200)]
		public string Name { get; set; }

		public string Color { get; set; }
	}
}
