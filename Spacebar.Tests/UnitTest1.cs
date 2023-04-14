#if false
using System;
using SpacebarSharp;
using SpacebarSharp.Entities;
using Xunit;

namespace Spacebar.Tests;

public class UnitTest1
{
    private static SpacebarClient? _cli = null;
    private static SpacebarClient GetClient()
    {
        if (_cli == null)
        {
            _cli = new SpacebarClient(new()
            {
                Email = $"SpacebarSharp{Environment.TickCount64}@unit.tests",
                // Email = $"SpacebarSharpDev1234@example.com",
                Password = "SpacebarSharp",
                Endpoint = "http://localhost:2000",
                Verbose = true,
                ShouldRegister = true,
                RegistrationOptions =
                {
                    Username = "SpacebarSharp Example Bot",
                    DateOfBirth = "1969-01-01",
                    CreateBotGuild = true
                },
                PollMessages = true
            });
            _cli.Login();
        }

        return _cli;
    }
    [Fact, Trait("type", "api")]
    public User GetCurrentUser()
    {
        return GetClient().GetCurrentUser().Result;
    }
    [Fact, Trait("type", "api")]
    public Guild[] GetGuildlist()
    {
        return GetClient().GetGuilds().Result;
    }
    [Fact, Trait("type", "api")]
    public Guild GetGuildById()
    {
        return GetClient().GetGuild(GetGuildlist()[0].Id).Result;
    }
    [Fact, Trait("type", "api")]
    public Channel[] GetChannels()
    {
        return GetGuildlist()[0].GetChannels().Result;
    }
}
#endif