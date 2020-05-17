using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelsApi.Migrations
{
    public partial class InitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    EfAccountId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(maxLength: 254, nullable: true),
                    PwHash = table.Column<string>(maxLength: 60, nullable: true),
                    IsManager = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.EfAccountId);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    EfJobId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer = table.Column<string>(maxLength: 64, nullable: true),
                    StartDate = table.Column<DateTimeOffset>(nullable: false),
                    Days = table.Column<int>(nullable: false),
                    Location = table.Column<string>(maxLength: 128, nullable: true),
                    Comments = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.EfJobId);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    EfManagerId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EfAccountId = table.Column<long>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 64, nullable: true),
                    LastName = table.Column<string>(maxLength: 32, nullable: true),
                    Email = table.Column<string>(maxLength: 254, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.EfManagerId);
                    table.ForeignKey(
                        name: "FK_Managers_Accounts_EfAccountId",
                        column: x => x.EfAccountId,
                        principalTable: "Accounts",
                        principalColumn: "EfAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    EfModelId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EfAccountId = table.Column<long>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 64, nullable: true),
                    LastName = table.Column<string>(maxLength: 32, nullable: true),
                    Email = table.Column<string>(maxLength: 254, nullable: true),
                    PhoneNo = table.Column<string>(maxLength: 12, nullable: true),
                    AddresLine1 = table.Column<string>(maxLength: 64, nullable: true),
                    AddresLine2 = table.Column<string>(maxLength: 64, nullable: true),
                    Zip = table.Column<string>(maxLength: 9, nullable: true),
                    City = table.Column<string>(maxLength: 64, nullable: true),
                    Country = table.Column<string>(maxLength: 64, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Nationality = table.Column<string>(maxLength: 64, nullable: true),
                    Height = table.Column<double>(nullable: false),
                    ShoeSize = table.Column<int>(nullable: false),
                    HairColor = table.Column<string>(maxLength: 32, nullable: true),
                    EyeColor = table.Column<string>(maxLength: 32, nullable: true),
                    Comments = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.EfModelId);
                    table.ForeignKey(
                        name: "FK_Models_Accounts_EfAccountId",
                        column: x => x.EfAccountId,
                        principalTable: "Accounts",
                        principalColumn: "EfAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    EfExpenseId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelId = table.Column<long>(nullable: false),
                    JobId = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Text = table.Column<string>(nullable: true),
                    amount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    EfModelId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.EfExpenseId);
                    table.ForeignKey(
                        name: "FK_Expenses_Models_EfModelId",
                        column: x => x.EfModelId,
                        principalTable: "Models",
                        principalColumn: "EfModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobModels",
                columns: table => new
                {
                    EfJobId = table.Column<long>(nullable: false),
                    EfModelId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobModels", x => new { x.EfJobId, x.EfModelId });
                    table.ForeignKey(
                        name: "FK_JobModels_Jobs_EfJobId",
                        column: x => x.EfJobId,
                        principalTable: "Jobs",
                        principalColumn: "EfJobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobModels_Models_EfModelId",
                        column: x => x.EfModelId,
                        principalTable: "Models",
                        principalColumn: "EfModelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_EfModelId",
                table: "Expenses",
                column: "EfModelId");

            migrationBuilder.CreateIndex(
                name: "IX_JobModels_EfModelId",
                table: "JobModels",
                column: "EfModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_EfAccountId",
                table: "Managers",
                column: "EfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_Email",
                table: "Managers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Models_EfAccountId",
                table: "Models",
                column: "EfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_Email",
                table: "Models",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "JobModels");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
