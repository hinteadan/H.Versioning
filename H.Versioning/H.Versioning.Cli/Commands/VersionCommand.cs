using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;

namespace H.Versioning.Cli.Commands
{
    internal class VersionCommand : CommandBase
    {
        public override Task<OperationResult> Run()
        {
            Console.WriteLine(Version.Self.GetCurrent().ToString());

            return OperationResult.Win().AsTask();
        }
    }
}
