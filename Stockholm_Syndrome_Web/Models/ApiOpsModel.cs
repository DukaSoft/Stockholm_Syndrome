using Stockholm_Syndrome_Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Models
{
	public class ApiOpsModel
	{

		public int Id { get; set; }

		public DateTime OpsTime { get; set; }

		public int? StagingSystemId { get; set; }
		public string StagingSystemName { get; set; }

		public int? TargetSystemId { get; set; }
		public string TargetSystemName { get; set; }

		public int? FcId { get; set; }
		public string FcName { get; set; }

		public string CreatorId { get; set; }
		public string Creator { get; set; }

		public string OpStatus { get; set; }

		public string StructureOwner { get; set; }
		public string StructureType { get; set; }
		public string StructureName { get; set; }
		public string StructureLayer { get; set; }
		public string StructureStatus { get; set; }

		public string Description { get; set; }

		//public List<Participants> Participants { get; set; }

	}
}
