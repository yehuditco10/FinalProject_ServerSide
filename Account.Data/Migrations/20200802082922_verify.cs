using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Account.Data.Migrations
{
    public partial class verify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Operation");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Operation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EmailVerification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    VerificationCode = table.Column<int>(nullable: false),
                    ExpirationTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerification", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerification_Email",
                table: "EmailVerification",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailVerification");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Operation");

            migrationBuilder.AddColumn<int>(
                name: "TransactionAmount",
                table: "Operation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
