using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MMBackUp.Client.Shared;
using MMBackUp.Server.Data;
using MMBackUp.Shared;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace MMBackUp.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FlightInfoController : ControllerBase
	{

        private DataContext _context;

		public FlightInfoController(DataContext context) {
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<ServiceResponse<List<Shared.FlightInfo>>>> GetFlightInfo() {
			OracleDataReader dr;
			List<Shared.FlightInfo> flightList = new List<Shared.FlightInfo>();
			try {
				OracleCommand cmd = new OracleCommand();
				cmd.CommandTimeout = 60;
				cmd.Connection = new OracleConnection("Data Source=ORAODSSBUTB01P.azul.corp:1521/SBM;Persist Security Info=True;User ID=USER_SABRE;Password=ZQAJPEI7jXVp81b4");
				cmd.Connection.Open();
				cmd.CommandType = CommandType.Text;
                cmd.CommandText = $@"SELECT airport_scheduleddeparture || '/'|| airport_scheduledarrival AS key_p,time_originaldeparture AS fltdate, flightid_flightnumber AS flt_nbr, equipment_aircraftregistration AS acreg,equipment_aircrafttype AS actype, flightlegreferencenumber AS flt_id, time_actualblockoff AS out_, time_actualtakeoff AS off_, time_actualtouchdown             AS on_, time_actualblockon               AS in_, time_scheduleddeparture as STD, time_scheduledarrival as STA, status, specialstatus FROM mm_ods.flightleg WHERE flightid_date >= SYSDATE - 1 ORDER BY time_originaldeparture ASC";
				// LIMITED DATA -  cmd.CommandText = $@"SELECT airport_scheduleddeparture|| '/'|| airport_scheduledarrival AS key_p,time_originaldeparture AS fltdate,flightid_flightnumber  AS flt_nbr,equipment_aircraftregistration   AS acreg,equipment_aircrafttype AS actype,flightlegreferencenumber     AS flt_id,time_actualblockoff    AS out_,time_actualtakeoff AS off_,time_actualtouchdown   AS on_,time_actualblockon AS in_,time_scheduleddeparture  AS std,time_scheduledarrival  AS sta,status,specialstatus FROM mm_ods.flightleg WHERE flightid_date >= SYSDATE - 1 AND ( equipment_aircraftregistration = 'PR-AXO' OR equipment_aircraftregistration = 'PR-YRA' OR equipment_aircraftregistration = 'PR-YRB' OR equipment_aircraftregistration = 'PR-YRC') ORDER BY time_originaldeparture ASC";
                dr = cmd.ExecuteReader();
				var flightInfoList = new ObservableCollection<Shared.FlightInfo>();
				while (dr.Read())
				{
					//Safe Gets prevent Errors when reading NULL from DB
					var tempFlight = new Shared.FlightInfo {
						key_p = SafeGetString(dr, 0),
						fltdate = SafeDateTime(dr, 1),
						flt_nbr = SafeGetString(dr, 2),
						acreg = SafeGetString(dr, 3),
						actype = SafeGetString(dr, 4),
						flt_id = dr.GetInt64(dr.GetOrdinal("flt_id")),
						out_ = SafeDateTime(dr, 6),
						off_ = SafeDateTime(dr, 7),
						on_ = SafeDateTime(dr, 8),
						in_ = SafeDateTime(dr, 9),
						STD = SafeDateTime(dr, 10),
						STA = SafeDateTime(dr, 11),
						status = SafeGetString(dr, 12),
						specialstatus = SafeGetString(dr, 13)
					};
					flightInfoList.Add(tempFlight);
				}
				flightList = flightInfoList.ToList();
				Console.WriteLine("Succesfully Selected \n");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed in the try or FI Controller");
			}
			var response = new ServiceResponse<List<Shared.FlightInfo>>() {
				Data = flightList
			};
			return Ok(response);
		}

		[HttpGet("{days:int}")]
		public async Task<ActionResult<ServiceResponse<List<Shared.FlightInfo>>>> GetFlightInfo(int days)
		{
			OracleDataReader dr;
			List<Shared.FlightInfo> flightList = new List<Shared.FlightInfo>();
			try
			{
				DateTime tempdate = DateTime.Today.AddDays(days);
				string sqlDate = tempdate.ToString("dd/MM/yyyy hh:mm tt");
				OracleCommand cmd = new OracleCommand();
				cmd.CommandTimeout = 60;
				cmd.Connection = new OracleConnection("Data Source=ORAODSSBUTB01P.azul.corp:1521/SBM;Persist Security Info=True;User ID=USER_SABRE;Password=ZQAJPEI7jXVp81b4");
				cmd.Connection.Open();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = @"SELECT airport_scheduleddeparture || '/'|| airport_scheduledarrival AS key_p,time_originaldeparture AS fltdate, flightid_flightnumber AS flt_nbr, equipment_aircraftregistration AS acreg,equipment_aircrafttype AS actype, flightlegreferencenumber AS flt_id, time_actualblockoff AS out_, time_actualtakeoff AS off_, time_actualtouchdown             AS on_, time_actualblockon AS in_, time_scheduleddeparture as STD, time_scheduledarrival as STA, status, specialstatus FROM mm_ods.flightleg WHERE flightid_date >= SYSDATE - 1 AND flightid_date <= :sqlDate ORDER BY time_originaldeparture ASC";

               // LIMITED DATA  - cmd.CommandText = $@"SELECT airport_scheduleddeparture|| '/'|| airport_scheduledarrival AS key_p,time_originaldeparture AS fltdate,flightid_flightnumber  AS flt_nbr,equipment_aircraftregistration   AS acreg,equipment_aircrafttype AS actype,flightlegreferencenumber     AS flt_id,time_actualblockoff    AS out_,time_actualtakeoff AS off_,time_actualtouchdown   AS on_,time_actualblockon AS in_,time_scheduleddeparture  AS std,time_scheduledarrival  AS sta,status,specialstatus FROM mm_ods.flightleg WHERE flightid_date >= SYSDATE - 1 AND flightid_date <= :sqlDate AND ( equipment_aircraftregistration = 'PR-AXO' OR equipment_aircraftregistration = 'PR-YRA' OR equipment_aircraftregistration = 'PR-YRB' OR equipment_aircraftregistration = 'PR-YRC') ORDER BY time_originaldeparture ASC";


                //defining the paremeter fields
                OracleParameter dateParam = new OracleParameter();
				dateParam.ParameterName = ":sqlDate";
				dateParam.Direction = ParameterDirection.Input;
				dateParam.DbType = DbType.DateTime;
				dateParam.Value = tempdate;
				cmd.Parameters.Add(dateParam);

				dr = cmd.ExecuteReader();
				var flightInfoList = new ObservableCollection<Shared.FlightInfo>();
				while (dr.Read())
				{
                    //Safe Gets prevent Errors when reading NULL from DB
                    var tempFlight = new Shared.FlightInfo
					{
						key_p = SafeGetString(dr, 0),
						fltdate = SafeDateTime(dr, 1),
						flt_nbr = SafeGetString(dr, 2),
						acreg = SafeGetString(dr, 3),
						actype = SafeGetString(dr, 4),
						flt_id = dr.GetInt64(dr.GetOrdinal("flt_id")),
						out_ = SafeDateTime(dr, 6),
						off_ = SafeDateTime(dr, 7),
						on_ = SafeDateTime(dr, 8),
						in_ = SafeDateTime(dr, 9),
						STD = SafeDateTime(dr, 10),
						STA = SafeDateTime(dr, 11),
						status = SafeGetString(dr, 12),
						specialstatus = SafeGetString(dr, 13)
					};
					flightInfoList.Add(tempFlight);
				}
				flightList = flightInfoList.ToList();
				Console.WriteLine("Succesfully Selected \n");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed in the try or FI Controller");
			}
			var response = new ServiceResponse<List<Shared.FlightInfo>>()
			{
				Data = flightList
			};
			return Ok(response);
		}
		protected DateTime? SafeDateTime(OracleDataReader reader, int colIndex)
		{
			if (!reader.IsDBNull(colIndex))
				return reader.GetDateTime(colIndex);
			return (DateTime?)null;
		}
		protected string SafeGetString(OracleDataReader reader, int colIndex)
		{
			if (!reader.IsDBNull(colIndex))
				return reader.GetString(colIndex);
			return string.Empty;
		}
	}
}
