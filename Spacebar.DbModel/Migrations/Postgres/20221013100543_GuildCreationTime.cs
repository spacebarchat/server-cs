using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spacebar.DbModel.Migrations.Postgres;

public partial class GuildCreationTime : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            "created_at",
            "guilds",
            "timestamp without time zone",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "created_at",
            "guilds");
    }
}