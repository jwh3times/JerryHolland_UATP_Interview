using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidPay.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "RapidPay");

            migrationBuilder.CreateTable(
                name: "Cards",
                schema: "RapidPay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeeHistories",
                schema: "RapidPay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FeeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "RapidPay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizationLogs",
                schema: "RapidPay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    IsAuthorized = table.Column<bool>(type: "bit", nullable: false),
                    AuthorizationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorizationLogs_Cards_CardId",
                        column: x => x.CardId,
                        principalSchema: "RapidPay",
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardUpdateLogs",
                schema: "RapidPay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    UpdatedField = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardUpdateLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardUpdateLogs_Cards_CardId",
                        column: x => x.CardId,
                        principalSchema: "RapidPay",
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "RapidPay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Cards_CardId",
                        column: x => x.CardId,
                        principalSchema: "RapidPay",
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationLogs_CardId",
                schema: "RapidPay",
                table: "AuthorizationLogs",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardUpdateLogs_CardId",
                schema: "RapidPay",
                table: "CardUpdateLogs",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CardId",
                schema: "RapidPay",
                table: "Transactions",
                column: "CardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorizationLogs",
                schema: "RapidPay");

            migrationBuilder.DropTable(
                name: "CardUpdateLogs",
                schema: "RapidPay");

            migrationBuilder.DropTable(
                name: "FeeHistories",
                schema: "RapidPay");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "RapidPay");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "RapidPay");

            migrationBuilder.DropTable(
                name: "Cards",
                schema: "RapidPay");
        }
    }
}
