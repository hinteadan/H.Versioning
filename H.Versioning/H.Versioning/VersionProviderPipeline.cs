using System;
using System.Linq;

namespace H.Versioning
{
    public abstract class VersionProviderPipeline : IProvideVersion
    {
        private readonly Tuple<Func<bool>, IProvideVersion>[] providers;

        public VersionProviderPipeline(params Tuple<Func<bool>, IProvideVersion>[] providers)
        {
            this.providers = providers;
        }

        public Version GetCurrent()
        {
            return this.providers
                .Where(x => x.Item1.Invoke())
                .FirstOrDefault()
                ?.Item2.GetCurrent()
                ?? Version.Unknown;
        }
    }
}
