using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Data
{
	public class OpsTags
	{
		public int Id { get; set; }

		public OpsTag Tag { get; set; }

		public Ops Ops { get; set; }
	}
}
