using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewPropertiesToServiceRequestEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Accessories",
                table: "ServiceRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Device",
                table: "ServiceRequests",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestNumber",
                table: "ServiceRequests",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accessories",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "Device",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "RequestNumber",
                table: "ServiceRequests");
        }
    }
}
