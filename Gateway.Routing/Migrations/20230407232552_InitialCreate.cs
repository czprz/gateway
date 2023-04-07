using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Routing.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RouteConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UseAuthentication = table.Column<bool>(type: "bit", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Audience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scopes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoadBalancingPolicy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeaderMatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RouteConfigDbId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeaderMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeaderMatches_RouteConfigs_RouteConfigDbId",
                        column: x => x.RouteConfigDbId,
                        principalTable: "RouteConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Hosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RouteConfigDbId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hosts_RouteConfigs_RouteConfigDbId",
                        column: x => x.RouteConfigDbId,
                        principalTable: "RouteConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Methods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RouteConfigDbId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Methods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Methods_RouteConfigs_RouteConfigDbId",
                        column: x => x.RouteConfigDbId,
                        principalTable: "RouteConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QueryParameterMatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RouteConfigDbId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryParameterMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueryParameterMatches_RouteConfigs_RouteConfigDbId",
                        column: x => x.RouteConfigDbId,
                        principalTable: "RouteConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Upstreams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HealthProbeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RouteConfigDbId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upstreams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Upstreams_RouteConfigs_RouteConfigDbId",
                        column: x => x.RouteConfigDbId,
                        principalTable: "RouteConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HeaderMatchValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeaderMatchDbId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeaderMatchValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeaderMatchValues_HeaderMatches_HeaderMatchDbId",
                        column: x => x.HeaderMatchDbId,
                        principalTable: "HeaderMatches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QueryParameterMatchValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QueryParameterMatchDbId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryParameterMatchValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueryParameterMatchValues_QueryParameterMatches_QueryParameterMatchDbId",
                        column: x => x.QueryParameterMatchDbId,
                        principalTable: "QueryParameterMatches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeaderMatches_RouteConfigDbId",
                table: "HeaderMatches",
                column: "RouteConfigDbId");

            migrationBuilder.CreateIndex(
                name: "IX_HeaderMatchValues_HeaderMatchDbId",
                table: "HeaderMatchValues",
                column: "HeaderMatchDbId");

            migrationBuilder.CreateIndex(
                name: "IX_Hosts_RouteConfigDbId",
                table: "Hosts",
                column: "RouteConfigDbId");

            migrationBuilder.CreateIndex(
                name: "IX_Methods_RouteConfigDbId",
                table: "Methods",
                column: "RouteConfigDbId");

            migrationBuilder.CreateIndex(
                name: "IX_QueryParameterMatches_RouteConfigDbId",
                table: "QueryParameterMatches",
                column: "RouteConfigDbId");

            migrationBuilder.CreateIndex(
                name: "IX_QueryParameterMatchValues_QueryParameterMatchDbId",
                table: "QueryParameterMatchValues",
                column: "QueryParameterMatchDbId");

            migrationBuilder.CreateIndex(
                name: "IX_Upstreams_RouteConfigDbId",
                table: "Upstreams",
                column: "RouteConfigDbId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeaderMatchValues");

            migrationBuilder.DropTable(
                name: "Hosts");

            migrationBuilder.DropTable(
                name: "Methods");

            migrationBuilder.DropTable(
                name: "QueryParameterMatchValues");

            migrationBuilder.DropTable(
                name: "Upstreams");

            migrationBuilder.DropTable(
                name: "HeaderMatches");

            migrationBuilder.DropTable(
                name: "QueryParameterMatches");

            migrationBuilder.DropTable(
                name: "RouteConfigs");
        }
    }
}
