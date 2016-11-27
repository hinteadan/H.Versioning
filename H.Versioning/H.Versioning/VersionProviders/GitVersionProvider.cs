﻿using LibGit2Sharp;
using System;
using System.IO;
using System.Linq;

namespace H.Versioning.VersionProviders
{
    public sealed class GitVersionProvider : IProvideVersion
    {
        private readonly string gitRepositoryPath;

        public GitVersionProvider(string pathToGitRepo)
        {
            if (string.IsNullOrWhiteSpace(pathToGitRepo))
            {
                throw new InvalidOperationException("The git repository path cannot be empty");
            }

            this.gitRepositoryPath = FindGitFolder(new DirectoryInfo(pathToGitRepo));
        }

        private string FindGitFolder(DirectoryInfo folder)
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

        public Version GetCurrent()
        {
            using (var repo = new Repository(gitRepositoryPath))
            {
                var branch = repo.Branches.Single(b => b.IsCurrentRepositoryHead);

                return new Version(FetchVersionNumber(repo), branch.Tip.Committer.When.DateTime, branch.FriendlyName, branch.Tip.Sha);
            }
        }

        private VersionNumber FetchVersionNumber(Repository repo)
        {
            var branch = repo.Branches.Single(b => b.IsCurrentRepositoryHead);
            var tag = FetchClosestTag(repo, branch.Tip.Committer.When);

            if (tag == null)
            {
                return NeverReleasedDevelopmentVersionNumber(repo, branch.Tip.Committer.When);
            }

            var tagVersionNumber = VersionNumber.Parse(tag.Annotation.Name);
            if (branch.Tip.Id == tag.Target.Id)
            {
                return tagVersionNumber;
            }

            return new VersionNumber(
                tagVersionNumber.Major,
                tagVersionNumber.Minor,
                tagVersionNumber.Patch ?? 0,
                FetchBuildNumber(repo, branch.Tip.Committer.When, TagCommit(repo, tag).Committer.When),
                InDevelopmentSuffix(tagVersionNumber.Suffix)
                );
        }

        private int FetchBuildNumber(Repository repo, DateTimeOffset timestamp, DateTimeOffset tagTimestamp)
        {
            return repo
                .Commits
                .Where(c => c.Committer.When > tagTimestamp && c.Committer.When <= timestamp)
                .Count();
        }

        private VersionNumber NeverReleasedDevelopmentVersionNumber(Repository repo, DateTimeOffset timestamp)
        {
            return new VersionNumber(0, 0, 0, repo.Commits.Where(c => c.Committer.When <= timestamp).Count(), InDevelopmentSuffix());
        }

        private string InDevelopmentSuffix(string existingSuffix = null)
        {
            return string.IsNullOrWhiteSpace(existingSuffix) ? "in-development" : $"{existingSuffix}-in-development";
        }

        private Tag FetchClosestTag(Repository repo, DateTimeOffset timestamp)
        {
            return repo.Tags
                .Where(t => t.IsAnnotated)
                .OrderByDescending(t => t.Annotation.Tagger.When)
                .FirstOrDefault(t => TagCommit(repo, t).Committer.When <= timestamp);
        }

        private Commit TagCommit(Repository repo, Tag tag)
        {
            return repo.Commits.Single(c => c.Id == tag.Target.Id);
        }
    }
}
