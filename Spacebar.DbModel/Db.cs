using System.ComponentModel.DataAnnotations.Schema;
using ArcaneLibs.Logging;
using ArcaneLibs.Logging.LogEndpoints;
using Spacebar.ConfigModel;
using Spacebar.DbModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Spacebar.DbModel;

public class Db : DbContext
{
    //props
    public bool InUse = true;

    //db
    public Db(DbContextOptions<Db> options)
        : base(options)
    {
    }

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
    public override void Dispose()
    {
        InUse = false;
        base.Dispose();
    }

    public override ValueTask DisposeAsync()
    {
        InUse = false;
        return base.DisposeAsync();
    }

    [NotMapped] private static LogManager? dbLogger;
    [NotMapped] private static List<Db> contexts = new();

    internal static LogManager GetDbModelLogger()
    {
        if (dbLogger == null)
        {
            dbLogger = new LogManager
            {
                Prefix = "[DbModel] ", LogTime = true
            };
            dbLogger.AddEndpoint(new DebugEndpoint());
        }

        return dbLogger;
    }

    [Obsolete("Don't use these!", true)]
    public static Db GetNewDb(DbConfig? cfg = null)
    {
        cfg ??= Config.Instance.DbConfig;
        var db = cfg.Driver.ToLower() switch
        {
            "postgres" => GetNewPostgres(cfg),
            "mysql" => GetNewMysql(cfg),
            "mariadb" => GetNewMysql(cfg),
            "sqlite" => GetSqlite(cfg),
            "inmemory" => GetInMemoryDb(cfg),
            _ => throw new Exception($"Invalid database driver: {cfg.Driver}")
        };

        db.Database.Migrate();
        db.SaveChanges();
        contexts.Add(db);
        return db;
    }

    private static DbConfig cfg = Config.Instance.DbConfig;
    public static string MySqlConnectionString =>
        $"Data Source={cfg.Host};port={cfg.Port};Database={cfg.Database};User Id={cfg.Username};password={cfg.Password};charset=utf8;";
    public static string PostgresConnectionString =>
        $"Host={cfg.Host};Database={cfg.Database};Username={cfg.Username};Password={cfg.Password};Port={cfg.Port};Include Error Detail=true;Maximum Pool Size=1000";
    public static string SqliteConnectionString => $"Data Source={cfg.Database}.db;Version=3;";
    
    
    [Obsolete("Don't use these!", true)]
    public static Db GetNewMysql(DbConfig cfg)
    {
        GetDbModelLogger().Log("Instantiating new DB context: MySQL/MariaDB");
        var db = new Db(new DbContextOptionsBuilder<Db>()
            .UseMySql(MySqlConnectionString, ServerVersion.AutoDetect(MySqlConnectionString))
            .LogTo(log, LogLevel.Information).EnableSensitiveDataLogging().Options);
        return db;
    }

    [Obsolete("Don't use these!", true)]
    public static Db GetNewPostgres(DbConfig cfg)
    {
        GetDbModelLogger().Log("Instantiating new DB context: Postgres");
        var db = new Db(new DbContextOptionsBuilder<Db>()
            .UseNpgsql(PostgresConnectionString)
            .LogTo(log, LogLevel.Information).EnableSensitiveDataLogging().Options);
        return db;
    }

    [Obsolete("Don't use these!", true)]
    public static Db GetSqlite(DbConfig cfg)
    {
        GetDbModelLogger().Log("Instantiating new DB context: Sqlite");
        return new Db(new DbContextOptionsBuilder<Db>()
            .UseSqlite(SqliteConnectionString,
                x =>
                {
                    if (MigrationsExist(cfg.Driver)) x.MigrationsAssembly(GetMigAsm(cfg.Driver));
                })
            .LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging().Options);
    }

    [Obsolete("Don't use these!", true)]
    public static Db GetInMemoryDb(DbConfig? cfg = null)
    {
        cfg ??= new DbConfig() { Database = "SpacebarInMemory", Driver = "InMemory" };
        GetDbModelLogger().Log("Instantiating new DB context: InMemory");
        return new Db(new DbContextOptionsBuilder<Db>().UseInMemoryDatabase(cfg.Database)
            .LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging().Options);
    }

    private static void log(string text)
    {
        GetDbModelLogger().Log(text);
    }

    private static bool MigrationsExist(string provider)
    {
        var provid = GetMigAsm(provider);
        log($"Checking if {provid} exists...");
        var res = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetTypes())
            .Any(x => x.Any(y => y.Namespace == provid));
        Console.WriteLine($"{provid} {(res ? "exists" : "doesn't exist")}!");
        return res;
    }

    public static string GetMigAsm(string provider)
    {
        return "Spacebar.DbModel.Migrations." + provider switch
        {
            "postgres" => "Postgres",
            "mysql" => "Mysql",
            "mariadb" => "Mysql",
            "sqlite" => "Sqlite",
            "inmemory" => "InMemoryDb",
            _ => "null"
        };
    }
}