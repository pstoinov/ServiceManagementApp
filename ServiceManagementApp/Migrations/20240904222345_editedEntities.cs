using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class editedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_City_Street_Number_Block",
                table: "Addresses");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Phones",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId1",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "Emails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(254)",
                oldMaxLength: 254,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "Addresses",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Addresses",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ServiceId1",
                table: "Employees",
                column: "ServiceId1");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_City_Street_Number_Block",
                table: "Addresses",
                columns: new[] { "City", "Street", "Number", "Block" },
                unique: true,
                filter: "[Block] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Services_ServiceId1",
                table: "Employees",
                column: "ServiceId1",
                principalTable: "Services",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Services_ServiceId1",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ServiceId1",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_City_Street_Number_Block",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "ServiceId1",
                table: "Employees");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Phones",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "Emails",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "Addresses",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Addresses",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_City_Street_Number_Block",
                table: "Addresses",
                columns: new[] { "City", "Street", "Number", "Block" },
                unique: true,
                filter: "[City] IS NOT NULL AND [Street] IS NOT NULL AND [Number] IS NOT NULL AND [Block] IS NOT NULL");
        }
    }
}
