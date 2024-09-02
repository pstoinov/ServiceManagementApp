using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCollectionsFromEmployeeNotNeeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Services_ServiceId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_ServiceId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Clients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ServiceId",
                table: "Clients",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Services_ServiceId",
                table: "Clients",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }
    }
}
