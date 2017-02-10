using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.Versioning.VersionNumberParsers
{
    public interface ICanParseVersionNumber
    {
        VersionNumber Parse(string versionNumber);
    }
}
