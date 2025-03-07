using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientDoctor.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddingTimeSlotInAppointmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeSlot",
                schema: "Admin",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeSlot",
                schema: "Admin",
                table: "Appointment");
        }
    }
}
