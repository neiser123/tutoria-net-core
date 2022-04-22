using Microsoft.EntityFrameworkCore.Migrations;

namespace tutoria_net_core.Migrations
{
    public partial class rutaFotoAmigo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "rutaFoto",
                table: "Amigos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rutaFoto",
                table: "Amigos");
        }
    }
}
