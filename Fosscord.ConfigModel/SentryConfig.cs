namespace Fosscord.ConfigModel;

public class SentryConfig
{
    public string Dsn = "https://b2bf2393e4f64336af713ed9b06f0a9a@sentry.thearcanebrony.net/8";
    public bool Enabled = true;
    public string Environment = System.Environment.MachineName;
}