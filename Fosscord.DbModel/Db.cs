﻿using System.ComponentModel.DataAnnotations.Schema;
using ArcaneLibs.Logging;
using ArcaneLibs.Logging.LogEndpoints;
using Fosscord.ConfigModel;
using Fosscord.DbModel.Entities;
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

    public virtual DbSet<Application> Applications { get; }
    public virtual DbSet<Attachment> Attachments { get; }
    public virtual DbSet<AuditLog> AuditLogs { get; }
    public virtual DbSet<BackupCode> BackupCodes { get; }
    public virtual DbSet<Ban> Bans { get; }
    public virtual DbSet<Category> Categories { get; }
    public virtual DbSet<Channel> Channels { get; }
    public virtual DbSet<ClientRelease> ClientReleases { get; }
    public virtual DbSet<ConnectedAccount> ConnectedAccounts { get; }
    public virtual DbSet<Emoji> Emojis { get; }
    public virtual DbSet<Guild> Guilds { get; }
    public virtual DbSet<Invite> Invites { get; }
    public virtual DbSet<Member> Members { get; }
    public virtual DbSet<Message> Messages { get; }
    public virtual DbSet<Note> Notes { get; }
    public virtual DbSet<RateLimit> RateLimits { get; }
    public virtual DbSet<ReadState> ReadStates { get; }
    public virtual DbSet<Recipient> Recipients { get; }
    public virtual DbSet<Relationship> Relationships { get; }
    public virtual DbSet<Role> Roles { get; }
    public virtual DbSet<Session> Sessions { get; }
    public virtual DbSet<Sticker> Stickers { get; }
    public virtual DbSet<StickerPack> StickerPacks { get; }
    public virtual DbSet<Team> Teams { get; }
    public virtual DbSet<TeamMember> TeamMembers { get; }
    public virtual DbSet<Template> Templates { get; }
    public virtual DbSet<User> Users { get; }
    public virtual DbSet<UserSetting> UserSettings { get; }
    public virtual DbSet<ValidRegistrationToken> ValidRegistrationTokens { get; }
    public virtual DbSet<VoiceState> VoiceStates { get; }
    public virtual DbSet<Webhook> Webhooks { get; }

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

    public static Db GetNewMysql(DbConfig cfg)
    {
        GetDbModelLogger().Log("Instantiating new DB context: MySQL/MariaDB");
        string ds =
            $"Data Source={cfg.Host};port={cfg.Port};Database={cfg.Database};User Id={cfg.Username};password={cfg.Password};charset=utf8;";
        var db = new Db(new DbContextOptionsBuilder<Db>().UseMySql(ds, ServerVersion.AutoDetect(ds)
            )
            .LogTo(log, LogLevel.Information).EnableSensitiveDataLogging().Options);
        return db;
    }

    public static Db GetNewPostgres(DbConfig cfg)
    {
        GetDbModelLogger().Log("Instantiating new DB context: Postgres");
        var db = new Db(new DbContextOptionsBuilder<Db>()
            .UseNpgsql(
                $"Host={cfg.Host};Database={cfg.Database};Username={cfg.Username};Password={cfg.Password};Port={cfg.Port};Include Error Detail=true"
            )
            .LogTo(log, LogLevel.Information).EnableSensitiveDataLogging().Options);
        return db;
    }

    public static Db GetSqlite(DbConfig cfg)
    {
        GetDbModelLogger().Log("Instantiating new DB context: Sqlite");
        return new Db(new DbContextOptionsBuilder<Db>()
            .UseSqlite($"Data Source={cfg.Database}.db;Version=3;",
                x =>
                {
                    if (MigrationsExist(cfg.Driver)) x.MigrationsAssembly(GetMigAsm(cfg.Driver));
                })
            .LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging().Options);
    }

    public static Db GetInMemoryDb(DbConfig? cfg = null)
    {
        cfg ??= new DbConfig() {Database = "FosscordInMemory", Driver = "InMemory"};
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

    public static string GetMigAsm(string provider) => "Fosscord.DbModel.Migrations." + provider switch
    {
        "postgres" => "Postgres",
        "mysql" => "Mysql",
        "mariadb" => "Mysql",
        "sqlite" => "Sqlite",
        "inmemory" => "InMemoryDb",
        _ => "null"
    };
}