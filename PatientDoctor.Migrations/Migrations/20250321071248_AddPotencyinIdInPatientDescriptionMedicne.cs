using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientDoctor.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddPotencyinIdInPatientDescriptionMedicne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedicine_Medicine_MedicineId",
                schema: "Admin",
                table: "PrescriptionMedicine");

            migrationBuilder.AddColumn<Guid>(
                name: "PotencyId",
                schema: "Admin",
                table: "PrescriptionMedicine",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedicine_PotencyId",
                schema: "Admin",
                table: "PrescriptionMedicine",
                column: "PotencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicine_MedicineTypeId",
                schema: "Admin",
                table: "Medicine",
                column: "MedicineTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicine_MedicineType_MedicineTypeId",
                schema: "Admin",
                table: "Medicine",
                column: "MedicineTypeId",
                principalSchema: "Admin",
                principalTable: "MedicineType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedicine_MedicinePotency_PotencyId",
                schema: "Admin",
                table: "PrescriptionMedicine",
                column: "PotencyId",
                principalSchema: "Admin",
                principalTable: "MedicinePotency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedicine_Medicine_MedicineId",
                schema: "Admin",
                table: "PrescriptionMedicine",
                column: "MedicineId",
                principalSchema: "Admin",
                principalTable: "Medicine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicine_MedicineType_MedicineTypeId",
                schema: "Admin",
                table: "Medicine");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedicine_MedicinePotency_PotencyId",
                schema: "Admin",
                table: "PrescriptionMedicine");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedicine_Medicine_MedicineId",
                schema: "Admin",
                table: "PrescriptionMedicine");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionMedicine_PotencyId",
                schema: "Admin",
                table: "PrescriptionMedicine");

            migrationBuilder.DropIndex(
                name: "IX_Medicine_MedicineTypeId",
                schema: "Admin",
                table: "Medicine");

            migrationBuilder.DropColumn(
                name: "PotencyId",
                schema: "Admin",
                table: "PrescriptionMedicine");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedicine_Medicine_MedicineId",
                schema: "Admin",
                table: "PrescriptionMedicine",
                column: "MedicineId",
                principalSchema: "Admin",
                principalTable: "Medicine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
