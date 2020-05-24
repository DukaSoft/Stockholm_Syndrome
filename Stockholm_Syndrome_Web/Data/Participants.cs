using Stockholm_Syndrome_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Data
{
	public class Participants
	{
		[Key]
		public int Id { get; set; }

		public Ops Ops { get; set; }

		public Participation Answer { get; set; } = Participation.NoAnswer;

		public ApplicationUser User { get; set; }
	}
}
