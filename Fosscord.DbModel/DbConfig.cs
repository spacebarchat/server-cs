using ArcaneLibs;

namespace Fosscord.DbModel; 

public class DbConfig : SaveableObject<DbConfig> {
    public string Host { get; set; } = Environment.GetEnvironmentVariable("PG_HOST") ?? "localhost";
    public string Username { get; set; } = Environment.GetEnvironmentVariable("PG_USER") ?? "postgres";
    public string Password { get; set; } = Environment.GetEnvironmentVariable("PG_PASS") ?? "postgres";
    public string Database { get; set; } = Environment.GetEnvironmentVariable("PG_DB_FOSSCORD_CS") ?? "fosscord_cs";
    public short Port { get; set; } = short.Parse(Environment.GetEnvironmentVariable("PG_PORT") ?? "5432");
}