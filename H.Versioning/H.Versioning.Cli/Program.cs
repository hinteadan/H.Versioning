﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace H.Versioning.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Any())
            {
                HandleArgs(args);
                return;
            }

            Console.WriteLine(Version.Self.GetCurrent());
        }

        private static void HandleArgs(string[] args)
        {
            switch (args[0].ToLowerInvariant())
            {
                case "nuspec": HandleUpdateNuspecFileVersion(args.Skip(1)); return;
                default: HandleUnknowArgument(args[0]); return;
            }
        }

        private static void HandleUpdateNuspecFileVersion(IEnumerable<string> args)
        {
            if (!args.Any())
            {
                Console.WriteLine("You must specify the *.nuspec file path. E.g.: H.Versioning.Cli.exe nuspec \".\\MyProject\\MyProject.nuspec\"");
                return;
            }

            try
            {
                new NuspecFile(args.First()).UpdateVersion();
                Console.WriteLine("Nuspec file version successfully updated");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void HandleUnknowArgument(string arg)
        {
            Console.WriteLine($"Unknown command argument: {arg}");
        }
    }
}
