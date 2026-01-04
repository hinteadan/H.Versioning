using System.Collections.Generic;

namespace H.Versioning
{
    public interface IProvideReleaseVersions
    {
        IEnumerable<ReleaseVersion> GetAllReleasesFor(string gitFolderPath);
    }
}
