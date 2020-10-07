using EVE.SingleSignOn.Core;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SSDataLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Models
{

	public class ESINavigation
	{
		public bool add_to_beginning { get; set; }
		public bool clear_other_waypoints { get; set; }
		public long destination_id { get; set; }
	}
}
