using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetapp.Migrations
{
    public partial class wertyui : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartyHalls",
                columns: table => new
                {
                    PartyHallID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyHalls", x => x.PartyHallID);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartyHallID = table.Column<int>(type: "int", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationInMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK_Bookings_PartyHalls_PartyHallID",
                        column: x => x.PartyHallID,
                        principalTable: "PartyHalls",
                        principalColumn: "PartyHallID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PartyHalls",
                columns: new[] { "PartyHallID", "Availability", "Capacity", "Name" },
                values: new object[,]
                {
                    { 1, true, 100, "Elegant Banquet Hall" },
                    { 2, true, 50, "Cozy Party Room" },
                    { 3, true, 200, "Grand Celebration Hall" },
                    { 4, true, 150, "Lavish Ballroom" },
                    { 5, true, 80, "Rustic Barn Venue" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PartyHallID",
                table: "Bookings",
                column: "PartyHallID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "PartyHalls");
        }
    }
}
