using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace H.Versioning.VersionProviders
{
    internal sealed class GitReleaseVersionsProvider : IProvideReleaseVersions
    {
        public IEnumerable<ReleaseVersion> GetAllReleasesFor(string gitFolderPath)
        {
            if (string.IsNullOrWhiteSpace(gitFolderPath))
                yield break;

            if (!File.GetAttributes(gitFolderPath).HasFlag(FileAttributes.Directory))
                gitFolderPath = Path.GetDirectoryName(gitFolderPath);

            gitFolderPath = FindGitFolder(new DirectoryInfo(gitFolderPath));

            if (string.IsNullOrWhiteSpace(gitFolderPath))
                yield break;

            using (Repository gitRepo = new Repository(gitFolderPath))
            {
                if (gitRepo.Tags?.Any() != true)
                    yield break;

                foreach (Tag tag in gitRepo.Tags)
                {
                    string versionString = tag.Annotation?.Name;

                    if (string.IsNullOrWhiteSpace(versionString))
                        versionString = tag.FriendlyName;

                    if (string.IsNullOrWhiteSpace(versionString))
                        continue;

                    VersionNumber versionNumber;
                    try
                    {
                        versionNumber = VersionNumber.Parse(versionString);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    yield return BuildReleaseVersion(tag, versionNumber, versionString);
                }
            }
        }

        ReleaseVersion BuildReleaseVersion(Tag tag, VersionNumber versionNumber, string versionString)
        {
            Commit commit = tag.Target as Commit;

            string shortMess = tag.Annotation?.Message?.Replace(versionString, "").Trim();
            if (string.IsNullOrWhiteSpace(shortMess)) shortMess = null;
            string longMess = commit?.Message?.Replace(versionString, "").Trim();
            if (string.IsNullOrWhiteSpace(longMess)) longMess = null;

            if ((shortMess?.Length ?? 0) > (longMess?.Length ?? 0))
            {
                string tmp = longMess;
                longMess = shortMess;
                shortMess = tmp;
            }

            string mess = longMess;
            string commitSha = tag.Target.Sha;
            var author = tag.Annotation?.Tagger ?? commit?.Author ?? commit?.Committer;
            DateTime asOf = author.When.UtcDateTime;
            string by = string.IsNullOrWhiteSpace(author.Name) ? author.Email : author.Name;

            return
                new ReleaseVersion
                {
                    ID = commitSha,
                    Name = versionNumber.ToString(),
                    Description = mess,
                    Notes = [
                        new KeyValuePair<string, string>(key: "Excerpt", value: !string.IsNullOrWhiteSpace(shortMess) ? shortMess : string.IsNullOrWhiteSpace(commit?.MessageShort) ? null : commit.MessageShort),
                        new KeyValuePair<string, string>(key: "ShortMess", value: shortMess),
                        new KeyValuePair<string, string>(key: "LongMess", value: longMess),
                        new KeyValuePair<string, string>(key: "Tag.Annotation", value: !string.IsNullOrWhiteSpace(tag.Annotation?.Message) ? tag.Annotation.Message : null),
                        new KeyValuePair<string, string>(key: "Commit.Message", value: !string.IsNullOrWhiteSpace(commit?.Message) ? commit?.Message : null),
                        new KeyValuePair<string, string>(key: "Commit.MessageShort", value: !string.IsNullOrWhiteSpace(commit?.MessageShort) ? commit?.MessageShort : null),
                    ],
                    Version = new Version(versionNumber, asOf, "master", commitSha),
                };
        }

        static string FindGitFolder(DirectoryInfo folder)
        {
            if (folder == null || !folder.Exists)
            {
                return string.Empty;
            }

            if (folder.GetDirectories(".git").Any())
            {
                return folder.FullName;
            }

            return FindGitFolder(folder.Parent);
        }
    }
}
