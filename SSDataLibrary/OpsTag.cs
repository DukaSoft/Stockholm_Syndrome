using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSDataLibrary
{
	public class OpsTag
	{
		[Key]
		public int Id { get; set; }

		[MaxLength(200)]
		public string Name { get; set; }

		[MaxLength(2000)]
		public string Description { get; set; }

		public string Color { get; set; }
	}
}
