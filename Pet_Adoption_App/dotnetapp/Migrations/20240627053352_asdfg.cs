using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetapp.Migrations
{
    public partial class asdfg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    PetID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.PetID);
                });

            migrationBuilder.CreateTable(
                name: "PetAdopters",
                columns: table => new
                {
                    PetAdopterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PetID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetAdopters", x => x.PetAdopterID);
                    table.ForeignKey(
                        name: "FK_PetAdopters_Pets_PetID",
                        column: x => x.PetID,
                        principalTable: "Pets",
                        principalColumn: "PetID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "PetID", "Age", "Availability", "Name", "Type" },
                values: new object[,]
                {
                    { 1, 3, true, "Buddy", "Dog" },
                    { 2, 2, true, "Mittens", "Cat" },
                    { 3, 1, true, "Tweety", "Bird" },
                    { 4, 1, true, "Rocky", "Hamster" },
                    { 5, 2, true, "Whiskers", "Rabbit" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetAdopters_PetID",
                table: "PetAdopters",
                column: "PetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetAdopters");

            migrationBuilder.DropTable(
                name: "Pets");
        }
    }
}
