using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Models
{
	public class SSOUserLogin
	{
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string OurCallbackUrl { get; set; }
	}
	public class SSOCorp
	{
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string OurCallbackUrl { get; set; }
	}
}
