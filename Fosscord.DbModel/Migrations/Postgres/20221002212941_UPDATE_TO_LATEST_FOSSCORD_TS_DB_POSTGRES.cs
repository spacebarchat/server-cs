using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fosscord.DbModel.Migrations.Postgres
{
    public partial class UPDATE_TO_LATEST_FOSSCORD_TS_DB_POSTGRES : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applications_guilds_guild_id",
                table: "applications");

            migrationBuilder.DropForeignKey(
                name: "FK_messages_members_member_id",
                table: "messages");

            migrationBuilder.DropTable(
                name: "client_relase");

            migrationBuilder.DropTable(
                name: "typeorm_metadata");

            migrationBuilder.DropIndex(
                name: "IX_applications_guild_id",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "manual",
                table: "read_states");

            migrationBuilder.DropColumn(
                name: "guild_id",
                table: "applications");

            migrationBuilder.RenameColumn(
                name: "settings",
                table: "users",
                newName: "extended_settings");

            migrationBuilder.RenameColumn(
                name: "verifie",
                table: "connected_accounts",
                newName: "verified");

            migrationBuilder.RenameColumn(
                name: "slug",
                table: "applications",
                newName: "interactions_endpoint_url");

            migrationBuilder.RenameColumn(
                name: "rpc_origins",
                table: "applications",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "primary_sku_id",
                table: "applications",
                newName: "bot_user_id");

            migrationBuilder.AlterColumn<decimal>(
                name: "rights",
                table: "users",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying");

            migrationBuilder.AlterColumn<bool>(
                name: "mfa_enabled",
                table: "users",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "bio",
                table: "users",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying");

            migrationBuilder.AddColumn<DateTime>(
                name: "premium_since",
                table: "users",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "settingsId",
                table: "users",
                type: "character varying",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "totp_last_ticket",
                table: "users",
                type: "character varying",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "totp_secret",
                table: "users",
                type: "character varying",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "icon",
                table: "roles",
                type: "character varying",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unicode_emoji",
                table: "roles",
                type: "character varying",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "notifications_cursor",
                table: "read_states",
                type: "character varying",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "public_ack",
                table: "read_states",
                type: "character varying",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "member_id",
                table: "messages",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "premium_since",
                table: "members",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "avatar",
                table: "members",
                type: "character varying",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "banner",
                table: "members",
                type: "character varying",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bio",
                table: "members",
                type: "character varying",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "communication_disabled_until",
                table: "members",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "joined_by",
                table: "members",
                type: "character varying",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "parent",
                table: "guilds",
                type: "character varying",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "premium_progress_bar_enabled",
                table: "guilds",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "primary_category_id",
                table: "guilds",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "groups",
                table: "emojis",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "default_thread_rate_limit_per_user",
                table: "channels",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "flags",
                table: "channels",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "retention_policy_id",
                table: "channels",
                type: "character varying",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "flags",
                table: "applications",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "applications",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying");

            migrationBuilder.AddColumn<int>(
                name: "discoverability_state",
                table: "applications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "discovery_eligibility_flags",
                table: "applications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "hook",
                table: "applications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "install_params",
                table: "applications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "integration_public",
                table: "applications",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "integration_require_code_grant",
                table: "applications",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "redirect_uris",
                table: "applications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "rpc_application_state",
                table: "applications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "store_application_state",
                table: "applications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tags",
                table: "applications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "verification_state",
                table: "applications",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "backup_codes",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    code = table.Column<string>(type: "character varying", nullable: false),
                    consumed = table.Column<bool>(type: "boolean", nullable: false),
                    expired = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_backup_codes", x => x.id);
                    table.ForeignKey(
                        name: "FK_backup_codes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying", nullable: true),
                    localizations = table.Column<string>(type: "text", nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "client_release",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    pub_date = table.Column<string>(type: "character varying", nullable: false),
                    url = table.Column<string>(type: "character varying", nullable: false),
                    deb_url = table.Column<string>(type: "character varying", nullable: false),
                    osx_url = table.Column<string>(type: "character varying", nullable: false),
                    win_url = table.Column<string>(type: "character varying", nullable: false),
                    notes = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_release", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notes",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    content = table.Column<string>(type: "character varying", nullable: false),
                    owner_id = table.Column<string>(type: "character varying", nullable: true),
                    target_id = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notes", x => x.id);
                    table.ForeignKey(
                        name: "FK_notes_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notes_users_target_id",
                        column: x => x.target_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_settings",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    afk_timeout = table.Column<int>(type: "integer", nullable: true),
                    allow_accessibility_detection = table.Column<bool>(type: "boolean", nullable: true),
                    animate_emoji = table.Column<bool>(type: "boolean", nullable: true),
                    animate_stickers = table.Column<int>(type: "integer", nullable: true),
                    contact_sync_enabled = table.Column<bool>(type: "boolean", nullable: true),
                    convert_emoticons = table.Column<bool>(type: "boolean", nullable: true),
                    custom_status = table.Column<string>(type: "text", nullable: true),
                    default_guilds_restricted = table.Column<bool>(type: "boolean", nullable: true),
                    detect_platform_accounts = table.Column<bool>(type: "boolean", nullable: true),
                    developer_mode = table.Column<bool>(type: "boolean", nullable: true),
                    disable_games_tab = table.Column<bool>(type: "boolean", nullable: true),
                    enable_tts_command = table.Column<bool>(type: "boolean", nullable: true),
                    explicit_content_filter = table.Column<int>(type: "integer", nullable: true),
                    friend_source_flags = table.Column<string>(type: "text", nullable: true),
                    gateway_connected = table.Column<bool>(type: "boolean", nullable: true),
                    gif_auto_play = table.Column<bool>(type: "boolean", nullable: true),
                    guild_folders = table.Column<string>(type: "text", nullable: true),
                    guild_positions = table.Column<string>(type: "text", nullable: true),
                    inline_attachment_media = table.Column<bool>(type: "boolean", nullable: true),
                    inline_embed_media = table.Column<bool>(type: "boolean", nullable: true),
                    locale = table.Column<string>(type: "character varying", nullable: true),
                    message_display_compact = table.Column<bool>(type: "boolean", nullable: true),
                    native_phone_integration_enabled = table.Column<bool>(type: "boolean", nullable: true),
                    render_embeds = table.Column<bool>(type: "boolean", nullable: true),
                    render_reactions = table.Column<bool>(type: "boolean", nullable: true),
                    restricted_guilds = table.Column<string>(type: "text", nullable: true),
                    show_current_game = table.Column<bool>(type: "boolean", nullable: true),
                    status = table.Column<string>(type: "character varying", nullable: true),
                    stream_notifications_enabled = table.Column<bool>(type: "boolean", nullable: true),
                    theme = table.Column<string>(type: "character varying", nullable: true),
                    timezone_offset = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "valid_registration_tokens",
                columns: table => new
                {
                    token = table.Column<string>(type: "character varying", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_valid_registration_tokens", x => x.token);
                });

            migrationBuilder.CreateIndex(
                name: "UQ_76ba283779c8441fd5ff819c8cf",
                table: "users",
                column: "settingsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_2ce5a55796fe4c2f77ece57a647",
                table: "applications",
                column: "bot_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_backup_codes_user_id",
                table: "backup_codes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_notes_target_id",
                table: "notes",
                column: "target_id");

            migrationBuilder.CreateIndex(
                name: "UQ_74e6689b9568cc965b8bfc9150b",
                table: "notes",
                columns: new[] { "owner_id", "target_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_applications_users_bot_user_id",
                table: "applications",
                column: "bot_user_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_users_member_id",
                table: "messages",
                column: "member_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_user_settings_settingsId",
                table: "users",
                column: "settingsId",
                principalTable: "user_settings",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applications_users_bot_user_id",
                table: "applications");

            migrationBuilder.DropForeignKey(
                name: "FK_messages_users_member_id",
                table: "messages");

            migrationBuilder.DropForeignKey(
                name: "FK_users_user_settings_settingsId",
                table: "users");

            migrationBuilder.DropTable(
                name: "backup_codes");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "client_release");

            migrationBuilder.DropTable(
                name: "notes");

            migrationBuilder.DropTable(
                name: "user_settings");

            migrationBuilder.DropTable(
                name: "valid_registration_tokens");

            migrationBuilder.DropIndex(
                name: "UQ_76ba283779c8441fd5ff819c8cf",
                table: "users");

            migrationBuilder.DropIndex(
                name: "UQ_2ce5a55796fe4c2f77ece57a647",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "premium_since",
                table: "users");

            migrationBuilder.DropColumn(
                name: "settingsId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "totp_last_ticket",
                table: "users");

            migrationBuilder.DropColumn(
                name: "totp_secret",
                table: "users");

            migrationBuilder.DropColumn(
                name: "icon",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "unicode_emoji",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "notifications_cursor",
                table: "read_states");

            migrationBuilder.DropColumn(
                name: "public_ack",
                table: "read_states");

            migrationBuilder.DropColumn(
                name: "avatar",
                table: "members");

            migrationBuilder.DropColumn(
                name: "banner",
                table: "members");

            migrationBuilder.DropColumn(
                name: "bio",
                table: "members");

            migrationBuilder.DropColumn(
                name: "communication_disabled_until",
                table: "members");

            migrationBuilder.DropColumn(
                name: "joined_by",
                table: "members");

            migrationBuilder.DropColumn(
                name: "parent",
                table: "guilds");

            migrationBuilder.DropColumn(
                name: "premium_progress_bar_enabled",
                table: "guilds");

            migrationBuilder.DropColumn(
                name: "primary_category_id",
                table: "guilds");

            migrationBuilder.DropColumn(
                name: "groups",
                table: "emojis");

            migrationBuilder.DropColumn(
                name: "default_thread_rate_limit_per_user",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "flags",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "retention_policy_id",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "discoverability_state",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "discovery_eligibility_flags",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "hook",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "install_params",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "integration_public",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "integration_require_code_grant",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "redirect_uris",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "rpc_application_state",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "store_application_state",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "tags",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "verification_state",
                table: "applications");

            migrationBuilder.RenameColumn(
                name: "extended_settings",
                table: "users",
                newName: "settings");

            migrationBuilder.RenameColumn(
                name: "verified",
                table: "connected_accounts",
                newName: "verifie");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "applications",
                newName: "rpc_origins");

            migrationBuilder.RenameColumn(
                name: "interactions_endpoint_url",
                table: "applications",
                newName: "slug");

            migrationBuilder.RenameColumn(
                name: "bot_user_id",
                table: "applications",
                newName: "primary_sku_id");

            migrationBuilder.AlterColumn<string>(
                name: "rights",
                table: "users",
                type: "character varying",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<bool>(
                name: "mfa_enabled",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "bio",
                table: "users",
                type: "character varying",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "manual",
                table: "read_states",
                type: "boolean",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "member_id",
                table: "messages",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "premium_since",
                table: "members",
                type: "integer",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "flags",
                table: "applications",
                type: "character varying",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "applications",
                type: "character varying",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "guild_id",
                table: "applications",
                type: "character varying",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "client_relase",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    deb_url = table.Column<string>(type: "character varying", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    notes = table.Column<string>(type: "character varying", nullable: true),
                    osx_url = table.Column<string>(type: "character varying", nullable: false),
                    pub_date = table.Column<string>(type: "character varying", nullable: false),
                    url = table.Column<string>(type: "character varying", nullable: false),
                    win_url = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_relase", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "typeorm_metadata",
                columns: table => new
                {
                    database = table.Column<string>(type: "character varying", nullable: true),
                    name = table.Column<string>(type: "character varying", nullable: true),
                    schema = table.Column<string>(type: "character varying", nullable: true),
                    table = table.Column<string>(type: "character varying", nullable: true),
                    type = table.Column<string>(type: "character varying", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_applications_guild_id",
                table: "applications",
                column: "guild_id");

            migrationBuilder.AddForeignKey(
                name: "FK_applications_guilds_guild_id",
                table: "applications",
                column: "guild_id",
                principalTable: "guilds",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_members_member_id",
                table: "messages",
                column: "member_id",
                principalTable: "members",
                principalColumn: "index");
        }
    }
}
