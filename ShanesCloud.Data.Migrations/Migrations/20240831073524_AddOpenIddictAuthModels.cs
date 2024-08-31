using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShanesCloud.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddOpenIddictAuthModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ApplicationType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClientSecret = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ClientType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    ConsentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayNames = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    JsonWebKeySet = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Permissions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PostLogoutRedirectUris = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RedirectUris = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: true),
                    Requirements = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Settings = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DisplayNames = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Properties = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Resources = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authorizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Scopes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authorizations_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Payload = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    RedemptionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tokens_Authorizations_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalTable: "Authorizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ClientId",
                table: "Applications",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_ApplicationId_Status_Subject_Type",
                table: "Authorizations",
                columns: new[] { "ApplicationId", "Status", "Subject", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_Scopes_Name",
                table: "Scopes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_ApplicationId_Status_Subject_Type",
                table: "Tokens",
                columns: new[] { "ApplicationId", "Status", "Subject", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_AuthorizationId",
                table: "Tokens",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_ReferenceId",
                table: "Tokens",
                column: "ReferenceId",
                unique: true,
                filter: "[ReferenceId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scopes");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Authorizations");

            migrationBuilder.DropTable(
                name: "Applications");
        }
    }
}
