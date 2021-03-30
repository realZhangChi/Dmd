using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading.Tasks;
using Dmd.Designer.Models.Solution;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Dmd.Designer.Services.Solution
{
    public class SolutionManager : ISolutionManager
    {
        private ICollection<FileModel> _directoryTree;
        ICollection<FileModel> ISolutionManager.DirectoryTree
        {
            get => _directoryTree;
            set => _directoryTree = value;
        }
        
        private FileModel _solution;
        FileModel ISolutionManager.Solution
        {
            get => _solution;
            set => _solution = value;
        }

        private List<FileModel> _projects;
        public IReadOnlyCollection<FileModel> Projects => _projects;
        
        public SolutionManager()
        {
            _solution = new FileModel();
            _directoryTree = new List<FileModel>();
            _projects = new List<FileModel>();
        }

        public async Task SetSolutionPathAsync(IJSRuntime jsRuntime, string fullPath)
        {
            var js = await GetJsObjectReferenceAsync(jsRuntime);
            // TODO: Validation
            _solution.FullPath = fullPath;

            var json = await js.InvokeAsync<string>("getSolutionTree", _solution.Directory);
            _directoryTree = JsonSerializer.Deserialize<ICollection<FileModel>>(json, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            await GetProjectAsync(_directoryTree);

        }

        public async Task<string> GetProjectDirectoryAsync(string fileOrDirectoryFullPath)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetNameSpaceAsync(string fileOrDirectoryFullPath)
        {
            throw new NotImplementedException();
        }

        private async Task GetProjectAsync(ICollection<FileModel> model)
        {
            if (model == null || model.Count == 0)
                return;

            var project = model.FirstOrDefault(
                f =>
                    f.FileType == FileType.File &&
                    Path.GetExtension(f.FullPath) == "csproj");
            if (project is not null)
            {
                _projects.Add(project);
            }

            foreach (var fileModel in model)
            {
                await GetProjectAsync(fileModel.Children);
            }
        }

        private ValueTask<IJSObjectReference> GetJsObjectReferenceAsync(IJSRuntime jsRuntime)
        {
            return jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./js/solution.js");
        }
    }
}
