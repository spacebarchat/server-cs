using Spacebar.DbModel;
using Spacebar.DbModel.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Spacebar.API.Controllers.API.Guilds;

[Controller]
[Route("/")]
public class TemplatesController(Db db) : Controller
{
    [HttpGet("guilds/templates/{code}")]
    public Template GetGuildTemplates(string code)
    {
        var template = db.Templates.FirstOrDefault(x => x.Code == code);
        if (template == null) throw new Exception("Template not found");
        return template;
    }
}