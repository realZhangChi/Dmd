using System.IO;

namespace Dmd.Designer.Services.Solution
{
    public class ProjectProfile
    {
        public string FullPath { get; set; }

        public string FullName => Path.GetFileName(FullPath);

        public string Name => Path.GetFileNameWithoutExtension(FullPath);

        public string Directory => Path.GetDirectoryName(FullPath);

        protected internal ProjectProfile()
        {

        }
    }
}
