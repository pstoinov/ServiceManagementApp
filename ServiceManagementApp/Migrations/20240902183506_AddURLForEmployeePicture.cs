using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class AddURLForEmployeePicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "Employees",
                type: "nvarchar(2083)",
                maxLength: 2083,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_City_Street_Number_Block",
                table: "Addresses",
                columns: new[] { "City", "Street", "Number", "Block" },
                unique: true,
                filter: "[City] IS NOT NULL AND [Street] IS NOT NULL AND [Number] IS NOT NULL AND [Block] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_City_Street_Number_Block",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "Employees");
        }
    }
}
