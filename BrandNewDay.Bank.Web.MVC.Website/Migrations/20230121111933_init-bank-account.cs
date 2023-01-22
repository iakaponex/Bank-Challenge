using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrandNewDay.Bank.Web.MVC.Website.Migrations
{
    public partial class initbankaccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Bank");

            migrationBuilder.CreateTable(
                name: "BankAccount",
                schema: "Bank",
                columns: table => new
                {
                    IbanNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NetUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CloseAccountDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccount", x => x.IbanNumber);
                    table.ForeignKey(
                        name: "FK_BankAccount_AspNetUsers_NetUserId",
                        column: x => x.NetUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_NetUserId",
                schema: "Bank",
                table: "BankAccount",
                column: "NetUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccount",
                schema: "Bank");
        }
    }
}
