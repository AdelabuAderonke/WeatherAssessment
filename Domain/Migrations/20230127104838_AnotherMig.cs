using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    public partial class AnotherMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weathers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemperatureC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weathers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Weathers",
                columns: new[] { "Id", "Date", "Description", "TemperatureC" },
                values: new object[] { 1, new DateTime(2023, 1, 25, 11, 48, 37, 813, DateTimeKind.Local).AddTicks(5065), "Chilly", "23C" });

            migrationBuilder.InsertData(
                table: "Weathers",
                columns: new[] { "Id", "Date", "Description", "TemperatureC" },
                values: new object[] { 2, new DateTime(2023, 1, 26, 11, 48, 37, 813, DateTimeKind.Local).AddTicks(5080), "Hot", "50C" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weathers");
        }
    }
}
