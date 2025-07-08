using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchedulerApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBookingTimeTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Bookings");

            migrationBuilder.Sql("""
                ALTER TABLE "Bookings" ALTER COLUMN "StartTime" TYPE timestamp with time zone
                USING (TIMESTAMP WITH TIME ZONE '2000-01-01' + "StartTime");
            """);

            migrationBuilder.Sql("""
                ALTER TABLE "Bookings" ALTER COLUMN "EndTime" TYPE timestamp with time zone
                USING (TIMESTAMP WITH TIME ZONE '2000-01-01' + "EndTime");
            """);

            migrationBuilder.Sql("""
                ALTER TABLE "Bookings" ALTER COLUMN "CreatedAt" TYPE timestamp with time zone
                USING (TIMESTAMP WITH TIME ZONE '2000-01-01' + "CreatedAt");
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "Bookings" ALTER COLUMN "StartTime" TYPE time without time zone
                USING ("StartTime"::time);
            """);

            migrationBuilder.Sql("""
                ALTER TABLE "Bookings" ALTER COLUMN "EndTime" TYPE time without time zone
                USING ("EndTime"::time);
            """);

            migrationBuilder.Sql("""
                ALTER TABLE "Bookings" ALTER COLUMN "CreatedAt" TYPE time without time zone
                USING ("CreatedAt"::time);
            """);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Bookings",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
