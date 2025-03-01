using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientDoctor.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class addmedicinepotencyByIdInMedicineTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "medicineTypePotencyId",
                schema: "Admin",
                table: "Medicine",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "medicineTypePotencyId",
                schema: "Admin",
                table: "Medicine");
        }
    }
}
