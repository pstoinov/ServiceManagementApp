using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedBIMCertDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsCashRegisterService",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BIMCertificateDate",
                table: "CashRegisters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BIMCertificateDate",
                table: "CashRegisters");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCashRegisterService",
                table: "Services",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
