using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace SSDataLibrary
{
	public class Ops
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[DisplayName("Date & Time")]
		public DateTime OpsTime { get; set; }

		public int? StagingSystemId { get; set; }
		[MaxLength(500)]
		[DisplayName("Staging System")]
		public string StagingSystemName { get; set; }

		public int? TargetSystemId { get; set; }
		[MaxLength(500)]
		[DisplayName("Target System")]
		public string TargetSystemName { get; set; }

		public int? FcId { get; set; }
		[MaxLength(500)]
		[DisplayName("Fleet Commander")]
		public string FcName { get; set; }

		public ApplicationUser Creator { get; set; }

		public List<OpsTags> OpTags { get; set; }

		[DisplayName("Op Status")]
		[MaxLength(500)]
		public string OpStatus { get; set; }

		[DisplayName("Owner")]
		[MaxLength(2000)]
		public string StructureOwner { get; set; }

		[DisplayName("Type")]
		[MaxLength(500)]
		public string StructureType { get; set; }

		[DisplayName("Name")]
		[MaxLength(2000)]
		public string StructureName { get; set; }

		[DisplayName("Layer")]
		[MaxLength(500)]
		public string StructureLayer { get; set; }

		[DisplayName("Structure Status")]
		[MaxLength(500)]
		public string StructureStatus { get; set; }

		[MaxLength(5000)]
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		public List<Participants> Participants { get; set; }

	}
}
