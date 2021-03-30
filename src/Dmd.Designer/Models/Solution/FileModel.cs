using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace Dmd.Designer.Models.Solution
{
    public class FileModel
    {
        public string FullPath { get; set; }

        public FileType FileType { get; set; }

        [JsonIgnore]
        public string FullName => Path.GetFileName(FullPath);

        [JsonIgnore]
        public string Name => Path.GetFileNameWithoutExtension(FullPath);

        [JsonIgnore]
        public string Directory => Path.GetDirectoryName(FullPath);

        public ICollection<FileModel> Children { get; set; }
    }
}
