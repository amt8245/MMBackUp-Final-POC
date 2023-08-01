using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MMBackUp.Client.Shared;
using MMBackUp.Shared;
using FlightInfo = MMBackUp.Shared.FlightInfo;

namespace MMBackUp.Server.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) {
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<FlightInfo>().HasData(
				new FlightInfo
				{
					key_p = "k",
					fltdate = new DateTime(2012, 12, 31, 16, 45, 0),
					flt_nbr = "0",
					acreg = "help me ",
					actype = "enemy ac 130 above",
					flt_id = 1,
					out_ = new DateTime(2012, 12, 31, 16, 45, 0),
					off_ = new DateTime(2012, 12, 31, 16, 45, 0),
					on_ = new DateTime(2012, 12, 31, 16, 45, 0),
					in_ = new DateTime(2012, 12, 31, 16, 45, 0),
					STD = new DateTime(2012, 12, 31, 16, 45, 0),
					STA = new DateTime(2012, 12, 31, 16, 45, 0),
					status = "oka",
					specialstatus = "csgonades.net"
				}
				//,
				//new FlightInfo
				//{
				//	FlightLegId = 2,
				//	DepartureAirportCode = "not obama D:",
				//	LegSequenceNumber = 70,
				//	ArrivalAirportCode = "not obama",
				//	AircraftCode = "god help me",
				//	AircraftConfiguration = "oh its configured alright",
				//	Prbd = "...",
				//	ServiceTypeCode = "in the beginning, there was nothing",
				//	CockpitCrewEmployer = "10/11",
				//	UtcFlightDate = new DateTime(2022, 12, 31, 16, 45, 0),
				//	ExpirationTime = new DateTime(2022, 12, 31, 16, 45, 0),
				//	MaintenanceTime = new DateTime(2022, 12, 31, 16, 45, 0),
				//	AircraftOwner = "joe biden",
				//	CompanyCode = "nicepeople.com"
				//}
				);
		}
			
		public DbSet<FlightInfo> FlightInfos { get; set; }

	}
}
