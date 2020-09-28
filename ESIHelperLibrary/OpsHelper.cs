using System;
using System.Collections.Generic;
using System.Text;

namespace ESIHelperLibrary
{
	public static class OpsHelper
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
}
