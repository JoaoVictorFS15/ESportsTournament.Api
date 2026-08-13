using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESportsTournament.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoCapitaoNaEquipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CapitaoId",
                table: "Equipes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapitaoId",
                table: "Equipes");
        }
    }
}
