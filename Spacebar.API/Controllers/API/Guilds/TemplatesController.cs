using Spacebar.DbModel;
using Spacebar.DbModel.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Spacebar.API.Controllers.API.Guilds;

[Controller]
[Route("/")]
public class TemplatesController : Controller
{
    private readonly Db _db;

    public TemplatesController(Db db)
    {
        _db = db;
    }

    [HttpGet("guilds/templates/{code}")]
    public Template GetGuildTemplates(string code)
    {
        var template = _db.Templates.FirstOrDefault(x => x.Code == code);
        if (template == null) throw new Exception("Template not found");
        return template;
    }
}