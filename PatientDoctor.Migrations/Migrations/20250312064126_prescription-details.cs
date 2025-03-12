using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientDoctor.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class prescriptiondetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prescription",
                schema: "Admin",
                columns: table => new
                {
                    PrescriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LeftVision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RightVision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeftMG = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RightMG = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeftEOM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RightEOM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeftOrtho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RightOrtho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeftTension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RightTension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeftAntSegment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RightAntSegment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeftDisc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RightDisc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeftMacula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RightMacula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeftPeriphery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RightPeriphery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Complaint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Plan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescription", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK_Prescription_AspNetUsers_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prescription_Patient_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "Admin",
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionMedicine",
                schema: "Admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrescriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Morning = table.Column<bool>(type: "bit", nullable: false),
                    Afternoon = table.Column<bool>(type: "bit", nullable: false),
                    Evening = table.Column<bool>(type: "bit", nullable: false),
                    Night = table.Column<bool>(type: "bit", nullable: false),
                    RepeatEveryHours = table.Column<int>(type: "int", nullable: true),
                    RepeatEveryTwoHours = table.Column<int>(type: "int", nullable: true),
                    DurationInDays = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionMedicine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrescriptionMedicine_Medicine_MedicineId",
                        column: x => x.MedicineId,
                        principalSchema: "Admin",
                        principalTable: "Medicine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrescriptionMedicine_Prescription_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalSchema: "Admin",
                        principalTable: "Prescription",
                        principalColumn: "PrescriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_DoctorId",
                schema: "Admin",
                table: "Prescription",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_PatientId",
                schema: "Admin",
                table: "Prescription",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedicine_MedicineId",
                schema: "Admin",
                table: "PrescriptionMedicine",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedicine_PrescriptionId",
                schema: "Admin",
                table: "PrescriptionMedicine",
                column: "PrescriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrescriptionMedicine",
                schema: "Admin");

            migrationBuilder.DropTable(
                name: "Prescription",
                schema: "Admin");
        }
    }
}
