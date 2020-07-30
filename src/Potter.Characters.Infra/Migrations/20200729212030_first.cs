using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Potter.Characters.Infra.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Character",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    Role = table.Column<string>(maxLength: 100, nullable: false),
                    School = table.Column<string>(maxLength: 200, nullable: false),
                    House = table.Column<string>(maxLength: 100, nullable: false),
                    Patronus = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Character", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Character");
        }
    }
}
