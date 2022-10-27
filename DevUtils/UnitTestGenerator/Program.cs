// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using UnitTestGenerator;

if (Directory.Exists("output"))
    Directory.Delete("output", true);
Directory.CreateDirectory("output");

//run generators
DbInstantiationGenerator.GenerateTests();

