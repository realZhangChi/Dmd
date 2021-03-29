using System.IO;

namespace Dmd.Designer.Services.SolutionProfiles
{
    public class ProjectProfile
    {
        public string FullPath { get; set; }

        public string FullName => Path.GetFileName(FullPath);

        public string Name => Path.GetFileNameWithoutExtension(FullPath);

        public string Dictionary => Path.GetDirectoryName(FullPath);
    }
}
