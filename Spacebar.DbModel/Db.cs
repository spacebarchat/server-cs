using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Spacebar.DbModel.Entities;

namespace Spacebar.DbModel;

public class Db : DbContext {
    [NotMapped] private static List<Db> contexts = new();

    //props
    public bool InUse = true;

    //db
    public Db(DbContextOptions<Db> options)
        : base(options) { }

    public virtual DbSet<Application> Applications { get; set; }
    public virtual DbSet<Attachment> Attachments { get; set; }
    public virtual DbSet<AuditLog> AuditLogs { get; set; }
    public virtual DbSet<BackupCode> BackupCodes { get; set; }
    public virtual DbSet<Ban> Bans { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Channel> Channels { get; set; }
    public virtual DbSet<ClientRelease> ClientReleases { get; set; }
    public virtual DbSet<ConnectedAccount> ConnectedAccounts { get; set; }
    public virtual DbSet<Emoji> Emojis { get; set; }
    public virtual DbSet<Guild> Guilds { get; set; }
    public virtual DbSet<Invite> Invites { get; set; }
    public virtual DbSet<Member> Members { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<Note> Notes { get; set; }
    public virtual DbSet<RateLimit> RateLimits { get; set; }
    public virtual DbSet<ReadState> ReadStates { get; set; }
    public virtual DbSet<Recipient> Recipients { get; set; }
    public virtual DbSet<Relationship> Relationships { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Session> Sessions { get; set; }
    public virtual DbSet<Sticker> Stickers { get; set; }
    public virtual DbSet<StickerPack> StickerPacks { get; set; }
    public virtual DbSet<Team> Teams { get; set; }
    public virtual DbSet<TeamMember> TeamMembers { get; set; }
    public virtual DbSet<Template> Templates { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserSetting> UserSettings { get; set; }
    public virtual DbSet<ValidRegistrationToken> ValidRegistrationTokens { get; set; }
    public virtual DbSet<VoiceState> VoiceStates { get; set; }
    public virtual DbSet<Webhook> Webhooks { get; set; }

    //overrides
    public override void Dispose() {
        InUse = false;
        base.Dispose();
    }

    public override ValueTask DisposeAsync() {
        InUse = false;
        return base.DisposeAsync();
    }

    // internal static LogManager GetDbModelLogger()
    // {
    //     if (dbLogger == null)
    //     {
    //         dbLogger = new LogManager
    //         {
    //             Prefix = "[DbModel] ", LogTime = true
    //         };
    //         dbLogger.AddEndpoint(new DebugEndpoint());
    //     }
    //
    //     return dbLogger;
    // }

    // private static DbConfig cfg = Config.Instance.DbConfig;
    // public static string MySqlConnectionString =>
    // $"Data Source={cfg.Host};port={cfg.Port};Database={cfg.Database};User Id={cfg.Username};password={cfg.Password};charset=utf8;";
    // public static string PostgresConnectionString =>
    // $"Host={cfg.Host};Database={cfg.Database};Username={cfg.Username};Password={cfg.Password};Port={cfg.Port};Include Error Detail=true;Maximum Pool Size=1000";
    // public static string SqliteConnectionString => $"Data Source={cfg.Database}.db;Version=3;";

    // private static bool MigrationsExist(string provider)
    // {
    //     var provid = GetMigAsm(provider);
    //     log($"Checking if {provid} exists...");
    //     var res = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetTypes())
    //         .Any(x => x.Any(y => y.Namespace == provid));
    //     Console.WriteLine($"{provid} {(res ? "exists" : "doesn't exist")}!");
    //     return res;
    // }

    // public static string GetMigAsm(string provider)
    // {
    //     return "Spacebar.DbModel.Migrations." + provider switch
    //     {
    //         "postgres" => "Postgres",
    //         "mysql" => "Mysql",
    //         "mariadb" => "Mysql",
    //         "sqlite" => "Sqlite",
    //         "inmemory" => "InMemoryDb",
    //         _ => "null"
    //     };
    // }
}