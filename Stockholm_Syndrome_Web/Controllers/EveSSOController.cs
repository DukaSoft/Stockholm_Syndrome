using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EVE.SingleSignOn.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SSDataLibrary;
using Stockholm_Syndrome_Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using ESIHelperLibrary;

namespace Stockholm_Syndrome_Web.Controllers
{
	public class EveSSOController : Controller
	{
		private readonly ISingleSignOnClient _client;
		private readonly IOptions<SSOUserLogin> _config;
		private readonly IOptions<SSOCorp> _configCorp;
		private readonly ApplicationDbContext _context;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly IHttpClientFactory _httpClientFactory;

		public EveSSOController(ISingleSignOnClient client,
				IOptions<SSOUserLogin> config,
				IOptions<SSOCorp> configCorp,
				ApplicationDbContext context,
				SignInManager<ApplicationUser> signInManager,
				UserManager<ApplicationUser> userManager,
				RoleManager<ApplicationRole> roleManager,
				IHttpClientFactory httpClientFactory)
		{
			_client = client;
			_config = config;
			_configCorp = configCorp;
			_context = context;
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;
			_httpClientFactory = httpClientFactory;
		}

		/// <summary>
		/// Used when adding a new Eve Character to the current user
		/// </summary>
		/// <returns></returns>
		[Route("EveSSO/Login")]
		public IActionResult Login()
		{
			// Create Redirect URL
			var authURL = _client.GetAuthenticationUrl(new Uri("https://login.eveonline.com/v2/oauth/authorize"),
				_config.Value.ClientId,
				ESIHelper.CurrentESIScopeForUsers,
				_config.Value.OurCallbackUrl,
				"SSOLogin");

			// Redirect to CCP SSO
			return Redirect(authURL);
		}

		//[Route("EveSSO/Recruit")]
		//public IActionResult RecruitLogin()
		//{
		//	// Create Redirect URL
		//	var authURL = _client.GetAuthenticationUrl(new Uri("https://login.eveonline.com/v2/oauth/authorize"),
		//		_config.Value.ClientId,
		//		"publicData esi-skills.read_skills.v1 esi-ui.write_waypoint.v1",
		//		_config.Value.OurCallbackUrl,
		//		"Recruitment");

		//	// Redirect to CCP SSO
		//	return Redirect(authURL);
		//}

		/// <summary>
		/// Used when adding a new managed corporation to the database
		/// </summary>
		/// <returns></returns>
		[Route("EveSSO/AddCorp")]
		public IActionResult AddCorp()
		{
			// Create Redirect URL
			var authUrl = _client.GetAuthenticationUrl(new Uri("https://login.eveonline.com/v2/oauth/authorize"),
				_configCorp.Value.ClientId,
				ESIHelper.CurrentESIScopeForCorp,
				_configCorp.Value.OurCallbackUrl,
				"SSOAddCorp");

			// Redirect to CCP SSO
			return Redirect(authUrl);
		}

		/// <summary>
		/// Callbacks from Eve Online
		/// </summary>
		/// <param name="code"></param>
		/// <param name="state">Whats the call about?</param>
		/// <returns></returns>
		[Authorize]
		[Route("EveSSO/Callback")]
		public async Task<IActionResult> Callback(string code, string state)
		{
			if (state == "SSOLogin")
			{
				// This is a normal SSO Login from a user
				if (await SSOLogin(code) == true)
				{
					await _signInManager.SignInAsync(await _userManager.GetUserAsync(User), false);
					// Return the user to Home
					return LocalRedirect("/Identity/Account/Manage/EveToons");
				}

				// Something went wrong!
				return LocalRedirect("/Identity/Account/Manage/EveToons");

			}
			//else if (state == "Recruitment")
			//{
			//	// This is a normal SSO Login from a user with Recruitment
			//	if (await SSOLogin(code) == true)
			//	{
			//		await _signInManager.SignInAsync(await _userManager.GetUserAsync(User), false);
			//		// Return the user to Home
			//		return LocalRedirect("/Enlist");
			//	}

			//	// Something went wrong!
			//	return NotFound();
			//}
			else if (state == "SSOAddCorp")
			{
				if (await SSOAddCorp(code) == true)
				{
					return RedirectToPage("/AllianceManagement/Index");
				}

				// Something went wrong!
				return NotFound();
			}
			else
			{
				return NotFound();
			}
		}

		/// <summary>
		/// Used for re-evaluating all user roles
		/// </summary>
		/// <returns></returns>
		[Route("EveSSO/EvaluateRoles")]
		public async Task<IActionResult> EvaluateRoles()
		{
			try
			{
				await UpdateCorpIds();
			}
			catch (Exception e)
			{
				Log.Error("Error trying to update corp Id's");
				return BadRequest(e);
			}
			try
			{
				await CheckRoles();
			}
			catch (Exception e)
			{
				Log.Error("Error trying to update user roles");
				return BadRequest(e);
			}

			return Ok();
		}

		/// <summary>
		/// Normal login using Eve SSO<br />
		/// Used for adding Eve Characters to the user profile
		/// </summary>
		/// <param name="code">The code that gets returned to us from CCP</param>
		/// <returns>
		/// True if successfull<br />
		/// Otherwise False
		/// </returns>
		private async Task<bool> SSOLogin(string code)
		{
			Tokens tokens;
			try
			{
				tokens = await _client.AuthorizeAsync(new Uri("https://login.eveonline.com/v2/oauth/token"),
					_config.Value.ClientId,
					_config.Value.ClientSecret,
					code)
					.ConfigureAwait(false);
			}
			catch(Exception e)
			{
				Log.Warning("Token error", e);
				return false;
			}

			using (var _httpClient = _httpClientFactory.CreateClient())
			{
				_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokens.AccessToken);
				var response = await _httpClient.GetStringAsync("https://login.eveonline.com/oauth/verify");

				if (response == null)
				{
					// Something went wrong!
					// TODO: Maybe add error message?

					return false;
				}

				SSOCharacter character = JsonConvert.DeserializeObject<SSOCharacter>(response);

				var user = await _userManager.GetUserAsync(User);

				if(user == null)
				{
					Log.Warning(new Exception("User was null"), $"The user was null for {character.CharacterName}", User);
					return false;
				}
				
				var ch = new EveCharacter
				{
					User = user,
					CharacterId = character.CharacterId,
					CharacterName = character.CharacterName,
					DefaultToon = false,
					ESIScope = ESIHelper.CurrentESIScopeForUsers,
					CharacterRefreshToken = tokens.RefreshToken
				};

				if (await _context.EveCharacters.FirstOrDefaultAsync(c => c.User == user) == null)
				{
					ch.DefaultToon = true;
				}

				var oldCharacter = await _context.EveCharacters.FirstOrDefaultAsync(c => c.CharacterId == ch.CharacterId);
				if (oldCharacter != null)
				{
					// Eve Character is already in the system
					oldCharacter.ESIScope = ESIHelper.CurrentESIScopeForUsers;
					_context.EveCharacters.Update(oldCharacter);
				}
				else
				{
					await _context.EveCharacters.AddAsync(ch);
				}

				await _context.SaveChangesAsync();

				await UpdateCorpIds(user);
				await CheckRoles(user);

				//await CreateRoles(); // Not for production!

				return true;
			}
		}

		/// <summary>
		/// Check roles of all users or only a specific one
		/// </summary>
		/// <param name="checkUser">The user that needs to be checked, null if all needs to be ckecked</param>
		/// <returns></returns>
		private async Task UpdateCorpIds(ApplicationUser checkUser = null)
		{

			// Get the list of EvE Characters to check
			List<int> CharacterIds = new List<int>();

			if (checkUser == null)
			{
				var characters = _context.EveCharacters.Include(u => u.User).ToList();
				// Check all users
				foreach (var character in characters)
				{
					if(character.User != null)
					{
						CharacterIds.Add(character.CharacterId);
					}
				}
			}
			else
			{
				// Check specific user
				foreach (var character in checkUser.EveCharacter)
				{
					CharacterIds.Add(character.CharacterId);
				}
			}

			using (var _httpClient = _httpClientFactory.CreateClient())
			{
				_httpClient.DefaultRequestHeaders
					.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

				var characterIdsJson = JsonConvert.SerializeObject(CharacterIds.ToArray());

				StringContent payload = new StringContent(characterIdsJson);

				var response = await _httpClient.PostAsync("https://esi.evetech.net/latest/characters/affiliation/", payload);

				if(!response.IsSuccessStatusCode)
				{
					// Bad stuff has happend
					Log.Information("Error getting affiliations from ESI {@response}", response);
					return;
				}
				string responseBody = await response.Content.ReadAsStringAsync();

				var affiliationData = JsonConvert.DeserializeObject<dynamic>(responseBody);

				// /universe/names/ - for corp names
				List<dynamic> corpIds = new List<dynamic>();
				List<dynamic> uniqueCorpIds = new List<dynamic>();
				foreach (var item in affiliationData)
				{
					corpIds.Add(item.corporation_id);
				}

				// Needs to be unique!
				uniqueCorpIds = corpIds.Distinct().ToList();

				var corpIdsJson = JsonConvert.SerializeObject(uniqueCorpIds.ToArray());
				payload = new StringContent(corpIdsJson);
				response = await _httpClient.PostAsync("https://esi.evetech.net/latest/universe/names/", payload);
				if(!response.IsSuccessStatusCode)
				{
					Log.Information("Error getting corp names from ESI {@response}", response);
					return;
				}
				responseBody = await response.Content.ReadAsStringAsync();
				var corpNames = JsonConvert.DeserializeObject<dynamic>(responseBody);

				if (corpNames == null)
				{
					return;
				}
				foreach (var item in affiliationData)
				{
					CharacterData toon = new CharacterData
					{
						alliance_id = item.alliance_id,
						character_id = item.character_id,
						corporation_id = item.corporation_id
					};

					EveCharacter eveCharacter = await _context.EveCharacters.
						FirstOrDefaultAsync(c => c.CharacterId == toon.character_id);

					eveCharacter.CharacterCorpId = toon.corporation_id;
					eveCharacter.CharacterAllianceId = toon.alliance_id;

					foreach (var corpItem in corpNames)
					{
						if (corpItem.id == eveCharacter.CharacterCorpId)
						{
							eveCharacter.CharacterCorpName = corpItem.name;
							_context.EveCharacters.Update(eveCharacter);
						}
					}
					_context.EveCharacters.Update(eveCharacter);
				}

				await _context.SaveChangesAsync();
			}
		}

		/// <summary>
		/// Updates roles of user(s) according to their corp Id
		/// </summary>
		/// <param name="user">The specific user that needs to update roles.
		/// Leave null if everyone needs to be updated</param>
		/// <returns></returns>
		private async Task CheckRoles(ApplicationUser user = null)
		{
			bool IsInAlliance = false;
			//bool IsInMenOfMayhem = false;
			//bool IsInSonoranSunLegion = false;
			//bool IsInHighsecHeroes = false;
			//bool IsInFURR = false;

			List<ApplicationUser> appUserList;
			if (user == null)
			{
				appUserList = _context.Users.ToList();
			}
			else
			{
				appUserList = new List<ApplicationUser>
				{
					user
				};
			}


			foreach (var appUser in appUserList)
			{
				List<EveCharacter> eveCharacters = _context.EveCharacters
					.Where(c => c.User == appUser).ToList();

				foreach (var toon in eveCharacters)
				{
					if (toon.CharacterAllianceId == CorpIdHelper.StockholmSyndrome)
					{
						IsInAlliance = true;
					}

					//if (toon.CharacterAllianceId == CorpIdHelper.FURR)
					//{
					//	IsInFURR = true;
					//}

					//switch (toon.CharacterCorpId)
					//{
					//	case CorpIdHelper.MenOfMayhem:
					//		IsInMenOfMayhem = true;
					//		IsInAlliance = true;
					//		break;

					//	case CorpIdHelper.SonoranSunLegion:
					//		IsInSonoranSunLegion = true;
					//		IsInAlliance = true;
					//		break;

					//		//case CorpIdHelper.HighsecHeroes:
					//		//	IsInHighsecHeroes = true;
					//		//	break;

					//}
					//// Admin role [Not for production]
					//if(toon.CharacterId == 1438938408)
					//{
					//	if (await _userManager.IsInRoleAsync(appUser, "Admin") == false)
					//	{
					//		await _userManager.AddToRoleAsync(appUser, "Admin");
					//	}
					//}
				}


				// Alliance Role
				if (IsInAlliance == true)
				{
					if (await _userManager.IsInRoleAsync(appUser, "MemberOfAlliance") == false)
					{
						await _userManager.AddToRoleAsync(appUser, "MemberOfAlliance");
					}
				}
				else
				{
					if (await _userManager.IsInRoleAsync(appUser, "MemberOfAlliance") == true)
					{
						await _userManager.RemoveFromRoleAsync(appUser, "MemberOfAlliance");
					}
				}

				//// Furr Role
				//if (IsInFURR == true)
				//{
				//	if (await _userManager.IsInRoleAsync(appUser, "MemberOfFURR") == false)
				//	{
				//		await _userManager.AddToRoleAsync(appUser, "MemberOfFURR");
				//	}
				//}
				//else
				//{
				//	if (await _userManager.IsInRoleAsync(appUser, "MemberOfFURR") == true)
				//	{
				//		await _userManager.RemoveFromRoleAsync(appUser, "MemberOfFURR");
				//	}
				//}

				// Reset for next test
				IsInAlliance = false;
				//IsInMenOfMayhem = false;
				//IsInSonoranSunLegion = false;
				//IsInHighsecHeroes = false;
				//IsInFURR = false;
			}
		}
		/// <summary>
		/// Used to check Character alliance and corp for role automation
		/// </summary>
		private class CharacterData
		{
			public int? alliance_id { get; set; }
			public int character_id { get; set; }
			public int? corporation_id { get; set; }
		}

		/// <summary>
		/// Add corp to management
		/// </summary>
		/// <param name="code">The code that gets returned to us from CCP</param>
		/// <returns></returns>
		private async Task<bool> SSOAddCorp(string code)
		{
			Tokens tokens = await _client.AuthorizeAsync(new Uri("https://login.eveonline.com/v2/oauth/token"),
				_configCorp.Value.ClientId,
				_configCorp.Value.ClientSecret,
				code)
				.ConfigureAwait(false);

			SSOCharacter character;

			using (var _httpClient = _httpClientFactory.CreateClient())
			{

				_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokens.AccessToken);
				var response = await _httpClient.GetStringAsync("https://login.eveonline.com/oauth/verify");

				if (response == null)
				{
					// Something went wrong!
					// TODO: Maybe add error message?

					return false;
				}

				character = JsonConvert.DeserializeObject<SSOCharacter>(response);
			}

			// Do a lookup for the corp name
			dynamic ReturnCorpId;
			using (var _httpClient2 = _httpClientFactory.CreateClient())
			{

				_httpClient2.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokens.AccessToken);
				string CorpId = $"https://esi.evetech.net/latest/characters/{character.CharacterId}/";

				var response2 = await _httpClient2.GetStringAsync(CorpId);

				if (response2 == null)
				{
					// Something went wrong!

				}

				ReturnCorpId = JsonConvert.DeserializeObject<dynamic>(response2);

				if (ReturnCorpId.corporation_id == null)
				{
					// Something went wrong!

				}
			}

			dynamic ReturnCorpName;
			using (var _httpClient3 = _httpClientFactory.CreateClient())
			{

				_httpClient3.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokens.AccessToken);

				var response3 = await _httpClient3.GetStringAsync(
						$"https://esi.evetech.net/latest/corporations/{ReturnCorpId.corporation_id}");

				ReturnCorpName = JsonConvert.DeserializeObject<dynamic>(response3);

				if (ReturnCorpName == null)
				{
					// Something went wrong!"

				}
			}

			Corp corp = new Corp
			{
				CorpId = ReturnCorpId.corporation_id,
				CorpName = ReturnCorpName.name,
				AccessToken = tokens.AccessToken,
				RefreshToken = tokens.RefreshToken
			};
			await _context.Alliance.AddAsync(corp);
			await _context.SaveChangesAsync();

			// It worked
			return true;
		}


		/// <summary>
		/// (Not for production) Temporary to add Roles to the Database
		/// </summary>
		/// <returns></returns>
		private async Task CreateRoles()
		{
			//await _roleManager.CreateAsync(new ApplicationRole("Admin", "Site Administrator"));
			//await _roleManager.CreateAsync(new ApplicationRole("Director", "Site Director"));
			//await _roleManager.CreateAsync(new ApplicationRole("MemberOfAlliance", "Member of Alliance"));
			//await _roleManager.CreateAsync(new ApplicationRole("FuelAdmin", "Fuel Administrator can see ALL infrastructure"));
			//await _roleManager.CreateAsync(new ApplicationRole("FuelManager", "Fuel Manager can see NON critical infrastructure"));
			//await _roleManager.CreateAsync(new ApplicationRole("Miner", "Miners are able to see Frack Schedule"));
			//await _roleManager.CreateAsync(new ApplicationRole("MemberOfFURR", "Member of our Newbie"));
			//await _roleManager.CreateAsync(new ApplicationRole("RecruitmentAdmin", "Recruitment admin can approve or reject Applications"));
			//await _roleManager.CreateAsync(new ApplicationRole("OpsCreate", "Able to Create Ops"));
			//await _roleManager.CreateAsync(new ApplicationRole("OpsManager", "Able to Edit/Delete Ops"));
			//await _roleManager.CreateAsync(new ApplicationRole("FC", "Fleet Commander"));

			await Task.CompletedTask;
		}
	}
}