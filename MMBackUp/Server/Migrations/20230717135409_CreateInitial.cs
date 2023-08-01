using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MMBackUp.Server.Migrations
{
    /// <inheritdoc />
    public partial class CreateInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightInfos",
                columns: table => new
                {
                    FlightLegId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DepartureAirportCode = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LegSequenceNumber = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ArrivalAirportCode = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    AircraftCode = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    AircraftConfiguration = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Prbd = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ServiceTypeCode = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CabinCrewEmployer = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CockpitCrewEmployer = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UtcFlightDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MaintenanceTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    AircraftOwner = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CompanyCode = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightInfos", x => x.FlightLegId);
                });

            migrationBuilder.InsertData(
                table: "FlightInfos",
                columns: new[] { "FlightLegId", "AircraftCode", "AircraftConfiguration", "AircraftOwner", "ArrivalAirportCode", "CabinCrewEmployer", "CockpitCrewEmployer", "CompanyCode", "DepartureAirportCode", "ExpirationTime", "LegSequenceNumber", "MaintenanceTime", "Prbd", "ServiceTypeCode", "UtcFlightDate" },
                values: new object[,]
                {
                    { 1L, "god help me", "oh its configured alright", "saddaam hussein", "not obama", null, "9/11", "PH.com", "obama", null, 69, null, "...", "in the beginning, there was nothing", null },
                    { 2L, "god help me", "oh its configured alright", "saddaam hussein", "not obama", null, "9/11", "PH.com", "not obama D:", null, 79, null, "...", "in the beginning, there was nothing", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightInfos");
        }
    }
}
