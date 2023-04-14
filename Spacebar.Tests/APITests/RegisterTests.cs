using System;
using System.Threading.Tasks;
using Spacebar.API.Controllers.API.Auth;
using Spacebar.DbModel;
using Spacebar.Util;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Spacebar.Tests.APITests;

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