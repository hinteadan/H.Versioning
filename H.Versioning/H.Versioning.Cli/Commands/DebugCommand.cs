using H.Necessaire;
using H.Necessaire.CLI.Commands;
using System.Reflection;

namespace H.Versioning.Cli.Commands
{
    internal class DebugCommand : CommandBase
    {
        public override Task<OperationResult> Run()
        {
            //Version? version = Version.Self.GetCurrent();

            var releases = ReleaseVersion.ProviderForGit.GetAllReleasesFor(Assembly.GetEntryAssembly()?.Location).ToArray();

            return OperationResult.Win().AsTask();
        }
    }
}
