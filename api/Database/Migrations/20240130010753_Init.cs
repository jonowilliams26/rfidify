using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RFIDify.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpotifyAccessToken",
                columns: table => new
                {
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotifyAccessToken", x => x.Token);
                });

            migrationBuilder.CreateTable(
                name: "SpotifyAuthorizationState",
                columns: table => new
                {
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    RedirectUri = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotifyAuthorizationState", x => x.State);
                });

            migrationBuilder.CreateTable(
                name: "SpotifyCredentials",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "TEXT", nullable: false),
                    ClientSecret = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotifyCredentials", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "SpotifyRefreshToken",
                columns: table => new
                {
                    Token = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotifyRefreshToken", x => x.Token);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpotifyAccessToken");

            migrationBuilder.DropTable(
                name: "SpotifyAuthorizationState");

            migrationBuilder.DropTable(
                name: "SpotifyCredentials");

            migrationBuilder.DropTable(
                name: "SpotifyRefreshToken");
        }
    }
}
