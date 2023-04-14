using System.Text.Json;
using Spacebar.ConfigModel;
using Spacebar.DbModel;
using Spacebar.Util;
using Microsoft.AspNetCore.Mvc;

namespace Spacebar.API.Controllers.API.Auth;

//[Authorize]
[Controller]
[Route("/api/_fosscord/v1/")]
public class InfoController : Controller
{
    private readonly Db _db;
    private readonly JwtAuthenticationManager _auth;
    private static readonly Random Rnd = new();

    public InfoController(Db db, JwtAuthenticationManager auth)
    {
        _db = db;
        _auth = auth;
    }

    /// <summary>
    /// Get global environment vars for test client
    /// </summary>
    /// <returns>Json object with GLOBAL_ENV</returns>
    [HttpGet("global_env")]
    public async Task<object> GetGlobalEnv()
    {
        return new JsonResult(new
        {
            //api version
            API_VERSION = 9,
            //endpoints
            GATEWAY_ENDPOINT = Config.Instance.Endpoints.Gateway,
            API_ENDPOINT = Config.Instance.Endpoints.Api,
            CDN_HOST = Config.Instance.Endpoints.Cdn,
            WEBAPP_ENDPOINT = "",
            ASSET_ENDPOINT = "",
            MEDIA_PROXY_ENDPOINT = Config.Instance.Endpoints.Cdn,
            WIDGET_ENDPOINT = $"{Config.Instance.Endpoints.Api}/widget",
            INVITE_HOST = $"{Config.Instance.Endpoints.Api}/invite",
            GUILD_TEMPLATE_HOST = "discord.new",
            GIFT_CODE_HOST = "discord.gift",
            MARKETING_ENDPOINT = "//discord.com",
            NETWORKING_ENDPOINT = "//router.discordapp.net",
            RTC_LATENCY_ENDPOINT = "//latency.discord.media/rtc",
            ACTIVITY_APPLICATION_HOST = "discordsays.com",
            REMOTE_AUTH_ENDPOINT = "//localhost:3020",
            MIGRATION_SOURCE_ORIGIN = "https://discordapp.com",
            MIGRATION_DESTINATION_ORIGIN = "https://discord.com",
            //release channel, build info
            RELEASE_CHANNEL = "staging",
            DISCORD_TEST = true,
            PROJECT_ENV = "staging",

            SENTRY_TAGS = new {buildId = "75e36d9", buildType = "normal"},
            HTML_TIMESTAMP = DateTime.Now.Ticks,
            //keys
            ALGOLIA_KEY = "aca0d7082e4e63af5ba5917d5e96bed0",
            BRAINTREE_KEY = "production_5st77rrc_49pp2rp4phym7387",
            STRIPE_KEY = "pk_live_CUQtlpQUF0vufWpnpUmQvcdi",
        }, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = new OriginalNamingPolicy()
        });
    }
}

public class OriginalNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) => name;
}