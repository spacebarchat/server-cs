using Fosscord.DbModel.Scaffold;
using Microsoft.EntityFrameworkCore.Migrations;
using Migration = Microsoft.EntityFrameworkCore.Migrations.Migration;

#nullable disable

namespace Fosscord.DbModel.Migrations.Postgres
{
    public partial class JsonColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.AlterColumn<UserData>(
                name: "data",
                table: "users",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<UserChannelSettings>(
                name: "settings",
                table: "members",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<WelcomeScreen>(
                name: "welcome_screen",
                table: "guilds",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<PermissionOverwrite[]>(
                name: "permission_overwrites",
                table: "channels",
                type: "jsonb",
                nullable: false,
                defaultValue: new PermissionOverwrite[0],
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<InstallParams>(
                name: "install_params",
                table: "applications",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
                */
            
            migrationBuilder.Sql("alter table users alter \"data\" type jsonb using data::jsonb");
            migrationBuilder.Sql("alter table members alter \"settings\" type jsonb using settings::jsonb");
            migrationBuilder.Sql("alter table guilds alter \"welcome_screen\" type jsonb using welcome_screen::jsonb");
            migrationBuilder.Sql("alter table channels alter \"permission_overwrites\" type jsonb using permission_overwrites::jsonb");
            migrationBuilder.Sql("alter table applications alter \"install_params\" type jsonb using install_params::jsonb");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "data",
                table: "users",
                type: "text",
                nullable: false,
                oldClrType: typeof(UserData),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "settings",
                table: "members",
                type: "text",
                nullable: false,
                oldClrType: typeof(UserChannelSettings),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "welcome_screen",
                table: "guilds",
                type: "text",
                nullable: false,
                oldClrType: typeof(WelcomeScreen),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "permission_overwrites",
                table: "channels",
                type: "text",
                nullable: true,
                oldClrType: typeof(PermissionOverwrite[]),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "install_params",
                table: "applications",
                type: "text",
                nullable: true,
                oldClrType: typeof(InstallParams),
                oldType: "jsonb",
                oldNullable: true);
        }
    }
}
