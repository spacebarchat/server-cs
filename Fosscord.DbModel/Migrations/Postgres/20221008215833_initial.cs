using System.Collections;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fosscord.DbModel.Migrations.Postgres
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "migrations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    timestamp = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_migrations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "query-result-cache",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    identifier = table.Column<string>(type: "character varying", nullable: true),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    query = table.Column<string>(type: "text", nullable: false),
                    result = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_query-result-cache", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rate_limits",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    executor_id = table.Column<string>(type: "character varying", nullable: false),
                    hits = table.Column<int>(type: "integer", nullable: false),
                    blocked = table.Column<bool>(type: "boolean", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rate_limits", x => x.id);
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

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    username = table.Column<string>(type: "character varying", nullable: false),
                    discriminator = table.Column<string>(type: "character varying", nullable: false),
                    avatar = table.Column<string>(type: "character varying", nullable: true),
                    accent_color = table.Column<int>(type: "integer", nullable: true),
                    banner = table.Column<string>(type: "character varying", nullable: true),
                    phone = table.Column<string>(type: "character varying", nullable: true),
                    desktop = table.Column<bool>(type: "boolean", nullable: false),
                    mobile = table.Column<bool>(type: "boolean", nullable: false),
                    premium = table.Column<bool>(type: "boolean", nullable: false),
                    premium_type = table.Column<int>(type: "integer", nullable: false),
                    bot = table.Column<bool>(type: "boolean", nullable: false),
                    bio = table.Column<string>(type: "character varying", nullable: true),
                    system = table.Column<bool>(type: "boolean", nullable: false),
                    nsfw_allowed = table.Column<bool>(type: "boolean", nullable: false),
                    mfa_enabled = table.Column<bool>(type: "boolean", nullable: true),
                    totp_secret = table.Column<string>(type: "character varying", nullable: true),
                    totp_last_ticket = table.Column<string>(type: "character varying", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    premium_since = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    verified = table.Column<bool>(type: "boolean", nullable: false),
                    disabled = table.Column<bool>(type: "boolean", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false),
                    email = table.Column<string>(type: "character varying", nullable: true),
                    flags = table.Column<string>(type: "character varying", nullable: false),
                    public_flags = table.Column<int>(type: "integer", nullable: false),
                    rights = table.Column<BitArray>(type: "bit varying", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false),
                    fingerprints = table.Column<string>(type: "text", nullable: false),
                    extended_settings = table.Column<string>(type: "text", nullable: false),
                    settingsId = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_user_settings_settingsId",
                        column: x => x.settingsId,
                        principalTable: "user_settings",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    user_id = table.Column<string>(type: "character varying", nullable: true),
                    action_type = table.Column<int>(type: "integer", nullable: false),
                    options = table.Column<string>(type: "text", nullable: true),
                    changes = table.Column<string>(type: "text", nullable: false),
                    reason = table.Column<string>(type: "character varying", nullable: true),
                    target_id = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_audit_logs_users_target_id",
                        column: x => x.target_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_audit_logs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

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
                name: "connected_accounts",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    user_id = table.Column<string>(type: "character varying", nullable: true),
                    access_token = table.Column<string>(type: "character varying", nullable: false),
                    friend_sync = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    revoked = table.Column<bool>(type: "boolean", nullable: false),
                    show_activity = table.Column<bool>(type: "boolean", nullable: false),
                    type = table.Column<string>(type: "character varying", nullable: false),
                    verified = table.Column<bool>(type: "boolean", nullable: false),
                    visibility = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_connected_accounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_connected_accounts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
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
                name: "relationships",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    from_id = table.Column<string>(type: "character varying", nullable: false),
                    to_id = table.Column<string>(type: "character varying", nullable: false),
                    nickname = table.Column<string>(type: "character varying", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relationships", x => x.id);
                    table.ForeignKey(
                        name: "FK_relationships_users_from_id",
                        column: x => x.from_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_relationships_users_to_id",
                        column: x => x.to_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    user_id = table.Column<string>(type: "character varying", nullable: true),
                    session_id = table.Column<string>(type: "character varying", nullable: false),
                    activities = table.Column<string>(type: "text", nullable: true),
                    client_info = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_sessions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    icon = table.Column<string>(type: "character varying", nullable: true),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    owner_user_id = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.id);
                    table.ForeignKey(
                        name: "FK_teams_users_owner_user_id",
                        column: x => x.owner_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    icon = table.Column<string>(type: "character varying", nullable: true),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    bot_public = table.Column<bool>(type: "boolean", nullable: false),
                    bot_require_code_grant = table.Column<bool>(type: "boolean", nullable: false),
                    terms_of_service_url = table.Column<string>(type: "character varying", nullable: true),
                    privacy_policy_url = table.Column<string>(type: "character varying", nullable: true),
                    summary = table.Column<string>(type: "character varying", nullable: true),
                    verify_key = table.Column<string>(type: "character varying", nullable: false),
                    cover_image = table.Column<string>(type: "character varying", nullable: true),
                    owner_id = table.Column<string>(type: "character varying", nullable: true),
                    team_id = table.Column<string>(type: "character varying", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    hook = table.Column<bool>(type: "boolean", nullable: false),
                    redirect_uris = table.Column<string>(type: "text", nullable: true),
                    rpc_application_state = table.Column<int>(type: "integer", nullable: true),
                    store_application_state = table.Column<int>(type: "integer", nullable: true),
                    verification_state = table.Column<int>(type: "integer", nullable: true),
                    interactions_endpoint_url = table.Column<string>(type: "character varying", nullable: true),
                    integration_public = table.Column<bool>(type: "boolean", nullable: true),
                    integration_require_code_grant = table.Column<bool>(type: "boolean", nullable: true),
                    discoverability_state = table.Column<int>(type: "integer", nullable: true),
                    discovery_eligibility_flags = table.Column<int>(type: "integer", nullable: true),
                    tags = table.Column<string>(type: "text", nullable: true),
                    install_params = table.Column<string>(type: "text", nullable: true),
                    bot_user_id = table.Column<string>(type: "character varying", nullable: true),
                    flags = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applications", x => x.id);
                    table.ForeignKey(
                        name: "FK_applications_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_applications_users_bot_user_id",
                        column: x => x.bot_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_applications_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "team_members",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    membership_state = table.Column<int>(type: "integer", nullable: false),
                    permissions = table.Column<string>(type: "text", nullable: false),
                    team_id = table.Column<string>(type: "character varying", nullable: true),
                    user_id = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team_members", x => x.id);
                    table.ForeignKey(
                        name: "FK_team_members_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_team_members_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "attachments",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    filename = table.Column<string>(type: "character varying", nullable: false),
                    size = table.Column<int>(type: "integer", nullable: false),
                    url = table.Column<string>(type: "character varying", nullable: false),
                    proxy_url = table.Column<string>(type: "character varying", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: true),
                    width = table.Column<int>(type: "integer", nullable: true),
                    content_type = table.Column<string>(type: "character varying", nullable: true),
                    message_id = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attachments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bans",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    user_id = table.Column<string>(type: "character varying", nullable: true),
                    guild_id = table.Column<string>(type: "character varying", nullable: true),
                    executor_id = table.Column<string>(type: "character varying", nullable: true),
                    ip = table.Column<string>(type: "character varying", nullable: false),
                    reason = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bans", x => x.id);
                    table.ForeignKey(
                        name: "FK_bans_users_executor_id",
                        column: x => x.executor_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_bans_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ChannelMessage",
                columns: table => new
                {
                    ChannelsId = table.Column<string>(type: "character varying", nullable: false),
                    MessagesId = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelMessage", x => new { x.ChannelsId, x.MessagesId });
                });

            migrationBuilder.CreateTable(
                name: "channels",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: true),
                    icon = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    last_message_id = table.Column<string>(type: "character varying", nullable: true),
                    guild_id = table.Column<string>(type: "character varying", nullable: true),
                    parent_id = table.Column<string>(type: "character varying", nullable: true),
                    owner_id = table.Column<string>(type: "character varying", nullable: true),
                    last_pin_timestamp = table.Column<int>(type: "integer", nullable: true),
                    default_auto_archive_duration = table.Column<int>(type: "integer", nullable: true),
                    position = table.Column<int>(type: "integer", nullable: true),
                    permission_overwrites = table.Column<string>(type: "text", nullable: true),
                    video_quality_mode = table.Column<int>(type: "integer", nullable: true),
                    bitrate = table.Column<int>(type: "integer", nullable: true),
                    user_limit = table.Column<int>(type: "integer", nullable: true),
                    nsfw = table.Column<bool>(type: "boolean", nullable: true),
                    rate_limit_per_user = table.Column<int>(type: "integer", nullable: true),
                    topic = table.Column<string>(type: "character varying", nullable: true),
                    retention_policy_id = table.Column<string>(type: "character varying", nullable: true),
                    flags = table.Column<int>(type: "integer", nullable: true),
                    default_thread_rate_limit_per_user = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_channels", x => x.id);
                    table.ForeignKey(
                        name: "FK_channels_channels_parent_id",
                        column: x => x.parent_id,
                        principalTable: "channels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_channels_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "read_states",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    channel_id = table.Column<string>(type: "character varying", nullable: false),
                    user_id = table.Column<string>(type: "character varying", nullable: false),
                    last_message_id = table.Column<string>(type: "character varying", nullable: true),
                    public_ack = table.Column<string>(type: "character varying", nullable: true),
                    notifications_cursor = table.Column<string>(type: "character varying", nullable: true),
                    last_pin_timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    mention_count = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_read_states", x => x.id);
                    table.ForeignKey(
                        name: "FK_read_states_channels_channel_id",
                        column: x => x.channel_id,
                        principalTable: "channels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_read_states_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipients",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    channel_id = table.Column<string>(type: "character varying", nullable: false),
                    user_id = table.Column<string>(type: "character varying", nullable: false),
                    closed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipients", x => x.id);
                    table.ForeignKey(
                        name: "FK_recipients_channels_channel_id",
                        column: x => x.channel_id,
                        principalTable: "channels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recipients_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "emojis",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    animated = table.Column<bool>(type: "boolean", nullable: false),
                    available = table.Column<bool>(type: "boolean", nullable: false),
                    guild_id = table.Column<string>(type: "character varying", nullable: false),
                    user_id = table.Column<string>(type: "character varying", nullable: true),
                    managed = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    require_colons = table.Column<bool>(type: "boolean", nullable: false),
                    roles = table.Column<string>(type: "text", nullable: false),
                    groups = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emojis", x => x.id);
                    table.ForeignKey(
                        name: "FK_emojis_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "guilds",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    afk_channel_id = table.Column<string>(type: "character varying", nullable: true),
                    afk_timeout = table.Column<int>(type: "integer", nullable: true),
                    banner = table.Column<string>(type: "character varying", nullable: true),
                    default_message_notifications = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    discovery_splash = table.Column<string>(type: "character varying", nullable: true),
                    explicit_content_filter = table.Column<int>(type: "integer", nullable: true),
                    features = table.Column<string>(type: "text", nullable: false),
                    primary_category_id = table.Column<int>(type: "integer", nullable: true),
                    icon = table.Column<string>(type: "character varying", nullable: true),
                    large = table.Column<bool>(type: "boolean", nullable: true),
                    max_members = table.Column<int>(type: "integer", nullable: true),
                    max_presences = table.Column<int>(type: "integer", nullable: true),
                    max_video_channel_users = table.Column<int>(type: "integer", nullable: true),
                    member_count = table.Column<int>(type: "integer", nullable: true),
                    presence_count = table.Column<int>(type: "integer", nullable: true),
                    template_id = table.Column<string>(type: "character varying", nullable: true),
                    mfa_level = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    owner_id = table.Column<string>(type: "character varying", nullable: true),
                    preferred_locale = table.Column<string>(type: "character varying", nullable: true),
                    premium_subscription_count = table.Column<int>(type: "integer", nullable: true),
                    premium_tier = table.Column<int>(type: "integer", nullable: true),
                    public_updates_channel_id = table.Column<string>(type: "character varying", nullable: true),
                    rules_channel_id = table.Column<string>(type: "character varying", nullable: true),
                    region = table.Column<string>(type: "character varying", nullable: true),
                    splash = table.Column<string>(type: "character varying", nullable: true),
                    system_channel_id = table.Column<string>(type: "character varying", nullable: true),
                    system_channel_flags = table.Column<int>(type: "integer", nullable: true),
                    unavailable = table.Column<bool>(type: "boolean", nullable: true),
                    verification_level = table.Column<int>(type: "integer", nullable: true),
                    welcome_screen = table.Column<string>(type: "text", nullable: false),
                    widget_channel_id = table.Column<string>(type: "character varying", nullable: true),
                    widget_enabled = table.Column<bool>(type: "boolean", nullable: true),
                    nsfw_level = table.Column<int>(type: "integer", nullable: true),
                    nsfw = table.Column<bool>(type: "boolean", nullable: true),
                    parent = table.Column<string>(type: "character varying", nullable: true),
                    premium_progress_bar_enabled = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guilds", x => x.id);
                    table.ForeignKey(
                        name: "FK_guilds_channels_afk_channel_id",
                        column: x => x.afk_channel_id,
                        principalTable: "channels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_guilds_channels_public_updates_channel_id",
                        column: x => x.public_updates_channel_id,
                        principalTable: "channels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_guilds_channels_rules_channel_id",
                        column: x => x.rules_channel_id,
                        principalTable: "channels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_guilds_channels_system_channel_id",
                        column: x => x.system_channel_id,
                        principalTable: "channels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_guilds_channels_widget_channel_id",
                        column: x => x.widget_channel_id,
                        principalTable: "channels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_guilds_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "invites",
                columns: table => new
                {
                    code = table.Column<string>(type: "character varying", nullable: false),
                    temporary = table.Column<bool>(type: "boolean", nullable: false),
                    uses = table.Column<int>(type: "integer", nullable: false),
                    max_uses = table.Column<int>(type: "integer", nullable: false),
                    max_age = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    guild_id = table.Column<string>(type: "character varying", nullable: true),
                    channel_id = table.Column<string>(type: "character varying", nullable: true),
                    inviter_id = table.Column<string>(type: "character varying", nullable: true),
                    target_user_id = table.Column<string>(type: "character varying", nullable: true),
                    target_user_type = table.Column<int>(type: "integer", nullable: true),
                    vanity_url = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invites", x => x.code);
                    table.ForeignKey(
                        name: "FK_invites_channels_channel_id",
                        column: x => x.channel_id,
                        principalTable: "channels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_invites_guilds_guild_id",
                        column: x => x.guild_id,
                        principalTable: "guilds",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_invites_users_inviter_id",
                        column: x => x.inviter_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_invites_users_target_user_id",
                        column: x => x.target_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "members",
                columns: table => new
                {
                    index = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id = table.Column<string>(type: "character varying", nullable: false),
                    guild_id = table.Column<string>(type: "character varying", nullable: false),
                    nick = table.Column<string>(type: "character varying", nullable: true),
                    joined_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    deaf = table.Column<bool>(type: "boolean", nullable: false),
                    mute = table.Column<bool>(type: "boolean", nullable: false),
                    pending = table.Column<bool>(type: "boolean", nullable: false),
                    settings = table.Column<string>(type: "text", nullable: false),
                    last_message_id = table.Column<string>(type: "character varying", nullable: true),
                    joined_by = table.Column<string>(type: "character varying", nullable: true),
                    premium_since = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    avatar = table.Column<string>(type: "character varying", nullable: true),
                    banner = table.Column<string>(type: "character varying", nullable: true),
                    bio = table.Column<string>(type: "character varying", nullable: false),
                    communication_disabled_until = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_members", x => x.index);
                    table.ForeignKey(
                        name: "FK_members_guilds_guild_id",
                        column: x => x.guild_id,
                        principalTable: "guilds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_members_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    guild_id = table.Column<string>(type: "character varying", nullable: true),
                    color = table.Column<int>(type: "integer", nullable: false),
                    hoist = table.Column<bool>(type: "boolean", nullable: false),
                    managed = table.Column<bool>(type: "boolean", nullable: false),
                    mentionable = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    permissions = table.Column<string>(type: "character varying", nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false),
                    icon = table.Column<string>(type: "character varying", nullable: true),
                    unicode_emoji = table.Column<string>(type: "character varying", nullable: true),
                    tags = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                    table.ForeignKey(
                        name: "FK_roles_guilds_guild_id",
                        column: x => x.guild_id,
                        principalTable: "guilds",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "templates",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    code = table.Column<string>(type: "character varying", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    usage_count = table.Column<int>(type: "integer", nullable: true),
                    creator_id = table.Column<string>(type: "character varying", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    source_guild_id = table.Column<string>(type: "character varying", nullable: true),
                    serialized_source_guild = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_templates", x => x.id);
                    table.ForeignKey(
                        name: "FK_templates_guilds_source_guild_id",
                        column: x => x.source_guild_id,
                        principalTable: "guilds",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_templates_users_creator_id",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "voice_states",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    guild_id = table.Column<string>(type: "character varying", nullable: true),
                    channel_id = table.Column<string>(type: "character varying", nullable: true),
                    user_id = table.Column<string>(type: "character varying", nullable: true),
                    session_id = table.Column<string>(type: "character varying", nullable: false),
                    token = table.Column<string>(type: "character varying", nullable: true),
                    deaf = table.Column<bool>(type: "boolean", nullable: false),
                    mute = table.Column<bool>(type: "boolean", nullable: false),
                    self_deaf = table.Column<bool>(type: "boolean", nullable: false),
                    self_mute = table.Column<bool>(type: "boolean", nullable: false),
                    self_stream = table.Column<bool>(type: "boolean", nullable: true),
                    self_video = table.Column<bool>(type: "boolean", nullable: false),
                    suppress = table.Column<bool>(type: "boolean", nullable: false),
                    request_to_speak_timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_voice_states", x => x.id);
                    table.ForeignKey(
                        name: "FK_voice_states_channels_channel_id",
                        column: x => x.channel_id,
                        principalTable: "channels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_voice_states_guilds_guild_id",
                        column: x => x.guild_id,
                        principalTable: "guilds",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_voice_states_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "webhooks",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: true),
                    avatar = table.Column<string>(type: "character varying", nullable: true),
                    token = table.Column<string>(type: "character varying", nullable: true),
                    guild_id = table.Column<string>(type: "character varying", nullable: true),
                    channel_id = table.Column<string>(type: "character varying", nullable: true),
                    application_id = table.Column<string>(type: "character varying", nullable: true),
                    user_id = table.Column<string>(type: "character varying", nullable: true),
                    source_guild_id = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_webhooks", x => x.id);
                    table.ForeignKey(
                        name: "FK_webhooks_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "applications",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_webhooks_channels_channel_id",
                        column: x => x.channel_id,
                        principalTable: "channels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_webhooks_guilds_guild_id",
                        column: x => x.guild_id,
                        principalTable: "guilds",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_webhooks_guilds_source_guild_id",
                        column: x => x.source_guild_id,
                        principalTable: "guilds",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_webhooks_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "MemberRole",
                columns: table => new
                {
                    Index = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRole", x => new { x.Index, x.RoleId });
                    table.ForeignKey(
                        name: "FK_MemberRole_members_Index",
                        column: x => x.Index,
                        principalTable: "members",
                        principalColumn: "index",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberRole_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    channel_id = table.Column<string>(type: "character varying", nullable: true),
                    guild_id = table.Column<string>(type: "character varying", nullable: true),
                    author_id = table.Column<string>(type: "character varying", nullable: true),
                    member_id = table.Column<string>(type: "character varying", nullable: true),
                    webhook_id = table.Column<string>(type: "character varying", nullable: true),
                    application_id = table.Column<string>(type: "character varying", nullable: true),
                    content = table.Column<string>(type: "character varying", nullable: true),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    edited_timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    tts = table.Column<bool>(type: "boolean", nullable: true),
                    mention_everyone = table.Column<bool>(type: "boolean", nullable: true),
                    embeds = table.Column<string>(type: "text", nullable: false),
                    reactions = table.Column<string>(type: "text", nullable: false),
                    nonce = table.Column<string>(type: "text", nullable: true),
                    pinned = table.Column<bool>(type: "boolean", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    activity = table.Column<string>(type: "text", nullable: true),
                    flags = table.Column<string>(type: "character varying", nullable: true),
                    message_reference = table.Column<string>(type: "text", nullable: true),
                    interaction = table.Column<string>(type: "text", nullable: true),
                    components = table.Column<string>(type: "text", nullable: true),
                    message_reference_id = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_messages_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "applications",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_messages_channels_channel_id",
                        column: x => x.channel_id,
                        principalTable: "channels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_messages_guilds_guild_id",
                        column: x => x.guild_id,
                        principalTable: "guilds",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_messages_messages_message_reference_id",
                        column: x => x.message_reference_id,
                        principalTable: "messages",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_messages_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_messages_users_member_id",
                        column: x => x.member_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_messages_webhooks_webhook_id",
                        column: x => x.webhook_id,
                        principalTable: "webhooks",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "MessageRole",
                columns: table => new
                {
                    MessagesId = table.Column<string>(type: "character varying", nullable: false),
                    RolesId = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageRole", x => new { x.MessagesId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_MessageRole_messages_MessagesId",
                        column: x => x.MessagesId,
                        principalTable: "messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageRole_roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageUser",
                columns: table => new
                {
                    MessagesId = table.Column<string>(type: "character varying", nullable: false),
                    UsersId = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageUser", x => new { x.MessagesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_MessageUser_messages_MessagesId",
                        column: x => x.MessagesId,
                        principalTable: "messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageUser_users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageSticker",
                columns: table => new
                {
                    MessagesId = table.Column<string>(type: "character varying", nullable: false),
                    StickersId = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageSticker", x => new { x.MessagesId, x.StickersId });
                    table.ForeignKey(
                        name: "FK_MessageSticker_messages_MessagesId",
                        column: x => x.MessagesId,
                        principalTable: "messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sticker_packs",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    banner_asset_id = table.Column<string>(type: "character varying", nullable: true),
                    cover_sticker_id = table.Column<string>(type: "character varying", nullable: true),
                    coverStickerId = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sticker_packs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "stickers",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    available = table.Column<bool>(type: "boolean", nullable: true),
                    tags = table.Column<string>(type: "character varying", nullable: true),
                    pack_id = table.Column<string>(type: "character varying", nullable: true),
                    guild_id = table.Column<string>(type: "character varying", nullable: true),
                    user_id = table.Column<string>(type: "character varying", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    format_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stickers", x => x.id);
                    table.ForeignKey(
                        name: "FK_stickers_guilds_guild_id",
                        column: x => x.guild_id,
                        principalTable: "guilds",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stickers_sticker_packs_pack_id",
                        column: x => x.pack_id,
                        principalTable: "sticker_packs",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stickers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_applications_owner_id",
                table: "applications",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_applications_team_id",
                table: "applications",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "UQ_2ce5a55796fe4c2f77ece57a647",
                table: "applications",
                column: "bot_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attachments_message_id",
                table: "attachments",
                column: "message_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_target_id",
                table: "audit_logs",
                column: "target_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_user_id",
                table: "audit_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_backup_codes_user_id",
                table: "backup_codes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_bans_executor_id",
                table: "bans",
                column: "executor_id");

            migrationBuilder.CreateIndex(
                name: "IX_bans_guild_id",
                table: "bans",
                column: "guild_id");

            migrationBuilder.CreateIndex(
                name: "IX_bans_user_id",
                table: "bans",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMessage_MessagesId",
                table: "ChannelMessage",
                column: "MessagesId");

            migrationBuilder.CreateIndex(
                name: "IX_channels_guild_id",
                table: "channels",
                column: "guild_id");

            migrationBuilder.CreateIndex(
                name: "IX_channels_owner_id",
                table: "channels",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_channels_parent_id",
                table: "channels",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_connected_accounts_user_id",
                table: "connected_accounts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_emojis_guild_id",
                table: "emojis",
                column: "guild_id");

            migrationBuilder.CreateIndex(
                name: "IX_emojis_user_id",
                table: "emojis",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_guilds_afk_channel_id",
                table: "guilds",
                column: "afk_channel_id");

            migrationBuilder.CreateIndex(
                name: "IX_guilds_owner_id",
                table: "guilds",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_guilds_public_updates_channel_id",
                table: "guilds",
                column: "public_updates_channel_id");

            migrationBuilder.CreateIndex(
                name: "IX_guilds_rules_channel_id",
                table: "guilds",
                column: "rules_channel_id");

            migrationBuilder.CreateIndex(
                name: "IX_guilds_system_channel_id",
                table: "guilds",
                column: "system_channel_id");

            migrationBuilder.CreateIndex(
                name: "IX_guilds_template_id",
                table: "guilds",
                column: "template_id");

            migrationBuilder.CreateIndex(
                name: "IX_guilds_widget_channel_id",
                table: "guilds",
                column: "widget_channel_id");

            migrationBuilder.CreateIndex(
                name: "IX_invites_channel_id",
                table: "invites",
                column: "channel_id");

            migrationBuilder.CreateIndex(
                name: "IX_invites_guild_id",
                table: "invites",
                column: "guild_id");

            migrationBuilder.CreateIndex(
                name: "IX_invites_inviter_id",
                table: "invites",
                column: "inviter_id");

            migrationBuilder.CreateIndex(
                name: "IX_invites_target_user_id",
                table: "invites",
                column: "target_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRole_RoleId",
                table: "MemberRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IDX_bb2bf9386ac443afbbbf9f12d3",
                table: "members",
                columns: new[] { "id", "guild_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_members_guild_id",
                table: "members",
                column: "guild_id");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRole_RolesId",
                table: "MessageRole",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IDX_05535bc695e9f7ee104616459d",
                table: "messages",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IDX_3ed7a60fb7dbe04e1ba9332a8b",
                table: "messages",
                columns: new[] { "channel_id", "id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_86b9109b155eb70c0a2ca3b4b6",
                table: "messages",
                column: "channel_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_application_id",
                table: "messages",
                column: "application_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_guild_id",
                table: "messages",
                column: "guild_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_member_id",
                table: "messages",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_message_reference_id",
                table: "messages",
                column: "message_reference_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_webhook_id",
                table: "messages",
                column: "webhook_id");

            migrationBuilder.CreateIndex(
                name: "IX_MessageSticker_StickersId",
                table: "MessageSticker",
                column: "StickersId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageUser_UsersId",
                table: "MessageUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_notes_target_id",
                table: "notes",
                column: "target_id");

            migrationBuilder.CreateIndex(
                name: "UQ_74e6689b9568cc965b8bfc9150b",
                table: "notes",
                columns: new[] { "owner_id", "target_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_0abf8b443321bd3cf7f81ee17a",
                table: "read_states",
                columns: new[] { "channel_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_read_states_user_id",
                table: "read_states",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_recipients_channel_id",
                table: "recipients",
                column: "channel_id");

            migrationBuilder.CreateIndex(
                name: "IX_recipients_user_id",
                table: "recipients",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IDX_a0b2ff0a598df0b0d055934a17",
                table: "relationships",
                columns: new[] { "from_id", "to_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_relationships_to_id",
                table: "relationships",
                column: "to_id");

            migrationBuilder.CreateIndex(
                name: "IX_roles_guild_id",
                table: "roles",
                column: "guild_id");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_user_id",
                table: "sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_sticker_packs_coverStickerId",
                table: "sticker_packs",
                column: "coverStickerId");

            migrationBuilder.CreateIndex(
                name: "IX_stickers_guild_id",
                table: "stickers",
                column: "guild_id");

            migrationBuilder.CreateIndex(
                name: "IX_stickers_pack_id",
                table: "stickers",
                column: "pack_id");

            migrationBuilder.CreateIndex(
                name: "IX_stickers_user_id",
                table: "stickers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_members_team_id",
                table: "team_members",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_members_user_id",
                table: "team_members",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_teams_owner_user_id",
                table: "teams",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_templates_creator_id",
                table: "templates",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_templates_source_guild_id",
                table: "templates",
                column: "source_guild_id");

            migrationBuilder.CreateIndex(
                name: "UQ_be38737bf339baf63b1daeffb55",
                table: "templates",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_76ba283779c8441fd5ff819c8cf",
                table: "users",
                column: "settingsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_voice_states_channel_id",
                table: "voice_states",
                column: "channel_id");

            migrationBuilder.CreateIndex(
                name: "IX_voice_states_guild_id",
                table: "voice_states",
                column: "guild_id");

            migrationBuilder.CreateIndex(
                name: "IX_voice_states_user_id",
                table: "voice_states",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_webhooks_application_id",
                table: "webhooks",
                column: "application_id");

            migrationBuilder.CreateIndex(
                name: "IX_webhooks_channel_id",
                table: "webhooks",
                column: "channel_id");

            migrationBuilder.CreateIndex(
                name: "IX_webhooks_guild_id",
                table: "webhooks",
                column: "guild_id");

            migrationBuilder.CreateIndex(
                name: "IX_webhooks_source_guild_id",
                table: "webhooks",
                column: "source_guild_id");

            migrationBuilder.CreateIndex(
                name: "IX_webhooks_user_id",
                table: "webhooks",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_attachments_messages_message_id",
                table: "attachments",
                column: "message_id",
                principalTable: "messages",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_bans_guilds_guild_id",
                table: "bans",
                column: "guild_id",
                principalTable: "guilds",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMessage_channels_ChannelsId",
                table: "ChannelMessage",
                column: "ChannelsId",
                principalTable: "channels",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMessage_messages_MessagesId",
                table: "ChannelMessage",
                column: "MessagesId",
                principalTable: "messages",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_channels_guilds_guild_id",
                table: "channels",
                column: "guild_id",
                principalTable: "guilds",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_emojis_guilds_guild_id",
                table: "emojis",
                column: "guild_id",
                principalTable: "guilds",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_guilds_templates_template_id",
                table: "guilds",
                column: "template_id",
                principalTable: "templates",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageSticker_stickers_StickersId",
                table: "MessageSticker",
                column: "StickersId",
                principalTable: "stickers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sticker_packs_stickers_coverStickerId",
                table: "sticker_packs",
                column: "coverStickerId",
                principalTable: "stickers",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_channels_users_owner_id",
                table: "channels");

            migrationBuilder.DropForeignKey(
                name: "FK_guilds_users_owner_id",
                table: "guilds");

            migrationBuilder.DropForeignKey(
                name: "FK_stickers_users_user_id",
                table: "stickers");

            migrationBuilder.DropForeignKey(
                name: "FK_templates_users_creator_id",
                table: "templates");

            migrationBuilder.DropForeignKey(
                name: "FK_channels_guilds_guild_id",
                table: "channels");

            migrationBuilder.DropForeignKey(
                name: "FK_stickers_guilds_guild_id",
                table: "stickers");

            migrationBuilder.DropForeignKey(
                name: "FK_templates_guilds_source_guild_id",
                table: "templates");

            migrationBuilder.DropForeignKey(
                name: "FK_sticker_packs_stickers_coverStickerId",
                table: "sticker_packs");

            migrationBuilder.DropTable(
                name: "attachments");

            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "backup_codes");

            migrationBuilder.DropTable(
                name: "bans");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "ChannelMessage");

            migrationBuilder.DropTable(
                name: "client_release");

            migrationBuilder.DropTable(
                name: "connected_accounts");

            migrationBuilder.DropTable(
                name: "emojis");

            migrationBuilder.DropTable(
                name: "invites");

            migrationBuilder.DropTable(
                name: "MemberRole");

            migrationBuilder.DropTable(
                name: "MessageRole");

            migrationBuilder.DropTable(
                name: "MessageSticker");

            migrationBuilder.DropTable(
                name: "MessageUser");

            migrationBuilder.DropTable(
                name: "migrations");

            migrationBuilder.DropTable(
                name: "notes");

            migrationBuilder.DropTable(
                name: "query-result-cache");

            migrationBuilder.DropTable(
                name: "rate_limits");

            migrationBuilder.DropTable(
                name: "read_states");

            migrationBuilder.DropTable(
                name: "recipients");

            migrationBuilder.DropTable(
                name: "relationships");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "team_members");

            migrationBuilder.DropTable(
                name: "valid_registration_tokens");

            migrationBuilder.DropTable(
                name: "voice_states");

            migrationBuilder.DropTable(
                name: "members");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "webhooks");

            migrationBuilder.DropTable(
                name: "applications");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "user_settings");

            migrationBuilder.DropTable(
                name: "guilds");

            migrationBuilder.DropTable(
                name: "channels");

            migrationBuilder.DropTable(
                name: "templates");

            migrationBuilder.DropTable(
                name: "stickers");

            migrationBuilder.DropTable(
                name: "sticker_packs");
        }
    }
}
