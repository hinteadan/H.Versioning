using System;

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
            foreach (var provider in providers)
            {
                if (!provider.Item1.Invoke())
                {
                    continue;
                }

                try
                {
                    var v = provider.Item2.GetCurrent();
                    if (v == Version.Unknown)
                    {
                        continue;
                    }
                    return v;
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return Version.Unknown;
        }
    }
}
