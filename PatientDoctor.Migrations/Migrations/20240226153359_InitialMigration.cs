using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientDoctor.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "Type",
            //    schema: "Admin",
            //    table: "Medicine",
            //    newName: "MedicineTypeId");

            //migrationBuilder.AddColumn<string>(
            //    name: "TabletMg",
            //    schema: "Admin",
            //    table: "MedicineType",
            //    type: "text",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "DoctorId",
            //    schema: "Admin",
            //    table: "Medicine",
            //    type: "text",
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddColumn<string>(
            //    name: "MedicineName",
            //    schema: "Admin",
            //    table: "Medicine",
            //    type: "text",
            //    nullable: false,
            //    defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DoctorCheckUpFeeDetails",
                schema: "Admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorFee = table.Column<int>(type: "integer", nullable: false),
                    DoctorId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorCheckUpFeeDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorCheckUpFeeDetails",
                schema: "Admin");

            migrationBuilder.DropColumn(
                name: "TabletMg",
                schema: "Admin",
                table: "MedicineType");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                schema: "Admin",
                table: "Medicine");

            migrationBuilder.DropColumn(
                name: "MedicineName",
                schema: "Admin",
                table: "Medicine");

            migrationBuilder.RenameColumn(
                name: "MedicineTypeId",
                schema: "Admin",
                table: "Medicine",
                newName: "Type");
        }
    }
}
