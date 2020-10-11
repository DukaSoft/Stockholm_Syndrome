using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Data
{
	public class Corp
	{
		[Key]
		public int Id { get; set; }

		[Display(Name = "Corp Name")]
		[MaxLength(2000)]
		public string CorpName { get; set; }

		[Display(Name = "Corp Id")]
		public int CorpId { get; set; }

		[Display(Name = "Access Token")]
		[MaxLength(2000)]
		public string AccessToken { get; set; }

		[Display(Name = "Refresh Token")]
		[MaxLength(2000)]
		public string RefreshToken { get; set; }

	}
}
