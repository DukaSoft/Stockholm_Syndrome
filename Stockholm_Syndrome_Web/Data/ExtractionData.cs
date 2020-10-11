using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stockholm_Syndrome_Web.Data
{
	public class ExtractionData
	{
		[Key]
		public int Id { get; set; }
		[Display(Name = "Chunk Ready")]
		public string Chunk_arrival_time { get; set; }

		[Display(Name = "Extraction Started")]
		public string Extraction_start_time { get; set; }

		[Display(Name="Moon Id")]
		public int Moon_id { get; set; }

		[Display(Name = "Auto Extract")]
		public string Natural_decay_time { get; set; }

		[Display(Name = "Structure")]
		public long Structure_id { get; set; }

		public string LastExtraction { get; set; }

		public string Notes { get; set; }
	}
}
