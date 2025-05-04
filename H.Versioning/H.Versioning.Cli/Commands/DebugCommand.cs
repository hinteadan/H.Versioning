using H.Necessaire;
using H.Necessaire.CLI.Commands;

namespace H.Versioning.Cli.Commands
{
    internal class DebugCommand : CommandBase
    {
        public override Task<OperationResult> Run()
        {
            Version? version = Version.Self.GetCurrent();

            return OperationResult.Win().AsTask();
        }
    }
}
