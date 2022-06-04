using Microsoft.EntityFrameworkCore.Migrations;

namespace tutoria_net_core.Migrations
{
    public partial class Extend_IndentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ayudaPass",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ayudaPass",
                table: "AspNetUsers");
        }
    }
}
