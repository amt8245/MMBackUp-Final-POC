using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MMBackUp.Shared
{
	[PrimaryKey(nameof(flt_id))]                  //Primary Key for each flight, must be unique

	//All Data from Flights - not all Shown on Page
	public class FlightInfo
	{
		public string? key_p { get; set; }

		public DateTime? fltdate { get; set; }

		public string? flt_nbr { get; set; }

		public string? acreg { get; set; }

		public string? actype { get; set; }

		public long? flt_id { get; set; }

		public DateTime? out_ { get; set; }

		public DateTime? off_ { get; set; }

		public DateTime? on_ { get; set; }

		public DateTime? in_ { get; set; }

		public DateTime? STD { get; set; }

		public DateTime? STA { get; set; }

		public string? status { get; set; }

		public string? specialstatus { get; set; }
	}

}
