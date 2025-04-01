using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TpepPickupDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TpepDropoffDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PassengerCount = table.Column<int>(type: "int", nullable: false),
                    TripDistance = table.Column<double>(type: "float", nullable: false),
                    StoreAndFwdFlag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PULocationID = table.Column<int>(type: "int", nullable: false),
                    DOLocationID = table.Column<int>(type: "int", nullable: false),
                    FareAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TipAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");
        }
    }
}
