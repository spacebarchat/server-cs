using Fosscord.DbModel;
using Fosscord.Util;
using Microsoft.AspNetCore.Mvc;

namespace Fosscord.Gateway.Controllers;

public class DumpController : Controller
{
    private readonly ILogger<DumpController> _Logger;
    private readonly Db _db;

    public DumpController(ILogger<DumpController> logger, Db db)
    {
        _Logger = logger;
        _db = db;
    }

    [HttpPost("/dump/{*dir:required}")]
    public async Task Dump(string dir)
    {
        Console.WriteLine("Got data");
        string outdir = "dump/"+dir;
        if (!Directory.Exists("dump")) Directory.CreateDirectory("dump");
        if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);
        string outfile = outdir + "/" + Math.Floor(Directory.GetFiles(outdir).Length/2d)+".bin";
        var file = System.IO.File.OpenWrite(outfile);
        await Request.Body.CopyToAsync(file);
        file.Flush();
        file.Close();
        Console.WriteLine(outfile);
        try
        {
            System.IO.File.WriteAllBytes(outfile+".decompressed", ZLib.Decompress(System.IO.File.ReadAllBytes(outfile)));
            Console.WriteLine("Decompressed: " + outfile+".decompressed");
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to decompress: " + e);
        }
    }
}