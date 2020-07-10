using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Models
{
	public class OpsHelper
	{
		public static List<string> OpsStatus = new List<string>()
		{
			"Alpha",
			"Bravo",
			"Charlie"
		};

		public static List<string> StructureLayer = new List<string>()
		{
			"N/A",
			"Shield",
			"Armor",
			"Hull"
		};

		public static List<string> StructureStatus = new List<string>()
		{
			"N/A",
			"Online",
			"Low Power",
			"Abandoned",
			"Anchoring"
		};

		public static List<string> StructureType = new List<string>()
		{
			"N/A",
			"Astrahus",
			"Athanor",
			"Azbel",
			"Fortizar",
			"Keepstar",
			"Raitaru",
			"Sotiyo",
			"Tatara",
			"Customs Office"
		};
	}

	public enum Participation
	{
		NoAnswer = 0,
		Accepted = 1,
		Rejected = 2,
		Maybe = 3
	}
}
