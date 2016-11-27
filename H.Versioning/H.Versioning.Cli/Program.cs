using System;

namespace H.Versioning.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Version.Self.GetCurrent());
        }
    }
}
