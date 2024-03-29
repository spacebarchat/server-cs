﻿using Spacebar.DbModel;
using Spacebar.Util;
using Microsoft.AspNetCore.Mvc;

namespace Spacebar.Gateway.Controllers;

public class DumpController : Controller
{
    private readonly ILogger<DumpController> _logger;
    private readonly Db _db;

    public DumpController(ILogger<DumpController> logger, Db db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpPost("/dump/{*dir:required}")]
    public async Task Dump(string dir)
    {
        Console.WriteLine("Got data");
        var outdir = "dump/" + dir;
        if (!Directory.Exists("dump")) Directory.CreateDirectory("dump");
        if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);
        var outfile = outdir + "/" + Directory.GetFiles(outdir).Count(x => x.EndsWith(".bin")) + ".bin";
        var file = System.IO.File.OpenWrite(outfile);
        await Request.Body.CopyToAsync(file);
        file.Flush();
        file.Close();
        Console.WriteLine(outfile);
        try
        {
            System.IO.File.WriteAllBytes(outfile + ".decompressed",
                ZLib.Decompress(System.IO.File.ReadAllBytes(outfile)));
            Console.WriteLine("Decompressed: " + outfile + ".decompressed");
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to decompress: " + e);
        }
    }
}