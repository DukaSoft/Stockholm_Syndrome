using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Data
{
	public class EveGuide
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		[Display(Name ="Source")]
		public string Src { get; set; }

	}
}
