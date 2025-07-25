using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Spacebar.ConfigModel;
using Spacebar.Util;

namespace Spacebar.API.Controllers.API.Auth;

//[Authorize]
[Controller]
[Route("/api/_spacebar/v1/")]
public class InfoController(Db db, JwtAuthenticationManager auth, Config config) : Controller {
    private static readonly Random Rnd = new();
    private readonly JwtAuthenticationManager _auth = auth;
    private readonly Db _db = db;

    /// <summary>
    ///     Get global environment vars for test client
    /// </summary>
    /// <returns>Json object with GLOBAL_ENV</returns>
    [HttpGet("global_env")]
    public async Task<object> GetGlobalEnv() =>
        new JsonResult(new {
            //api version
            API_VERSION = 9,
            //endpoints
            GATEWAY_ENDPOINT = config.Endpoints.Gateway,
            API_ENDPOINT = config.Endpoints.Api,
            CDN_HOST = config.Endpoints.Cdn,
            WEBAPP_ENDPOINT = "",
            ASSET_ENDPOINT = "",
            MEDIA_PROXY_ENDPOINT = config.Endpoints.Cdn,
            WIDGET_ENDPOINT = $"{config.Endpoints.Api}/widget",
            INVITE_HOST = $"{config.Endpoints.Api}/invite",
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

            SENTRY_TAGS = new { buildId = "75e36d9", buildType = "normal" },
            HTML_TIMESTAMP = DateTime.Now.Ticks,
            //keys
            ALGOLIA_KEY = "aca0d7082e4e63af5ba5917d5e96bed0",
            BRAINTREE_KEY = "production_5st77rrc_49pp2rp4phym7387",
            STRIPE_KEY = "pk_live_CUQtlpQUF0vufWpnpUmQvcdi"
        }, new JsonSerializerOptions {
            PropertyNamingPolicy = new OriginalNamingPolicy()
        });
}

public class OriginalNamingPolicy : JsonNamingPolicy {
    public override string ConvertName(string name) => name;
}