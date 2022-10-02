using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using ArcaneLibs.Logging;
using ArcaneLibs.Logging.LogEndpoints;
using Fosscord.DbModel.Scaffold;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fosscord.DbModel;

public class Db : DbContext
{
    //props
    public bool InUse = true;

    //db
    public Db(DbContextOptions<Db> options)
        : base(options)
    {
    }

        public virtual DbSet<Application> Applications { get; set; } = null!;
        public virtual DbSet<Attachment> Attachments { get; set; } = null!;
        public virtual DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public virtual DbSet<BackupCode> BackupCodes { get; set; } = null!;
        public virtual DbSet<Ban> Bans { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Channel> Channels { get; set; } = null!;
        public virtual DbSet<ClientRelease> ClientReleases { get; set; } = null!;
        public virtual DbSet<ConnectedAccount> ConnectedAccounts { get; set; } = null!;
        public virtual DbSet<Emoji> Emojis { get; set; } = null!;
        public virtual DbSet<Guild> Guilds { get; set; } = null!;
        public virtual DbSet<Invite> Invites { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Migration> Migrations { get; set; } = null!;
        public virtual DbSet<Note> Notes { get; set; } = null!;
        public virtual DbSet<QueryResultCache> QueryResultCaches { get; set; } = null!;
        public virtual DbSet<RateLimit> RateLimits { get; set; } = null!;
        public virtual DbSet<ReadState> ReadStates { get; set; } = null!;
        public virtual DbSet<Recipient> Recipients { get; set; } = null!;
        public virtual DbSet<Relationship> Relationships { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Session> Sessions { get; set; } = null!;
        public virtual DbSet<Sticker> Stickers { get; set; } = null!;
        public virtual DbSet<StickerPack> StickerPacks { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        public virtual DbSet<TeamMember> TeamMembers { get; set; } = null!;
        public virtual DbSet<Template> Templates { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserSetting> UserSettings { get; set; } = null!;
        public virtual DbSet<ValidRegistrationToken> ValidRegistrationTokens { get; set; } = null!;
        public virtual DbSet<VoiceState> VoiceStates { get; set; } = null!;
        public virtual DbSet<Webhook> Webhooks { get; set; } = null!;

//         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             if (!optionsBuilder.IsConfigured) {
//                 var cfg = DbConfig.Read();
//                 cfg.Save();
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                 optionsBuilder.UseLazyLoadingProxies().UseNpgsql($"Host={cfg.Host};Database={cfg.Database};Username={cfg.Username};Password={cfg.Password};Port={cfg.Port}").LogTo((str)=>Debug.WriteLine(str), LogLevel.Information).EnableSensitiveDataLogging();
//             }
//         }

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


    public static Db GetDb()
    {
        GetDbModelLogger().Log("Instantiating new DB context: auto");
        if (contexts.Any(x => !x.ChangeTracker.HasChanges())) return contexts.First(x => !x.ChangeTracker.HasChanges());
        var db = GetNewDb();
        contexts.Add(db);
        return db;
    }

    public static Db GetNewDb()
    {
        var cfg = DbConfig.Read();
        cfg.Save();
        return cfg.Driver.ToLower() switch
        {
            "postgres" => GetNewPostgres(),
            "mysql" => GetNewMysql(),
            "mariadb" => GetNewMysql(),
            "sqlite" => GetSqlite(),
            "inmemory" => GetInMemoryDb(),
            _ => LogFailAndExit($"No such database engine supported: {cfg.Driver}")
        };
    }

    public static Db GetNewMysql()
    {
        GetDbModelLogger().Log("Instantiating new DB context: MariDB");
        var cfg = DbConfig.Read();
        cfg.Save();
        string ds =
            $"Data Source={cfg.Host};port={cfg.Port};Database={cfg.Database};User Id={cfg.Username};password={cfg.Password};charset=utf8;";
        var db = new Db(new DbContextOptionsBuilder<Db>().UseMySql(ds, ServerVersion.AutoDetect(ds)
                // ,
                // x =>
                // {
                //     if (MigrationsExist(cfg.Driver)) x.MigrationsAssembly(GetMigAsm(cfg.Driver));
                // }
            )
            .LogTo(log, LogLevel.Information).EnableSensitiveDataLogging().Options);
        contexts.Add(db);
        db.Database.Migrate();
        db.SaveChanges();
        return db;
    }

    public static Db GetNewPostgres()
    {
        GetDbModelLogger().Log("Instantiating new DB context: Postgres");
        var cfg = DbConfig.Read();
        cfg.Save();
        var db = new Db(new DbContextOptionsBuilder<Db>()
            .UseNpgsql(
                $"Host={cfg.Host};Database={cfg.Database};Username={cfg.Username};Password={cfg.Password};Port={cfg.Port};Include Error Detail=true"
                // ,
                // x =>
                // {
                //     if (MigrationsExist(cfg.Driver)) x.MigrationsAssembly(GetMigAsm(cfg.Driver));
                // }
            )
            .LogTo(log, LogLevel.Information).EnableSensitiveDataLogging().Options);
        contexts.Add(db);
        // if (MigrationsExist(cfg.Driver))
        // {
        db.Database.Migrate();
        db.SaveChanges();
        // }

        return db;
    }

    public static Db GetSqlite()
    {
        GetDbModelLogger().Log("Instantiating new DB context: Sqlite");
        var cfg = DbConfig.Read();
        cfg.Save();
        return new Db(new DbContextOptionsBuilder<Db>()
            .UseSqlite($"Data Source={cfg.Database}.db;Version=3;",
                x =>
                {
                    if (MigrationsExist(cfg.Driver)) x.MigrationsAssembly(GetMigAsm(cfg.Driver));
                })
            .LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging().Options);
    }

    public static Db GetInMemoryDb()
    {
        GetDbModelLogger().Log("Instantiating new DB context: InMemory");
        var cfg = DbConfig.Read();
        cfg.Save();
        return new Db(new DbContextOptionsBuilder<Db>().UseInMemoryDatabase("InMemoryDb")
            .LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging().Options);
    }

    public static Db LogFailAndExit(string error = "")
    {
        log(error);
        Environment.Exit(1);
        return null;
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

    private static string GetMigAsm(string provider) => "Fosscord.DbModel.Migrations." + provider switch
    {
        "postgres" => "Postgres",
        "mysql" => "Mysql",
        "mariadb" => "Mysql",
        "sqlite" => "Sqlite",
        "inmemory" => "InMemoryDb",
        _ => "null"
    };
}