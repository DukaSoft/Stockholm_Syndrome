using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSDataLibrary;
using Stockholm_Syndrome_Web.Models;

namespace Stockholm_Syndrome_Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OpsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		// ToDo: Add propper code
		private const string _opToken = "ionoiesjn3489hnsiunue4ihhsuoyeh4uybuyo3bsbdjhtb4utb3nkjnwu4ht";

		public OpsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/Ops
		[HttpGet]
		public async Task<ActionResult<List<ApiOpsModel>>> GetOps(string token)
		{
			if (token != _opToken)
			{
				return NotFound();
			}

			var ExpireTime = DateTime.UtcNow.AddMinutes(20);

			var ops = await _context.Ops.Include(c => c.Creator).Include(t => t.OpTags).Where(d => d.OpsTime > ExpireTime).ToListAsync();
			List<ApiOpsModel> apiOpsModel = new List<ApiOpsModel>();

			foreach (var op in ops)
			{
				ApiOpsModel newOp = new ApiOpsModel
				{
					Id = op.Id,
					OpsTime = op.OpsTime,
					StagingSystemId = op.StagingSystemId,
					StagingSystemName = op.StagingSystemName,
					TargetSystemId = op.TargetSystemId,
					TargetSystemName = op.TargetSystemName,
					FcId = op.FcId,
					FcName = op.FcName,
					CreatorId = op.Creator.DiscordId,
					Creator = op.Creator.DiscordName,
					OpStatus = op.OpStatus,
					StructureOwner = op.StructureOwner,
					StructureType = op.StructureType,
					StructureName = op.StructureName,
					StructureLayer = op.StructureLayer,
					StructureStatus = op.StructureStatus,
					Description = op.Description
				};

				apiOpsModel.Add(newOp);
			}


			return Ok(apiOpsModel.ToList());
		}

		// GET: api/Ops/5/token
		[HttpGet("{id}/{token}")]
		public async Task<ActionResult<ApiOpsModel>> GetOps(int id, string token)
		{
			if (token != _opToken)
			{
				return NotFound();
			}

			if(OpsExists(id) == false)
			{
				return NotFound();
			}

			var op = await _context.Ops.Include(c => c.Creator).Include(t => t.OpTags).Include(p => p.Participants).FirstOrDefaultAsync(i => i.Id == id);
			ApiOpsModel apiOpsModel = new ApiOpsModel
			{
				Id = op.Id,
				OpsTime = op.OpsTime,
				StagingSystemId = op.StagingSystemId,
				StagingSystemName = op.StagingSystemName,
				TargetSystemId = op.TargetSystemId,
				TargetSystemName = op.TargetSystemName,
				FcId = op.FcId,
				FcName = op.FcName,
				CreatorId = op.Creator.DiscordId,
				Creator = op.Creator.DiscordName,
				OpStatus = op.OpStatus,
				StructureOwner = op.StructureOwner,
				StructureType = op.StructureType,
				StructureName = op.StructureName,
				StructureLayer = op.StructureLayer,
				StructureStatus = op.StructureStatus,
				Description = op.Description
			};

			if (op == null)
			{
				return NotFound();
			}

			return Ok(apiOpsModel);
		}

		// PUT: api/Ops/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		//[HttpPut("{id}")]
		//public async Task<IActionResult> PutOps(int id, Ops ops)
		//{
		//    if (id != ops.Id)
		//    {
		//        return BadRequest();
		//    }

		//    _context.Entry(ops).State = EntityState.Modified;

		//    try
		//    {
		//        await _context.SaveChangesAsync();
		//    }
		//    catch (DbUpdateConcurrencyException)
		//    {
		//        if (!OpsExists(id))
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

		// POST: api/Ops
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		//[HttpPost]
		//public async Task<ActionResult<Ops>> PostOps(Ops ops)
		//{
		//    _context.Ops.Add(ops);
		//    await _context.SaveChangesAsync();

		//    return CreatedAtAction("GetOps", new { id = ops.Id }, ops);
		//}

		// DELETE: api/Ops/5
		//[HttpDelete("{id}")]
		//public async Task<ActionResult<Ops>> DeleteOps(int id)
		//{
		//    var ops = await _context.Ops.FindAsync(id);
		//    if (ops == null)
		//    {
		//        return NotFound();
		//    }

		//    _context.Ops.Remove(ops);
		//    await _context.SaveChangesAsync();

		//    return ops;
		//}

		private bool OpsExists(int id)
		{
			return _context.Ops.Any(e => e.Id == id);
		}
	}
}
