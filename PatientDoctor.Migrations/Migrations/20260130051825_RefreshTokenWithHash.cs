using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientDoctor.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenWithHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "UserRefreshTokens",
                newName: "TokenHash");

            migrationBuilder.RenameColumn(
                name: "Revoked",
                table: "UserRefreshTokens",
                newName: "IsUsed");

            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                table: "UserRefreshTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRevoked",
                table: "UserRefreshTokens");

            migrationBuilder.RenameColumn(
                name: "TokenHash",
                table: "UserRefreshTokens",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "IsUsed",
                table: "UserRefreshTokens",
                newName: "Revoked");
        }
    }
}
