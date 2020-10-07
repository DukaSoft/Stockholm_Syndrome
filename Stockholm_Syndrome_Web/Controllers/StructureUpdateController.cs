using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EVE.SingleSignOn.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SSDataLibrary;
using Stockholm_Syndrome_Web.Helpers;
using Stockholm_Syndrome_Web.Models;

namespace Stockholm_Syndrome_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StructureUpdateController : ControllerBase
    {
		private ApplicationDbContext _dbContext;
		private readonly IOptions<SSOCorp> _configCorp;
		private ISingleSignOnClient _client;

		public StructureUpdateController(ApplicationDbContext dbContext, IOptions<SSOCorp> configCorp, ISingleSignOnClient client)
		{
			_dbContext = dbContext;
			_configCorp = configCorp;
			_client = client;
		}

		/// <summary>
		/// Updates the Alliance Structures with a CronJob, so that the user doesn't have to wait for it.
		/// </summary>
		/// <returns></returns>
		public async Task<HttpResponseMessage> UpdateStructures()
		{
			List<Corp> corps = _dbContext.Alliance.ToList();
			foreach(var corp in corps)
			{
				await EvaluateCorpStructures(corp);
			}

			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		/// <summary>
		/// Compare the list of structures to the list we have in our database
		/// Add new structures if any
		/// Update changes to names
		/// And let the user worry about deleting structures
		/// </summary>
		/// <param name="corp"></param>
		private async Task EvaluateCorpStructures(Corp corp)
		{
			// Refresh our access token
			//corp = ESIHelper.RefreshAccess(corp, _configCorp);
			Tokens refresh = await _client.RefreshAsync(new Uri("https://login.eveonline.com/v2/oauth/token"),
				_configCorp.Value.ClientId, _configCorp.Value.ClientSecret, corp.RefreshToken).ConfigureAwait(false);

			corp.AccessToken = refresh.AccessToken;

			// ESI Structure List		
			List<Structure> structures = ESIStructureHelper.GetStructures(corp);

			// Database Structure List
			List<Structure> ListOfStructures = _dbContext.Structures.ToList();

			// Compare our List and the ESI List
			foreach(var structure in structures)
			{
				bool found = false;
				for(int i = 0; i < ListOfStructures.Count; i++)
				{
					if(ListOfStructures[i].StructureId == structure.StructureId)
					{
						ListOfStructures[i].StructureName = structure.StructureName;
						ListOfStructures[i].FuelExpires = structure.FuelExpires;
						_dbContext.Update(ListOfStructures[i]);
						found = true;
					}
				}
				if (found == false)
				{
					structure.RoleNeededToManage = "Admin";
					_dbContext.Structures.Add(structure);
				}
			}

			// Save changes
			await _dbContext.SaveChangesAsync();
		}
	}
}