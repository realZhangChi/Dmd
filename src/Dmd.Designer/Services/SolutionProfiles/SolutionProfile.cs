using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dmd.Designer.Services.SolutionProfiles
{
    public class SolutionProfile
    {
        public string FullPath { get; init; }

        public string FullName => Path.GetFileName(FullPath);

        public string Name => Path.GetFileNameWithoutExtension(FullPath);

        public string Dictionary => Path.GetDirectoryName(FullPath);
    }
}
