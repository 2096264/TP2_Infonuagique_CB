using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidAuto.Vehicules.API.Migrations
{
    public partial class MigrationInitialeVehicules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicules",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Constructeur = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Modele = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    AnneeFabrication = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    NbrSieges = table.Column<int>(type: "INTEGER", nullable: false),
                    Couleur = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Niv = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    NomImage1 = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    NomImage2 = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    EstVendu = table.Column<bool>(type: "INTEGER", nullable: false),
                    Prix = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicules", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicules");
        }
    }
}
