using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EVE.SingleSignOn.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stockholm_Syndrome_Web.Controllers;
using Stockholm_Syndrome_Web.Data;
using Stockholm_Syndrome_Web.Models;

namespace Stockholm_Syndrome_Web.Pages.StructureManagement
{
	[Authorize(Roles = "Admin,Director,FuelAdmin,FuelManager")]
	public class StructureFuelLevelsModel : PageModel
    {
        private readonly Stockholm_Syndrome_Web.Data.ApplicationDbContext _context;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ISingleSignOnClient _client;
		private readonly IOptions<SSOUserLogin> _configUser;

		public const int LowFuelDays = 15;

		// Set Destination
		public List<EveCharacter> eveCharacters;

		public StructureFuelLevelsModel(Stockholm_Syndrome_Web.Data.ApplicationDbContext context,
									IHttpClientFactory clientFactory,
									ISingleSignOnClient client,
									IOptions<SSOUserLogin> configUser)
        {
            _context = context;
			_httpClientFactory = clientFactory;
			_client = client;
			_configUser = configUser;

			//foreach(var toon in _context.EveCharacters)
			//{
			//	if (toon.User == User.Identity)
			//	{
			//		eveCharacters.Add(toon);
			//	}
			//}
        }

        public IList<Structure> Structure { get; set; }

        public async Task OnGetAsync()
        {
			var RoleToQuery = "";
			if (User.IsInRole("FuelAdmin") || User.IsInRole("Admin"))
			{
				Structure = await _context.Structures.ToListAsync();
				Structure = Structure.OrderBy(s => s.StructureName).ToList();
				return;
			}
			else if(User.IsInRole("FuelManager"))
			{
				RoleToQuery = "FuelManager";
			}
			else
			{
				RoleToQuery = "";
			}

			Structure = await _context.Structures.Where(s => s.RoleNeededToManage == RoleToQuery)
				.OrderBy(s => s.StructureName).ToListAsync();
        }

		public bool AnyLowFuelStructures()
		{
			foreach(var st in _context.Structures)
			{
				DateTime date = DateTime.UtcNow;
				try
				{
					date = DateTime.Parse(st.FuelExpires, new System.Globalization.CultureInfo("en-US"));
				}
				catch
				{
					// Its ok if this fails
				}

				float days = (date - DateTime.UtcNow).Days;

				if(days < LowFuelDays)
				{
					return true;
				}
			}

			return false;
		}

		public async Task<int> CalcFuelLevel(long structureId )
		{
			var st = await _context.Structures.FirstOrDefaultAsync(s => s.StructureId == structureId);

			if (st == null) return 0;

			if (st.FuelExpires == null) return 0;

			var date = DateTime.Parse(st.FuelExpires, new System.Globalization.CultureInfo("en-US"));

			float days = (date - DateTime.UtcNow).Days;

			if(days > 30)
			{
				return 100;
			}
			else if(days < 0)
			{
				return 0;
			}
			else
			{

				return (int)((days / 30) * 100) ;
			}

		}

		public async Task<string> CalcFuelColor(long structureId)
		{
			int fuelLevel = await CalcFuelLevel(structureId);
			if(fuelLevel > 50)
			{
				return "bg-success";
			}
			else if (fuelLevel <= 50 && fuelLevel > 25)
			{
				return "bg-warning";
			}
			else
			{
				return "bg-danger";
			}
		}

		public async Task<int> CalcFuelDays(long structureId)
		{
			var st = await _context.Structures.FirstOrDefaultAsync(s => s.StructureId == structureId);

			if (st == null) return 0;

			if (st.FuelExpires == null) return 0;

			var date = DateTime.Parse(st.FuelExpires, new System.Globalization.CultureInfo("en-US"));

			float days = (date - DateTime.UtcNow).Days;

			return (int)days;
		}

		public string GetStructureType(int id)
		{
			switch (id)
			{
				// Citadels
				case 35832:
					return "Astrahus";

				case 35833:
					return "Fortizar";

				case 35834:
					return "Keepstar";

				// Refineries
				case 35835:
					return "Athanor";

				case 35836:
					return "Tatara";

				// Engineering Complexes
				case 35825:
					return "Raitaru";

				case 35826:
					return "Azbel";

				case 35827:
					return "Sotiyo";

				default:
					return "";
			}
		}

		public string FuelLevelToString(Structure structure)
		{
			string fuelLevel;

			if (structure.FuelExpires == null)
			{
				fuelLevel = "Low Power";
			}
			else
			{
				fuelLevel = DateTime.Parse(structure.FuelExpires, new System.Globalization.CultureInfo("en-US")).ToString("yyyy/MM/dd HH:mm:ss");
			}
			return fuelLevel;
		}

		public async Task<string> FuelInDays(long structureId)
		{
			var st = await _context.Structures.FirstOrDefaultAsync(s => s.StructureId == structureId);

			if (st == null) return "";

			if (st.FuelExpires == null) return "";

			var date = DateTime.Parse(st.FuelExpires, new System.Globalization.CultureInfo("en-US"));

			float days = (date - DateTime.UtcNow).Days;

			if (days > 1)
			{
				return days.ToString() + " Days";
			}
			else
			{
				return days.ToString() + " Day";
			}	
		}

		/// <summary>
		/// Set autopilot destination
		/// </summary>
		/// <param name="NavId"></param>
		/// <param name="character"></param>
		/// <returns></returns>
		public async Task<IActionResult> OnPostSetDestination(long NavId, EveCharacter character = null)
		{
			ESINavigation nav = new ESINavigation
			{
				add_to_beginning = false,
				clear_other_waypoints = false,
				destination_id = NavId
			};

			// TODO: Re-design !!!
			// How do i get this from the user ???
			//character.CharacterRefreshToken = "Iyp/AzsebE+x8hSGpmKpDQ==";

			//Tokens refresh = await _client.RefreshAsync(new Uri("https://login.eveonline.com/v2/oauth/token"),
			//				_configUser.Value.ClientId,
			//				_configUser.Value.ClientSecret,
			//				character.CharacterRefreshToken)
			//				.ConfigureAwait(false);


			//using (var _httpClient = _httpClientFactory.CreateClient())
			//{

			//	_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + refresh.AccessToken);

			//	var navData = JsonConvert.SerializeObject(nav);

			//	//StringContent payload = new StringContent(navData);

			//	//var response = await _httpClient.PostAsync("https://esi.evetech.net/latest/ui/autopilot/waypoint/", payload);
			//	using (var wc = new WebClient())
			//	{

			//		wc.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + refresh.AccessToken);

			//		string StructureUrl = $"https://esi.evetech.net/latest/ui/autopilot/waypoint/?add_to_beginning=false&clear_other_waypoints=false&datasource=tranquility&destination_id={NavId}";

			//		//var result = wc.DownloadString(StructureUrl);
			//		var result = wc.UploadString(StructureUrl, navData);

			//	}

			//	//if (response == null)
			//	//{
			//	//	// Something went wrong!
			//	//	// TODO: Maybe add error message?
			//	//}

			//	//character = JsonConvert.DeserializeObject<SSOCharacter>(response);
			//}

			return Redirect("/StructureManagement/FuelLevels");
		}
	}
}
