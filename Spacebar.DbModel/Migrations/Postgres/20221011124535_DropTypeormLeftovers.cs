#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Spacebar.DbModel.Migrations.Postgres;

public partial class DropTypeormLeftovers : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropTable(
            "migrations");

        migrationBuilder.DropTable(
            "query-result-cache");
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.CreateTable(
            "migrations",
            table => new {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>("character varying", nullable: false),
                timestamp = table.Column<long>("bigint", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_migrations", x => x.id); });

        migrationBuilder.CreateTable(
            "query-result-cache",
            table => new {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                duration = table.Column<int>("integer", nullable: false),
                identifier = table.Column<string>("character varying", nullable: true),
                query = table.Column<string>("text", nullable: false),
                result = table.Column<string>("text", nullable: false),
                time = table.Column<long>("bigint", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_query-result-cache", x => x.id); });
    }
}