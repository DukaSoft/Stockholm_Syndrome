using System;
using System.Collections.Generic;
using System.Text;

namespace ESIHelperLibrary
{
	public static class ESIHelper
	{
		public static string CurrentESIScopeForUsers =
			"publicData esi-skills.read_skills.v1 esi-ui.write_waypoint.v1 esi-fittings.read_fittings.v1 esi-fittings.write_fittings.v1 esi-fleets.read_fleet.v1";

		public static string CurrentESIScopeForCorp =
			"esi-universe.read_structures.v1 esi-corporations.read_structures.v1 esi-industry.read_corporation_mining.v1 esi-characters.read_notifications.v1";

	}
}
