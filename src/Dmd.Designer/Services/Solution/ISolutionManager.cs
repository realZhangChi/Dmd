using System.Collections.Generic;
using System.Threading.Tasks;
using Dmd.Designer.Models.Solution;
using Microsoft.JSInterop;

namespace Dmd.Designer.Services.Solution
{
    public interface ISolutionManager
    {
        public ICollection<FileModel> DirectoryTree { get; protected set; }

        public FileModel Solution { get; protected set; }

        Task SetSolutionPathAsync(IJSRuntime jsRuntime, string fullPath);

        Task<string> GetProjectDirectoryAsync(string path);

        Task<string> GetNameSpaceAsync(string path);
    }
}
