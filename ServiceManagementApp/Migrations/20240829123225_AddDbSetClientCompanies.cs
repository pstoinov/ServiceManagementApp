using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDbSetClientCompanies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientCompany_Clients_ClientId",
                table: "ClientCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientCompany_Companies_CompanyId",
                table: "ClientCompany");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientCompany",
                table: "ClientCompany");

            migrationBuilder.RenameTable(
                name: "ClientCompany",
                newName: "ClientCompanies");

            migrationBuilder.RenameIndex(
                name: "IX_ClientCompany_CompanyId",
                table: "ClientCompanies",
                newName: "IX_ClientCompanies_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientCompanies",
                table: "ClientCompanies",
                columns: new[] { "ClientId", "CompanyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCompanies_Clients_ClientId",
                table: "ClientCompanies",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCompanies_Companies_CompanyId",
                table: "ClientCompanies",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientCompanies_Clients_ClientId",
                table: "ClientCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientCompanies_Companies_CompanyId",
                table: "ClientCompanies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientCompanies",
                table: "ClientCompanies");

            migrationBuilder.RenameTable(
                name: "ClientCompanies",
                newName: "ClientCompany");

            migrationBuilder.RenameIndex(
                name: "IX_ClientCompanies_CompanyId",
                table: "ClientCompany",
                newName: "IX_ClientCompany_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientCompany",
                table: "ClientCompany",
                columns: new[] { "ClientId", "CompanyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCompany_Clients_ClientId",
                table: "ClientCompany",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCompany_Companies_CompanyId",
                table: "ClientCompany",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
