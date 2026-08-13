using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESportsTournament.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjustandoEntidadesESports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipes_Torneios_TorneioId",
                table: "Equipes");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Nick",
                table: "Usuarios",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "TorneioId",
                table: "Equipes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Equipes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Abreviacao",
                table: "Equipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Nick",
                table: "Usuarios",
                column: "Nick",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipes_Nome",
                table: "Equipes",
                column: "Nome",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipes_Torneios_TorneioId",
                table: "Equipes",
                column: "TorneioId",
                principalTable: "Torneios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipes_Torneios_TorneioId",
                table: "Equipes");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Nick",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Equipes_Nome",
                table: "Equipes");

            migrationBuilder.DropColumn(
                name: "Nick",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Abreviacao",
                table: "Equipes");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "TorneioId",
                table: "Equipes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Equipes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipes_Torneios_TorneioId",
                table: "Equipes",
                column: "TorneioId",
                principalTable: "Torneios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
