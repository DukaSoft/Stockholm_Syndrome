using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;

namespace Stockholm_Syndrome_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EveCharactersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private string Token = "%¤&#%¤#%¤!23536#¤45634&dsggseeh332533";

        public EveCharactersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EveCharacters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiToon>>> GetEveCharacters(string token)
        {
            if(token != Token)
			{
                return NotFound();
			}

			List<ApiToon> apiToons = new List<ApiToon>();
			List<EveCharacter> eveCharacters = await _context.EveCharacters.Include(u => u.User).ToListAsync();

			foreach(var toon in eveCharacters)
			{
				ApiToon t = new ApiToon
				{
					Id = toon.Id,
					Name = toon.CharacterName,
					DiscordId = toon.User.DiscordId,
					CorpId = toon.CharacterCorpId ?? 0,
					AllianceId = toon.CharacterAllianceId ?? 0,
					ESIScope = toon.ESIScope,
					RefreshToken = toon.CharacterRefreshToken,
					DefaultToon = toon.DefaultToon
				};

				apiToons.Add(t);
			}

			return apiToons;
        }

		public class ApiToon
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public string DiscordId { get; set; }
			public int CorpId { get; set; }
			public int AllianceId { get; set; }
			public string ESIScope { get; set; }
			public string RefreshToken { get; set; }
			public bool DefaultToon { get; set; }

		}

		//// GET: api/EveCharacters/5
		//[HttpGet("{id}")]
		//public async Task<ActionResult<EveCharacter>> GetEveCharacter(int id)
		//{
		//    var eveCharacter = await _context.EveCharacters.FindAsync(id);

		//    if (eveCharacter == null)
		//    {
		//        return NotFound();
		//    }

		//    return eveCharacter;
		//}

		//// PUT: api/EveCharacters/5
		//// To protect from overposting attacks, enable the specific properties you want to bind to, for
		//// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		//[HttpPut("{id}")]
		//public async Task<IActionResult> PutEveCharacter(int id, EveCharacter eveCharacter)
		//{
		//    if (id != eveCharacter.Id)
		//    {
		//        return BadRequest();
		//    }

		//    _context.Entry(eveCharacter).State = EntityState.Modified;

		//    try
		//    {
		//        await _context.SaveChangesAsync();
		//    }
		//    catch (DbUpdateConcurrencyException)
		//    {
		//        if (!EveCharacterExists(id))
		//        {
		//            return NotFound();
		//        }
		//        else
		//        {
		//            throw;
		//        }
		//    }

		//    return NoContent();
		//}

		//// POST: api/EveCharacters
		//// To protect from overposting attacks, enable the specific properties you want to bind to, for
		//// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		//[HttpPost]
		//public async Task<ActionResult<EveCharacter>> PostEveCharacter(EveCharacter eveCharacter)
		//{
		//    _context.EveCharacters.Add(eveCharacter);
		//    await _context.SaveChangesAsync();

		//    return CreatedAtAction("GetEveCharacter", new { id = eveCharacter.Id }, eveCharacter);
		//}

		//// DELETE: api/EveCharacters/5
		//[HttpDelete("{id}")]
		//public async Task<ActionResult<EveCharacter>> DeleteEveCharacter(int id)
		//{
		//    var eveCharacter = await _context.EveCharacters.FindAsync(id);
		//    if (eveCharacter == null)
		//    {
		//        return NotFound();
		//    }

		//    _context.EveCharacters.Remove(eveCharacter);
		//    await _context.SaveChangesAsync();

		//    return eveCharacter;
		//}

		//private bool EveCharacterExists(int id)
		//{
		//    return _context.EveCharacters.Any(e => e.Id == id);
		//}
	}
}
