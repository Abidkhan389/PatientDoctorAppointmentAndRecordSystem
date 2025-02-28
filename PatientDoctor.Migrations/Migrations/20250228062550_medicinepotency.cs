using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientDoctor.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class medicinepotency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TabletMg",
                schema: "Admin",
                table: "MedicineType");

            migrationBuilder.CreateTable(
                name: "MedicinePotency",
                schema: "Admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Potency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicineTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinePotency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicinePotency_MedicineType_MedicineTypeId",
                        column: x => x.MedicineTypeId,
                        principalSchema: "Admin",
                        principalTable: "MedicineType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePotency_MedicineTypeId",
                schema: "Admin",
                table: "MedicinePotency",
                column: "MedicineTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicinePotency",
                schema: "Admin");

            migrationBuilder.AddColumn<string>(
                name: "TabletMg",
                schema: "Admin",
                table: "MedicineType",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
