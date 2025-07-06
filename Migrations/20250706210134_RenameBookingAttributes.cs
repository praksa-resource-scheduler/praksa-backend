using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchedulerApp.Migrations
{
    /// <inheritdoc />
    public partial class RenameBookingAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Rooms_Room_id",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_User_id",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "User_id",
                table: "Bookings",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Start_time",
                table: "Bookings",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "Room_id",
                table: "Bookings",
                newName: "RoomId");

            migrationBuilder.RenameColumn(
                name: "End_time",
                table: "Bookings",
                newName: "EndTime");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Bookings",
                newName: "Purpose");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "Bookings",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_User_id",
                table: "Bookings",
                newName: "IX_Bookings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_Room_id",
                table: "Bookings",
                newName: "IX_Bookings_RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Bookings",
                newName: "User_id");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Bookings",
                newName: "Start_time");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Bookings",
                newName: "Room_id");

            migrationBuilder.RenameColumn(
                name: "Purpose",
                table: "Bookings",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "Bookings",
                newName: "End_time");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Bookings",
                newName: "Created_At");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                newName: "IX_Bookings_User_id");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                newName: "IX_Bookings_Room_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Rooms_Room_id",
                table: "Bookings",
                column: "Room_id",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_User_id",
                table: "Bookings",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
