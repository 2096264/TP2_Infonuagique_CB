using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidAuto.Utilisateurs.API.Migrations
{
    public partial class AjoutNumeroTelephone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumeroTelephone",
                table: "Utilisateurs",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroTelephone",
                table: "Utilisateurs");
        }
    }
}
