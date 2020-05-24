using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Data
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

		[MaxLength(5000)]
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		public List<Participants> Participants { get; set; }

	}
}
