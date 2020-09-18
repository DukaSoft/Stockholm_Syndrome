using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using Stockholm_Syndrome_Web.Data;
using Stockholm_Syndrome_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Helpers
{
	public class ESIHelper
	{

		public static string CurrentESIScopeForUsers =
			"publicData esi-skills.read_skills.v1 esi-ui.write_waypoint.v1 esi-fittings.read_fittings.v1 esi-fittings.write_fittings.v1 esi-fleets.read_fleet.v1";

		public static string CurrentESIScopeForCorp =
			"esi-universe.read_structures.v1 esi-corporations.read_structures.v1 esi-industry.read_corporation_mining.v1 esi-characters.read_notifications.v1";

		public class ESITokenRefresh
		{
			public string grant_type { get; set; }
			public string refresh_token { get; set; }
		}

		public class ESITokenReply
		{
			public string access_token { get; set; }
			public string token_type { get; set; }
			public int expires_in { get; set; }
			public string refresh_token { get; set; }
		}

		public static Corp RefreshAccess(Corp corp, IOptions<SSOCorp> configCorp)
		{
			Corp RefreshedCorp = new Corp
			{
				Id				= corp.Id,
				CorpId			= corp.CorpId,
				CorpName		= corp.CorpName,
				RefreshToken	= corp.RefreshToken
			};

			const string AuthUrl = "https://login.eveonline.com/oauth/token";
			using (var wc = new WebClient())
			{
				// Refresh the token
				ESITokenRefresh et = new ESITokenRefresh
				{
					grant_type		= "refresh_token",
					refresh_token	= RefreshedCorp.RefreshToken
				};

				var jsondata = JsonConvert.SerializeObject(et);


				var authcode = configCorp.Value.ClientId + ":" + configCorp.Value.ClientSecret;
				var plainTextBytes = Encoding.UTF8.GetBytes(authcode);
				var aHeader = Convert.ToBase64String(plainTextBytes);
				wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
				wc.Headers.Add(HttpRequestHeader.Authorization, "Basic " + aHeader);

				
				Uri uri = new Uri(AuthUrl);

				var result = wc.UploadString(uri, jsondata);

				ESITokenReply reply = new ESITokenReply();
				//reply = JsonConvert.DeserializeObject<ESITokenReply>(result);

				RefreshedCorp.AccessToken = reply.access_token;
			}

			return RefreshedCorp;
		}

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
					catch(WebException e)
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
