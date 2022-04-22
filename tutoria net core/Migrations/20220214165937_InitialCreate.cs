using Microsoft.EntityFrameworkCore.Migrations;

namespace tutoria_net_core.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amigos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(maxLength: 100, nullable: false),
                    email = table.Column<string>(nullable: false),
                    ciudad = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amigos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Amigos",
                columns: new[] { "Id", "ciudad", "email", "nombre" },
                values: new object[] { 1, 0, "neiser@hotmail.com", "neiser" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amigos");
        }
    }
}
