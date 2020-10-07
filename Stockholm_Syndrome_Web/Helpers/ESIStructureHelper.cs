using Newtonsoft.Json;
using Serilog;
using SSDataLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Stockholm_Syndrome_Web.Helpers
{
	public static class ESIStructureHelper
	{
		public static List<Structure> GetStructures(Corp corp)
		{
			List<Structure> StructureList = new List<Structure>();

			using (var wc = new WebClient())
			{
				string StructureUrl = $"https://esi.evetech.net/latest/corporations/{corp.CorpId}/structures/";
				string StructureNameUrl = $"https://esi.evetech.net/latest/universe/structures/";

				wc.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + corp.AccessToken);

				var result = wc.DownloadString(StructureUrl);

				var Structures = JsonConvert.DeserializeObject<List<dynamic>>(result);

				result = String.Empty;

				foreach (var Structure in Structures)
				{
					Structure structure = new Structure();
					structure.FuelExpires = Structure.fuel_expires;
					structure.StructureId = Structure.structure_id;
					try
					{
						result = wc.DownloadString(StructureNameUrl + structure.StructureId);
					}
					catch (WebException e)
					{
						Log.Warning(e.Message);
					}
					var structureNameData = JsonConvert.DeserializeObject<dynamic>(result);

					structure.StructureName = structureNameData.name;
					structure.TypeId = Structure.type_id;

					StructureList.Add(structure);
				}
			}

			return StructureList;
		}
	}
}
