using System;
using System.Threading.Tasks;
using Fosscord.API.Controllers.API.Auth;
using Fosscord.DbModel;
using Fosscord.Util;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Fosscord.Tests.APITests;

public class RegisterTests
{
    [Fact]
    public async Task Register()
    {
        //test asp controller
        var controller = new AuthController(Db.GetInMemoryDb(), new JwtAuthenticationManager());
        var data = await controller.Register(new()
        {
            Consent = true,
            Email = "test",
            Password = "test",
            Username = "test",
            DateOfBirth = new DateTime().ToString("O")
        });
        Assert.NotNull(data);
        Assert.IsNotType<StatusCodeResult>(data);
    }
}