using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Data
{
	public class EveCharacter
	{
		[Key]
		public int Id { get; set; }

		public ApplicationUser User { get; set; }

		public int CharacterId { get; set; }

		[Display(Name = "Character Name")]
		[MaxLength(2000)]
		public string CharacterName { get; set; }

		public int? CharacterCorpId { get; set; }

		[Display(Name = "Corp Name")]
		[MaxLength(2000)]
		public string CharacterCorpName { get; set; }

		public int? CharacterAllianceId { get; set; }

		[Display(Name = "Alliance Name")]
		[MaxLength(2000)]
		public string CharacterAllianceName { get; set; }

		[Display(Name ="ESI Scope")]
		[MaxLength(2000)]
		public string ESIScope { get; set; }

		[Display(Name = "Refresh Token")]
		[MaxLength(2000)]
		public string CharacterRefreshToken { get; set; }

		[Display(Name = "Main Toon")]
		public bool DefaultToon { get; set; }
	}
}
