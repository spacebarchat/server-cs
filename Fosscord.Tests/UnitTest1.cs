using System;
using FosscordSharp;
using FosscordSharp.Entities;
using Xunit;

namespace Fosscord.Tests;

public class UnitTest1
{
    private static FosscordClient? _cli = null;
    private static FosscordClient GetClient()
    {
        if (_cli == null)
        {
            _cli = new FosscordClient(new()
            {
                Email = $"FosscordSharp{Environment.TickCount64}@unit.tests",
                // Email = $"FosscordSharpDev1234@example.com",
                Password = "FosscordSharp",
                Endpoint = "http://localhost:2000",
                Verbose = true,
                ShouldRegister = true,
                RegistrationOptions =
                {
                    Username = "FosscordSharp Example Bot",
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