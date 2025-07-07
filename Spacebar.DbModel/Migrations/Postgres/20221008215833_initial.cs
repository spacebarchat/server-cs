#nullable disable

using System.Collections;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Spacebar.DbModel.Migrations.Postgres;

public partial class initial : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.CreateTable(
            "categories",
            table => new {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>("character varying", nullable: true),
                localizations = table.Column<string>("text", nullable: false),
                is_primary = table.Column<bool>("boolean", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_categories", x => x.id); });

        migrationBuilder.CreateTable(
            "client_release",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                name = table.Column<string>("character varying", nullable: false),
                pub_date = table.Column<string>("character varying", nullable: false),
                url = table.Column<string>("character varying", nullable: false),
                deb_url = table.Column<string>("character varying", nullable: false),
                osx_url = table.Column<string>("character varying", nullable: false),
                win_url = table.Column<string>("character varying", nullable: false),
                notes = table.Column<string>("character varying", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_client_release", x => x.id); });

        migrationBuilder.CreateTable(
            "migrations",
            table => new {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                timestamp = table.Column<long>("bigint", nullable: false),
                name = table.Column<string>("character varying", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_migrations", x => x.id); });

        migrationBuilder.CreateTable(
            "query-result-cache",
            table => new {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                identifier = table.Column<string>("character varying", nullable: true),
                time = table.Column<long>("bigint", nullable: false),
                duration = table.Column<int>("integer", nullable: false),
                query = table.Column<string>("text", nullable: false),
                result = table.Column<string>("text", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_query-result-cache", x => x.id); });

        migrationBuilder.CreateTable(
            "rate_limits",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                executor_id = table.Column<string>("character varying", nullable: false),
                hits = table.Column<int>("integer", nullable: false),
                blocked = table.Column<bool>("boolean", nullable: false),
                expires_at = table.Column<DateTime>("timestamp without time zone", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_rate_limits", x => x.id); });

        migrationBuilder.CreateTable(
            "user_settings",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                afk_timeout = table.Column<int>("integer", nullable: true),
                allow_accessibility_detection = table.Column<bool>("boolean", nullable: true),
                animate_emoji = table.Column<bool>("boolean", nullable: true),
                animate_stickers = table.Column<int>("integer", nullable: true),
                contact_sync_enabled = table.Column<bool>("boolean", nullable: true),
                convert_emoticons = table.Column<bool>("boolean", nullable: true),
                custom_status = table.Column<string>("text", nullable: true),
                default_guilds_restricted = table.Column<bool>("boolean", nullable: true),
                detect_platform_accounts = table.Column<bool>("boolean", nullable: true),
                developer_mode = table.Column<bool>("boolean", nullable: true),
                disable_games_tab = table.Column<bool>("boolean", nullable: true),
                enable_tts_command = table.Column<bool>("boolean", nullable: true),
                explicit_content_filter = table.Column<int>("integer", nullable: true),
                friend_source_flags = table.Column<string>("text", nullable: true),
                gateway_connected = table.Column<bool>("boolean", nullable: true),
                gif_auto_play = table.Column<bool>("boolean", nullable: true),
                guild_folders = table.Column<string>("text", nullable: true),
                guild_positions = table.Column<string>("text", nullable: true),
                inline_attachment_media = table.Column<bool>("boolean", nullable: true),
                inline_embed_media = table.Column<bool>("boolean", nullable: true),
                locale = table.Column<string>("character varying", nullable: true),
                message_display_compact = table.Column<bool>("boolean", nullable: true),
                native_phone_integration_enabled = table.Column<bool>("boolean", nullable: true),
                render_embeds = table.Column<bool>("boolean", nullable: true),
                render_reactions = table.Column<bool>("boolean", nullable: true),
                restricted_guilds = table.Column<string>("text", nullable: true),
                show_current_game = table.Column<bool>("boolean", nullable: true),
                status = table.Column<string>("character varying", nullable: true),
                stream_notifications_enabled = table.Column<bool>("boolean", nullable: true),
                theme = table.Column<string>("character varying", nullable: true),
                timezone_offset = table.Column<int>("integer", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_user_settings", x => x.id); });

        migrationBuilder.CreateTable(
            "valid_registration_tokens",
            table => new {
                token = table.Column<string>("character varying", nullable: false),
                created_at = table.Column<DateTime>("timestamp without time zone", nullable: false),
                expires_at = table.Column<DateTime>("timestamp without time zone", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_valid_registration_tokens", x => x.token); });

        migrationBuilder.CreateTable(
            "users",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                username = table.Column<string>("character varying", nullable: false),
                discriminator = table.Column<string>("character varying", nullable: false),
                avatar = table.Column<string>("character varying", nullable: true),
                accent_color = table.Column<int>("integer", nullable: true),
                banner = table.Column<string>("character varying", nullable: true),
                phone = table.Column<string>("character varying", nullable: true),
                desktop = table.Column<bool>("boolean", nullable: false),
                mobile = table.Column<bool>("boolean", nullable: false),
                premium = table.Column<bool>("boolean", nullable: false),
                premium_type = table.Column<int>("integer", nullable: false),
                bot = table.Column<bool>("boolean", nullable: false),
                bio = table.Column<string>("character varying", nullable: true),
                system = table.Column<bool>("boolean", nullable: false),
                nsfw_allowed = table.Column<bool>("boolean", nullable: false),
                mfa_enabled = table.Column<bool>("boolean", nullable: true),
                totp_secret = table.Column<string>("character varying", nullable: true),
                totp_last_ticket = table.Column<string>("character varying", nullable: true),
                created_at = table.Column<DateTime>("timestamp without time zone", nullable: false),
                premium_since = table.Column<DateTime>("timestamp without time zone", nullable: true),
                verified = table.Column<bool>("boolean", nullable: false),
                disabled = table.Column<bool>("boolean", nullable: false),
                deleted = table.Column<bool>("boolean", nullable: false),
                email = table.Column<string>("character varying", nullable: true),
                flags = table.Column<string>("character varying", nullable: false),
                public_flags = table.Column<int>("integer", nullable: false),
                rights = table.Column<BitArray>("bit varying", nullable: false),
                data = table.Column<string>("text", nullable: false),
                fingerprints = table.Column<string>("text", nullable: false),
                extended_settings = table.Column<string>("text", nullable: false),
                settingsId = table.Column<string>("character varying", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_users", x => x.id);
                table.ForeignKey(
                    "FK_users_user_settings_settingsId",
                    x => x.settingsId,
                    "user_settings",
                    "id");
            });

        migrationBuilder.CreateTable(
            "audit_logs",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                user_id = table.Column<string>("character varying", nullable: true),
                action_type = table.Column<int>("integer", nullable: false),
                options = table.Column<string>("text", nullable: true),
                changes = table.Column<string>("text", nullable: false),
                reason = table.Column<string>("character varying", nullable: true),
                target_id = table.Column<string>("character varying", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_audit_logs", x => x.id);
                table.ForeignKey(
                    "FK_audit_logs_users_target_id",
                    x => x.target_id,
                    "users",
                    "id");
                table.ForeignKey(
                    "FK_audit_logs_users_user_id",
                    x => x.user_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "backup_codes",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                code = table.Column<string>("character varying", nullable: false),
                consumed = table.Column<bool>("boolean", nullable: false),
                expired = table.Column<bool>("boolean", nullable: false),
                user_id = table.Column<string>("character varying", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_backup_codes", x => x.id);
                table.ForeignKey(
                    "FK_backup_codes_users_user_id",
                    x => x.user_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "connected_accounts",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                user_id = table.Column<string>("character varying", nullable: true),
                access_token = table.Column<string>("character varying", nullable: false),
                friend_sync = table.Column<bool>("boolean", nullable: false),
                name = table.Column<string>("character varying", nullable: false),
                revoked = table.Column<bool>("boolean", nullable: false),
                show_activity = table.Column<bool>("boolean", nullable: false),
                type = table.Column<string>("character varying", nullable: false),
                verified = table.Column<bool>("boolean", nullable: false),
                visibility = table.Column<int>("integer", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_connected_accounts", x => x.id);
                table.ForeignKey(
                    "FK_connected_accounts_users_user_id",
                    x => x.user_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "notes",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                content = table.Column<string>("character varying", nullable: false),
                owner_id = table.Column<string>("character varying", nullable: true),
                target_id = table.Column<string>("character varying", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_notes", x => x.id);
                table.ForeignKey(
                    "FK_notes_users_owner_id",
                    x => x.owner_id,
                    "users",
                    "id");
                table.ForeignKey(
                    "FK_notes_users_target_id",
                    x => x.target_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "relationships",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                from_id = table.Column<string>("character varying", nullable: false),
                to_id = table.Column<string>("character varying", nullable: false),
                nickname = table.Column<string>("character varying", nullable: true),
                type = table.Column<int>("integer", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_relationships", x => x.id);
                table.ForeignKey(
                    "FK_relationships_users_from_id",
                    x => x.from_id,
                    "users",
                    "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_relationships_users_to_id",
                    x => x.to_id,
                    "users",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "sessions",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                user_id = table.Column<string>("character varying", nullable: true),
                session_id = table.Column<string>("character varying", nullable: false),
                activities = table.Column<string>("text", nullable: true),
                client_info = table.Column<string>("text", nullable: false),
                status = table.Column<string>("character varying", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_sessions", x => x.id);
                table.ForeignKey(
                    "FK_sessions_users_user_id",
                    x => x.user_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "teams",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                icon = table.Column<string>("character varying", nullable: true),
                name = table.Column<string>("character varying", nullable: false),
                owner_user_id = table.Column<string>("character varying", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_teams", x => x.id);
                table.ForeignKey(
                    "FK_teams_users_owner_user_id",
                    x => x.owner_user_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "applications",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                name = table.Column<string>("character varying", nullable: false),
                icon = table.Column<string>("character varying", nullable: true),
                description = table.Column<string>("character varying", nullable: true),
                bot_public = table.Column<bool>("boolean", nullable: false),
                bot_require_code_grant = table.Column<bool>("boolean", nullable: false),
                terms_of_service_url = table.Column<string>("character varying", nullable: true),
                privacy_policy_url = table.Column<string>("character varying", nullable: true),
                summary = table.Column<string>("character varying", nullable: true),
                verify_key = table.Column<string>("character varying", nullable: false),
                cover_image = table.Column<string>("character varying", nullable: true),
                owner_id = table.Column<string>("character varying", nullable: true),
                team_id = table.Column<string>("character varying", nullable: true),
                type = table.Column<string>("text", nullable: true),
                hook = table.Column<bool>("boolean", nullable: false),
                redirect_uris = table.Column<string>("text", nullable: true),
                rpc_application_state = table.Column<int>("integer", nullable: true),
                store_application_state = table.Column<int>("integer", nullable: true),
                verification_state = table.Column<int>("integer", nullable: true),
                interactions_endpoint_url = table.Column<string>("character varying", nullable: true),
                integration_public = table.Column<bool>("boolean", nullable: true),
                integration_require_code_grant = table.Column<bool>("boolean", nullable: true),
                discoverability_state = table.Column<int>("integer", nullable: true),
                discovery_eligibility_flags = table.Column<int>("integer", nullable: true),
                tags = table.Column<string>("text", nullable: true),
                install_params = table.Column<string>("text", nullable: true),
                bot_user_id = table.Column<string>("character varying", nullable: true),
                flags = table.Column<int>("integer", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_applications", x => x.id);
                table.ForeignKey(
                    "FK_applications_teams_team_id",
                    x => x.team_id,
                    "teams",
                    "id");
                table.ForeignKey(
                    "FK_applications_users_bot_user_id",
                    x => x.bot_user_id,
                    "users",
                    "id");
                table.ForeignKey(
                    "FK_applications_users_owner_id",
                    x => x.owner_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "team_members",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                membership_state = table.Column<int>("integer", nullable: false),
                permissions = table.Column<string>("text", nullable: false),
                team_id = table.Column<string>("character varying", nullable: true),
                user_id = table.Column<string>("character varying", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_team_members", x => x.id);
                table.ForeignKey(
                    "FK_team_members_teams_team_id",
                    x => x.team_id,
                    "teams",
                    "id");
                table.ForeignKey(
                    "FK_team_members_users_user_id",
                    x => x.user_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "attachments",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                filename = table.Column<string>("character varying", nullable: false),
                size = table.Column<int>("integer", nullable: false),
                url = table.Column<string>("character varying", nullable: false),
                proxy_url = table.Column<string>("character varying", nullable: false),
                height = table.Column<int>("integer", nullable: true),
                width = table.Column<int>("integer", nullable: true),
                content_type = table.Column<string>("character varying", nullable: true),
                message_id = table.Column<string>("character varying", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_attachments", x => x.id); });

        migrationBuilder.CreateTable(
            "bans",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                user_id = table.Column<string>("character varying", nullable: true),
                guild_id = table.Column<string>("character varying", nullable: true),
                executor_id = table.Column<string>("character varying", nullable: true),
                ip = table.Column<string>("character varying", nullable: false),
                reason = table.Column<string>("character varying", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_bans", x => x.id);
                table.ForeignKey(
                    "FK_bans_users_executor_id",
                    x => x.executor_id,
                    "users",
                    "id");
                table.ForeignKey(
                    "FK_bans_users_user_id",
                    x => x.user_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "ChannelMessage",
            table => new {
                ChannelsId = table.Column<string>("character varying", nullable: false),
                MessagesId = table.Column<string>("character varying", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_ChannelMessage", x => new { x.ChannelsId, x.MessagesId }); });

        migrationBuilder.CreateTable(
            "channels",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                created_at = table.Column<DateTime>("timestamp without time zone", nullable: false),
                name = table.Column<string>("character varying", nullable: true),
                icon = table.Column<string>("text", nullable: true),
                type = table.Column<int>("integer", nullable: false),
                last_message_id = table.Column<string>("character varying", nullable: true),
                guild_id = table.Column<string>("character varying", nullable: true),
                parent_id = table.Column<string>("character varying", nullable: true),
                owner_id = table.Column<string>("character varying", nullable: true),
                last_pin_timestamp = table.Column<int>("integer", nullable: true),
                default_auto_archive_duration = table.Column<int>("integer", nullable: true),
                position = table.Column<int>("integer", nullable: true),
                permission_overwrites = table.Column<string>("text", nullable: true),
                video_quality_mode = table.Column<int>("integer", nullable: true),
                bitrate = table.Column<int>("integer", nullable: true),
                user_limit = table.Column<int>("integer", nullable: true),
                nsfw = table.Column<bool>("boolean", nullable: true),
                rate_limit_per_user = table.Column<int>("integer", nullable: true),
                topic = table.Column<string>("character varying", nullable: true),
                retention_policy_id = table.Column<string>("character varying", nullable: true),
                flags = table.Column<int>("integer", nullable: true),
                default_thread_rate_limit_per_user = table.Column<int>("integer", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_channels", x => x.id);
                table.ForeignKey(
                    "FK_channels_channels_parent_id",
                    x => x.parent_id,
                    "channels",
                    "id");
                table.ForeignKey(
                    "FK_channels_users_owner_id",
                    x => x.owner_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "read_states",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                channel_id = table.Column<string>("character varying", nullable: false),
                user_id = table.Column<string>("character varying", nullable: false),
                last_message_id = table.Column<string>("character varying", nullable: true),
                public_ack = table.Column<string>("character varying", nullable: true),
                notifications_cursor = table.Column<string>("character varying", nullable: true),
                last_pin_timestamp = table.Column<DateTime>("timestamp without time zone", nullable: true),
                mention_count = table.Column<int>("integer", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_read_states", x => x.id);
                table.ForeignKey(
                    "FK_read_states_channels_channel_id",
                    x => x.channel_id,
                    "channels",
                    "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_read_states_users_user_id",
                    x => x.user_id,
                    "users",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "recipients",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                channel_id = table.Column<string>("character varying", nullable: false),
                user_id = table.Column<string>("character varying", nullable: false),
                closed = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_recipients", x => x.id);
                table.ForeignKey(
                    "FK_recipients_channels_channel_id",
                    x => x.channel_id,
                    "channels",
                    "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_recipients_users_user_id",
                    x => x.user_id,
                    "users",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "emojis",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                animated = table.Column<bool>("boolean", nullable: false),
                available = table.Column<bool>("boolean", nullable: false),
                guild_id = table.Column<string>("character varying", nullable: false),
                user_id = table.Column<string>("character varying", nullable: true),
                managed = table.Column<bool>("boolean", nullable: false),
                name = table.Column<string>("character varying", nullable: false),
                require_colons = table.Column<bool>("boolean", nullable: false),
                roles = table.Column<string>("text", nullable: false),
                groups = table.Column<string>("text", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_emojis", x => x.id);
                table.ForeignKey(
                    "FK_emojis_users_user_id",
                    x => x.user_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "guilds",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                afk_channel_id = table.Column<string>("character varying", nullable: true),
                afk_timeout = table.Column<int>("integer", nullable: true),
                banner = table.Column<string>("character varying", nullable: true),
                default_message_notifications = table.Column<int>("integer", nullable: true),
                description = table.Column<string>("character varying", nullable: true),
                discovery_splash = table.Column<string>("character varying", nullable: true),
                explicit_content_filter = table.Column<int>("integer", nullable: true),
                features = table.Column<string>("text", nullable: false),
                primary_category_id = table.Column<int>("integer", nullable: true),
                icon = table.Column<string>("character varying", nullable: true),
                large = table.Column<bool>("boolean", nullable: true),
                max_members = table.Column<int>("integer", nullable: true),
                max_presences = table.Column<int>("integer", nullable: true),
                max_video_channel_users = table.Column<int>("integer", nullable: true),
                member_count = table.Column<int>("integer", nullable: true),
                presence_count = table.Column<int>("integer", nullable: true),
                template_id = table.Column<string>("character varying", nullable: true),
                mfa_level = table.Column<int>("integer", nullable: true),
                name = table.Column<string>("character varying", nullable: false),
                owner_id = table.Column<string>("character varying", nullable: true),
                preferred_locale = table.Column<string>("character varying", nullable: true),
                premium_subscription_count = table.Column<int>("integer", nullable: true),
                premium_tier = table.Column<int>("integer", nullable: true),
                public_updates_channel_id = table.Column<string>("character varying", nullable: true),
                rules_channel_id = table.Column<string>("character varying", nullable: true),
                region = table.Column<string>("character varying", nullable: true),
                splash = table.Column<string>("character varying", nullable: true),
                system_channel_id = table.Column<string>("character varying", nullable: true),
                system_channel_flags = table.Column<int>("integer", nullable: true),
                unavailable = table.Column<bool>("boolean", nullable: true),
                verification_level = table.Column<int>("integer", nullable: true),
                welcome_screen = table.Column<string>("text", nullable: false),
                widget_channel_id = table.Column<string>("character varying", nullable: true),
                widget_enabled = table.Column<bool>("boolean", nullable: true),
                nsfw_level = table.Column<int>("integer", nullable: true),
                nsfw = table.Column<bool>("boolean", nullable: true),
                parent = table.Column<string>("character varying", nullable: true),
                premium_progress_bar_enabled = table.Column<bool>("boolean", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_guilds", x => x.id);
                table.ForeignKey(
                    "FK_guilds_channels_afk_channel_id",
                    x => x.afk_channel_id,
                    "channels",
                    "id");
                table.ForeignKey(
                    "FK_guilds_channels_public_updates_channel_id",
                    x => x.public_updates_channel_id,
                    "channels",
                    "id");
                table.ForeignKey(
                    "FK_guilds_channels_rules_channel_id",
                    x => x.rules_channel_id,
                    "channels",
                    "id");
                table.ForeignKey(
                    "FK_guilds_channels_system_channel_id",
                    x => x.system_channel_id,
                    "channels",
                    "id");
                table.ForeignKey(
                    "FK_guilds_channels_widget_channel_id",
                    x => x.widget_channel_id,
                    "channels",
                    "id");
                table.ForeignKey(
                    "FK_guilds_users_owner_id",
                    x => x.owner_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "invites",
            table => new {
                code = table.Column<string>("character varying", nullable: false),
                temporary = table.Column<bool>("boolean", nullable: false),
                uses = table.Column<int>("integer", nullable: false),
                max_uses = table.Column<int>("integer", nullable: false),
                max_age = table.Column<int>("integer", nullable: false),
                created_at = table.Column<DateTime>("timestamp without time zone", nullable: false),
                expires_at = table.Column<DateTime>("timestamp without time zone", nullable: false),
                guild_id = table.Column<string>("character varying", nullable: true),
                channel_id = table.Column<string>("character varying", nullable: true),
                inviter_id = table.Column<string>("character varying", nullable: true),
                target_user_id = table.Column<string>("character varying", nullable: true),
                target_user_type = table.Column<int>("integer", nullable: true),
                vanity_url = table.Column<bool>("boolean", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_invites", x => x.code);
                table.ForeignKey(
                    "FK_invites_channels_channel_id",
                    x => x.channel_id,
                    "channels",
                    "id");
                table.ForeignKey(
                    "FK_invites_guilds_guild_id",
                    x => x.guild_id,
                    "guilds",
                    "id");
                table.ForeignKey(
                    "FK_invites_users_inviter_id",
                    x => x.inviter_id,
                    "users",
                    "id");
                table.ForeignKey(
                    "FK_invites_users_target_user_id",
                    x => x.target_user_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "members",
            table => new {
                index = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                id = table.Column<string>("character varying", nullable: false),
                guild_id = table.Column<string>("character varying", nullable: false),
                nick = table.Column<string>("character varying", nullable: true),
                joined_at = table.Column<DateTime>("timestamp without time zone", nullable: false),
                deaf = table.Column<bool>("boolean", nullable: false),
                mute = table.Column<bool>("boolean", nullable: false),
                pending = table.Column<bool>("boolean", nullable: false),
                settings = table.Column<string>("text", nullable: false),
                last_message_id = table.Column<string>("character varying", nullable: true),
                joined_by = table.Column<string>("character varying", nullable: true),
                premium_since = table.Column<DateTime>("timestamp without time zone", nullable: true),
                avatar = table.Column<string>("character varying", nullable: true),
                banner = table.Column<string>("character varying", nullable: true),
                bio = table.Column<string>("character varying", nullable: false),
                communication_disabled_until = table.Column<DateTime>("timestamp without time zone", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_members", x => x.index);
                table.ForeignKey(
                    "FK_members_guilds_guild_id",
                    x => x.guild_id,
                    "guilds",
                    "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_members_users_id",
                    x => x.id,
                    "users",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "roles",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                guild_id = table.Column<string>("character varying", nullable: true),
                color = table.Column<int>("integer", nullable: false),
                hoist = table.Column<bool>("boolean", nullable: false),
                managed = table.Column<bool>("boolean", nullable: false),
                mentionable = table.Column<bool>("boolean", nullable: false),
                name = table.Column<string>("character varying", nullable: false),
                permissions = table.Column<string>("character varying", nullable: false),
                position = table.Column<int>("integer", nullable: false),
                icon = table.Column<string>("character varying", nullable: true),
                unicode_emoji = table.Column<string>("character varying", nullable: true),
                tags = table.Column<string>("text", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_roles", x => x.id);
                table.ForeignKey(
                    "FK_roles_guilds_guild_id",
                    x => x.guild_id,
                    "guilds",
                    "id");
            });

        migrationBuilder.CreateTable(
            "templates",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                code = table.Column<string>("character varying", nullable: false),
                name = table.Column<string>("character varying", nullable: false),
                description = table.Column<string>("character varying", nullable: true),
                usage_count = table.Column<int>("integer", nullable: true),
                creator_id = table.Column<string>("character varying", nullable: true),
                created_at = table.Column<DateTime>("timestamp without time zone", nullable: false),
                updated_at = table.Column<DateTime>("timestamp without time zone", nullable: false),
                source_guild_id = table.Column<string>("character varying", nullable: true),
                serialized_source_guild = table.Column<string>("text", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_templates", x => x.id);
                table.ForeignKey(
                    "FK_templates_guilds_source_guild_id",
                    x => x.source_guild_id,
                    "guilds",
                    "id");
                table.ForeignKey(
                    "FK_templates_users_creator_id",
                    x => x.creator_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "voice_states",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                guild_id = table.Column<string>("character varying", nullable: true),
                channel_id = table.Column<string>("character varying", nullable: true),
                user_id = table.Column<string>("character varying", nullable: true),
                session_id = table.Column<string>("character varying", nullable: false),
                token = table.Column<string>("character varying", nullable: true),
                deaf = table.Column<bool>("boolean", nullable: false),
                mute = table.Column<bool>("boolean", nullable: false),
                self_deaf = table.Column<bool>("boolean", nullable: false),
                self_mute = table.Column<bool>("boolean", nullable: false),
                self_stream = table.Column<bool>("boolean", nullable: true),
                self_video = table.Column<bool>("boolean", nullable: false),
                suppress = table.Column<bool>("boolean", nullable: false),
                request_to_speak_timestamp = table.Column<DateTime>("timestamp without time zone", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_voice_states", x => x.id);
                table.ForeignKey(
                    "FK_voice_states_channels_channel_id",
                    x => x.channel_id,
                    "channels",
                    "id");
                table.ForeignKey(
                    "FK_voice_states_guilds_guild_id",
                    x => x.guild_id,
                    "guilds",
                    "id");
                table.ForeignKey(
                    "FK_voice_states_users_user_id",
                    x => x.user_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "webhooks",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                type = table.Column<int>("integer", nullable: false),
                name = table.Column<string>("character varying", nullable: true),
                avatar = table.Column<string>("character varying", nullable: true),
                token = table.Column<string>("character varying", nullable: true),
                guild_id = table.Column<string>("character varying", nullable: true),
                channel_id = table.Column<string>("character varying", nullable: true),
                application_id = table.Column<string>("character varying", nullable: true),
                user_id = table.Column<string>("character varying", nullable: true),
                source_guild_id = table.Column<string>("character varying", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_webhooks", x => x.id);
                table.ForeignKey(
                    "FK_webhooks_applications_application_id",
                    x => x.application_id,
                    "applications",
                    "id");
                table.ForeignKey(
                    "FK_webhooks_channels_channel_id",
                    x => x.channel_id,
                    "channels",
                    "id");
                table.ForeignKey(
                    "FK_webhooks_guilds_guild_id",
                    x => x.guild_id,
                    "guilds",
                    "id");
                table.ForeignKey(
                    "FK_webhooks_guilds_source_guild_id",
                    x => x.source_guild_id,
                    "guilds",
                    "id");
                table.ForeignKey(
                    "FK_webhooks_users_user_id",
                    x => x.user_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateTable(
            "MemberRole",
            table => new {
                Index = table.Column<int>("integer", nullable: false),
                RoleId = table.Column<string>("character varying", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_MemberRole", x => new { x.Index, x.RoleId });
                table.ForeignKey(
                    "FK_MemberRole_members_Index",
                    x => x.Index,
                    "members",
                    "index",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_MemberRole_roles_RoleId",
                    x => x.RoleId,
                    "roles",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "messages",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                channel_id = table.Column<string>("character varying", nullable: true),
                guild_id = table.Column<string>("character varying", nullable: true),
                author_id = table.Column<string>("character varying", nullable: true),
                member_id = table.Column<string>("character varying", nullable: true),
                webhook_id = table.Column<string>("character varying", nullable: true),
                application_id = table.Column<string>("character varying", nullable: true),
                content = table.Column<string>("character varying", nullable: true),
                timestamp = table.Column<DateTime>("timestamp without time zone", nullable: false),
                edited_timestamp = table.Column<DateTime>("timestamp without time zone", nullable: true),
                tts = table.Column<bool>("boolean", nullable: true),
                mention_everyone = table.Column<bool>("boolean", nullable: true),
                embeds = table.Column<string>("text", nullable: false),
                reactions = table.Column<string>("text", nullable: false),
                nonce = table.Column<string>("text", nullable: true),
                pinned = table.Column<bool>("boolean", nullable: true),
                type = table.Column<int>("integer", nullable: false),
                activity = table.Column<string>("text", nullable: true),
                flags = table.Column<string>("character varying", nullable: true),
                message_reference = table.Column<string>("text", nullable: true),
                interaction = table.Column<string>("text", nullable: true),
                components = table.Column<string>("text", nullable: true),
                message_reference_id = table.Column<string>("character varying", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_messages", x => x.id);
                table.ForeignKey(
                    "FK_messages_applications_application_id",
                    x => x.application_id,
                    "applications",
                    "id");
                table.ForeignKey(
                    "FK_messages_channels_channel_id",
                    x => x.channel_id,
                    "channels",
                    "id");
                table.ForeignKey(
                    "FK_messages_guilds_guild_id",
                    x => x.guild_id,
                    "guilds",
                    "id");
                table.ForeignKey(
                    "FK_messages_messages_message_reference_id",
                    x => x.message_reference_id,
                    "messages",
                    "id");
                table.ForeignKey(
                    "FK_messages_users_author_id",
                    x => x.author_id,
                    "users",
                    "id");
                table.ForeignKey(
                    "FK_messages_users_member_id",
                    x => x.member_id,
                    "users",
                    "id");
                table.ForeignKey(
                    "FK_messages_webhooks_webhook_id",
                    x => x.webhook_id,
                    "webhooks",
                    "id");
            });

        migrationBuilder.CreateTable(
            "MessageRole",
            table => new {
                MessagesId = table.Column<string>("character varying", nullable: false),
                RolesId = table.Column<string>("character varying", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_MessageRole", x => new { x.MessagesId, x.RolesId });
                table.ForeignKey(
                    "FK_MessageRole_messages_MessagesId",
                    x => x.MessagesId,
                    "messages",
                    "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_MessageRole_roles_RolesId",
                    x => x.RolesId,
                    "roles",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "MessageUser",
            table => new {
                MessagesId = table.Column<string>("character varying", nullable: false),
                UsersId = table.Column<string>("character varying", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_MessageUser", x => new { x.MessagesId, x.UsersId });
                table.ForeignKey(
                    "FK_MessageUser_messages_MessagesId",
                    x => x.MessagesId,
                    "messages",
                    "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_MessageUser_users_UsersId",
                    x => x.UsersId,
                    "users",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "MessageSticker",
            table => new {
                MessagesId = table.Column<string>("character varying", nullable: false),
                StickersId = table.Column<string>("character varying", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_MessageSticker", x => new { x.MessagesId, x.StickersId });
                table.ForeignKey(
                    "FK_MessageSticker_messages_MessagesId",
                    x => x.MessagesId,
                    "messages",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "sticker_packs",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                name = table.Column<string>("character varying", nullable: false),
                description = table.Column<string>("character varying", nullable: true),
                banner_asset_id = table.Column<string>("character varying", nullable: true),
                cover_sticker_id = table.Column<string>("character varying", nullable: true),
                coverStickerId = table.Column<string>("character varying", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_sticker_packs", x => x.id); });

        migrationBuilder.CreateTable(
            "stickers",
            table => new {
                id = table.Column<string>("character varying", nullable: false),
                name = table.Column<string>("character varying", nullable: false),
                description = table.Column<string>("character varying", nullable: true),
                available = table.Column<bool>("boolean", nullable: true),
                tags = table.Column<string>("character varying", nullable: true),
                pack_id = table.Column<string>("character varying", nullable: true),
                guild_id = table.Column<string>("character varying", nullable: true),
                user_id = table.Column<string>("character varying", nullable: true),
                type = table.Column<int>("integer", nullable: false),
                format_type = table.Column<int>("integer", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_stickers", x => x.id);
                table.ForeignKey(
                    "FK_stickers_guilds_guild_id",
                    x => x.guild_id,
                    "guilds",
                    "id");
                table.ForeignKey(
                    "FK_stickers_sticker_packs_pack_id",
                    x => x.pack_id,
                    "sticker_packs",
                    "id");
                table.ForeignKey(
                    "FK_stickers_users_user_id",
                    x => x.user_id,
                    "users",
                    "id");
            });

        migrationBuilder.CreateIndex(
            "IX_applications_owner_id",
            "applications",
            "owner_id");

        migrationBuilder.CreateIndex(
            "IX_applications_team_id",
            "applications",
            "team_id");

        migrationBuilder.CreateIndex(
            "UQ_2ce5a55796fe4c2f77ece57a647",
            "applications",
            "bot_user_id",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_attachments_message_id",
            "attachments",
            "message_id");

        migrationBuilder.CreateIndex(
            "IX_audit_logs_target_id",
            "audit_logs",
            "target_id");

        migrationBuilder.CreateIndex(
            "IX_audit_logs_user_id",
            "audit_logs",
            "user_id");

        migrationBuilder.CreateIndex(
            "IX_backup_codes_user_id",
            "backup_codes",
            "user_id");

        migrationBuilder.CreateIndex(
            "IX_bans_executor_id",
            "bans",
            "executor_id");

        migrationBuilder.CreateIndex(
            "IX_bans_guild_id",
            "bans",
            "guild_id");

        migrationBuilder.CreateIndex(
            "IX_bans_user_id",
            "bans",
            "user_id");

        migrationBuilder.CreateIndex(
            "IX_ChannelMessage_MessagesId",
            "ChannelMessage",
            "MessagesId");

        migrationBuilder.CreateIndex(
            "IX_channels_guild_id",
            "channels",
            "guild_id");

        migrationBuilder.CreateIndex(
            "IX_channels_owner_id",
            "channels",
            "owner_id");

        migrationBuilder.CreateIndex(
            "IX_channels_parent_id",
            "channels",
            "parent_id");

        migrationBuilder.CreateIndex(
            "IX_connected_accounts_user_id",
            "connected_accounts",
            "user_id");

        migrationBuilder.CreateIndex(
            "IX_emojis_guild_id",
            "emojis",
            "guild_id");

        migrationBuilder.CreateIndex(
            "IX_emojis_user_id",
            "emojis",
            "user_id");

        migrationBuilder.CreateIndex(
            "IX_guilds_afk_channel_id",
            "guilds",
            "afk_channel_id");

        migrationBuilder.CreateIndex(
            "IX_guilds_owner_id",
            "guilds",
            "owner_id");

        migrationBuilder.CreateIndex(
            "IX_guilds_public_updates_channel_id",
            "guilds",
            "public_updates_channel_id");

        migrationBuilder.CreateIndex(
            "IX_guilds_rules_channel_id",
            "guilds",
            "rules_channel_id");

        migrationBuilder.CreateIndex(
            "IX_guilds_system_channel_id",
            "guilds",
            "system_channel_id");

        migrationBuilder.CreateIndex(
            "IX_guilds_template_id",
            "guilds",
            "template_id");

        migrationBuilder.CreateIndex(
            "IX_guilds_widget_channel_id",
            "guilds",
            "widget_channel_id");

        migrationBuilder.CreateIndex(
            "IX_invites_channel_id",
            "invites",
            "channel_id");

        migrationBuilder.CreateIndex(
            "IX_invites_guild_id",
            "invites",
            "guild_id");

        migrationBuilder.CreateIndex(
            "IX_invites_inviter_id",
            "invites",
            "inviter_id");

        migrationBuilder.CreateIndex(
            "IX_invites_target_user_id",
            "invites",
            "target_user_id");

        migrationBuilder.CreateIndex(
            "IX_MemberRole_RoleId",
            "MemberRole",
            "RoleId");

        migrationBuilder.CreateIndex(
            "IDX_bb2bf9386ac443afbbbf9f12d3",
            "members",
            new[] { "id", "guild_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_members_guild_id",
            "members",
            "guild_id");

        migrationBuilder.CreateIndex(
            "IX_MessageRole_RolesId",
            "MessageRole",
            "RolesId");

        migrationBuilder.CreateIndex(
            "IDX_05535bc695e9f7ee104616459d",
            "messages",
            "author_id");

        migrationBuilder.CreateIndex(
            "IDX_3ed7a60fb7dbe04e1ba9332a8b",
            "messages",
            new[] { "channel_id", "id" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IDX_86b9109b155eb70c0a2ca3b4b6",
            "messages",
            "channel_id");

        migrationBuilder.CreateIndex(
            "IX_messages_application_id",
            "messages",
            "application_id");

        migrationBuilder.CreateIndex(
            "IX_messages_guild_id",
            "messages",
            "guild_id");

        migrationBuilder.CreateIndex(
            "IX_messages_member_id",
            "messages",
            "member_id");

        migrationBuilder.CreateIndex(
            "IX_messages_message_reference_id",
            "messages",
            "message_reference_id");

        migrationBuilder.CreateIndex(
            "IX_messages_webhook_id",
            "messages",
            "webhook_id");

        migrationBuilder.CreateIndex(
            "IX_MessageSticker_StickersId",
            "MessageSticker",
            "StickersId");

        migrationBuilder.CreateIndex(
            "IX_MessageUser_UsersId",
            "MessageUser",
            "UsersId");

        migrationBuilder.CreateIndex(
            "IX_notes_target_id",
            "notes",
            "target_id");

        migrationBuilder.CreateIndex(
            "UQ_74e6689b9568cc965b8bfc9150b",
            "notes",
            new[] { "owner_id", "target_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IDX_0abf8b443321bd3cf7f81ee17a",
            "read_states",
            new[] { "channel_id", "user_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_read_states_user_id",
            "read_states",
            "user_id");

        migrationBuilder.CreateIndex(
            "IX_recipients_channel_id",
            "recipients",
            "channel_id");

        migrationBuilder.CreateIndex(
            "IX_recipients_user_id",
            "recipients",
            "user_id");

        migrationBuilder.CreateIndex(
            "IDX_a0b2ff0a598df0b0d055934a17",
            "relationships",
            new[] { "from_id", "to_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_relationships_to_id",
            "relationships",
            "to_id");

        migrationBuilder.CreateIndex(
            "IX_roles_guild_id",
            "roles",
            "guild_id");

        migrationBuilder.CreateIndex(
            "IX_sessions_user_id",
            "sessions",
            "user_id");

        migrationBuilder.CreateIndex(
            "IX_sticker_packs_coverStickerId",
            "sticker_packs",
            "coverStickerId");

        migrationBuilder.CreateIndex(
            "IX_stickers_guild_id",
            "stickers",
            "guild_id");

        migrationBuilder.CreateIndex(
            "IX_stickers_pack_id",
            "stickers",
            "pack_id");

        migrationBuilder.CreateIndex(
            "IX_stickers_user_id",
            "stickers",
            "user_id");

        migrationBuilder.CreateIndex(
            "IX_team_members_team_id",
            "team_members",
            "team_id");

        migrationBuilder.CreateIndex(
            "IX_team_members_user_id",
            "team_members",
            "user_id");

        migrationBuilder.CreateIndex(
            "IX_teams_owner_user_id",
            "teams",
            "owner_user_id");

        migrationBuilder.CreateIndex(
            "IX_templates_creator_id",
            "templates",
            "creator_id");

        migrationBuilder.CreateIndex(
            "IX_templates_source_guild_id",
            "templates",
            "source_guild_id");

        migrationBuilder.CreateIndex(
            "UQ_be38737bf339baf63b1daeffb55",
            "templates",
            "code",
            unique: true);

        migrationBuilder.CreateIndex(
            "UQ_76ba283779c8441fd5ff819c8cf",
            "users",
            "settingsId",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_voice_states_channel_id",
            "voice_states",
            "channel_id");

        migrationBuilder.CreateIndex(
            "IX_voice_states_guild_id",
            "voice_states",
            "guild_id");

        migrationBuilder.CreateIndex(
            "IX_voice_states_user_id",
            "voice_states",
            "user_id");

        migrationBuilder.CreateIndex(
            "IX_webhooks_application_id",
            "webhooks",
            "application_id");

        migrationBuilder.CreateIndex(
            "IX_webhooks_channel_id",
            "webhooks",
            "channel_id");

        migrationBuilder.CreateIndex(
            "IX_webhooks_guild_id",
            "webhooks",
            "guild_id");

        migrationBuilder.CreateIndex(
            "IX_webhooks_source_guild_id",
            "webhooks",
            "source_guild_id");

        migrationBuilder.CreateIndex(
            "IX_webhooks_user_id",
            "webhooks",
            "user_id");

        migrationBuilder.AddForeignKey(
            "FK_attachments_messages_message_id",
            "attachments",
            "message_id",
            "messages",
            principalColumn: "id");

        migrationBuilder.AddForeignKey(
            "FK_bans_guilds_guild_id",
            "bans",
            "guild_id",
            "guilds",
            principalColumn: "id");

        migrationBuilder.AddForeignKey(
            "FK_ChannelMessage_channels_ChannelsId",
            "ChannelMessage",
            "ChannelsId",
            "channels",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            "FK_ChannelMessage_messages_MessagesId",
            "ChannelMessage",
            "MessagesId",
            "messages",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            "FK_channels_guilds_guild_id",
            "channels",
            "guild_id",
            "guilds",
            principalColumn: "id");

        migrationBuilder.AddForeignKey(
            "FK_emojis_guilds_guild_id",
            "emojis",
            "guild_id",
            "guilds",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            "FK_guilds_templates_template_id",
            "guilds",
            "template_id",
            "templates",
            principalColumn: "id");

        migrationBuilder.AddForeignKey(
            "FK_MessageSticker_stickers_StickersId",
            "MessageSticker",
            "StickersId",
            "stickers",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            "FK_sticker_packs_stickers_coverStickerId",
            "sticker_packs",
            "coverStickerId",
            "stickers",
            principalColumn: "id");
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropForeignKey(
            "FK_channels_users_owner_id",
            "channels");

        migrationBuilder.DropForeignKey(
            "FK_guilds_users_owner_id",
            "guilds");

        migrationBuilder.DropForeignKey(
            "FK_stickers_users_user_id",
            "stickers");

        migrationBuilder.DropForeignKey(
            "FK_templates_users_creator_id",
            "templates");

        migrationBuilder.DropForeignKey(
            "FK_channels_guilds_guild_id",
            "channels");

        migrationBuilder.DropForeignKey(
            "FK_stickers_guilds_guild_id",
            "stickers");

        migrationBuilder.DropForeignKey(
            "FK_templates_guilds_source_guild_id",
            "templates");

        migrationBuilder.DropForeignKey(
            "FK_sticker_packs_stickers_coverStickerId",
            "sticker_packs");

        migrationBuilder.DropTable(
            "attachments");

        migrationBuilder.DropTable(
            "audit_logs");

        migrationBuilder.DropTable(
            "backup_codes");

        migrationBuilder.DropTable(
            "bans");

        migrationBuilder.DropTable(
            "categories");

        migrationBuilder.DropTable(
            "ChannelMessage");

        migrationBuilder.DropTable(
            "client_release");

        migrationBuilder.DropTable(
            "connected_accounts");

        migrationBuilder.DropTable(
            "emojis");

        migrationBuilder.DropTable(
            "invites");

        migrationBuilder.DropTable(
            "MemberRole");

        migrationBuilder.DropTable(
            "MessageRole");

        migrationBuilder.DropTable(
            "MessageSticker");

        migrationBuilder.DropTable(
            "MessageUser");

        migrationBuilder.DropTable(
            "migrations");

        migrationBuilder.DropTable(
            "notes");

        migrationBuilder.DropTable(
            "query-result-cache");

        migrationBuilder.DropTable(
            "rate_limits");

        migrationBuilder.DropTable(
            "read_states");

        migrationBuilder.DropTable(
            "recipients");

        migrationBuilder.DropTable(
            "relationships");

        migrationBuilder.DropTable(
            "sessions");

        migrationBuilder.DropTable(
            "team_members");

        migrationBuilder.DropTable(
            "valid_registration_tokens");

        migrationBuilder.DropTable(
            "voice_states");

        migrationBuilder.DropTable(
            "members");

        migrationBuilder.DropTable(
            "roles");

        migrationBuilder.DropTable(
            "messages");

        migrationBuilder.DropTable(
            "webhooks");

        migrationBuilder.DropTable(
            "applications");

        migrationBuilder.DropTable(
            "teams");

        migrationBuilder.DropTable(
            "users");

        migrationBuilder.DropTable(
            "user_settings");

        migrationBuilder.DropTable(
            "guilds");

        migrationBuilder.DropTable(
            "channels");

        migrationBuilder.DropTable(
            "templates");

        migrationBuilder.DropTable(
            "stickers");

        migrationBuilder.DropTable(
            "sticker_packs");
    }
}