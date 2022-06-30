using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidAuto.Vehicules.API.Migrations
{
    public partial class AjoutCodeUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeUnique",
                table: "Vehicules",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeUnique",
                table: "Vehicules");
        }
    }
}
