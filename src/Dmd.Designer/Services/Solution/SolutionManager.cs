using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using Dmd.Designer.Models.Solution;
using Dmd.Designer.Services.File;
using Microsoft.Extensions.DependencyInjection;
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

        private readonly List<ProjectModel> _projects;
        public IReadOnlyCollection<ProjectModel> Projects => _projects;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;

        public SolutionManager(
            IServiceScopeFactory scopeFactory,
            ILogger<SolutionManager> logger)
        {
            _solution = new FileModel();
            _directoryTree = new List<FileModel>();
            _projects = new List<ProjectModel>();

            _scopeFactory = scopeFactory;
            _logger = logger;
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

        public bool IsInProject(string absolutePath)
        {
            return !absolutePath.EndsWith(".csproj") && _projects.Select(p => p.Directory).Any(absolutePath.StartsWith);
        }

        public Task<string> GetProjectDirectoryAsync(string path)
        {
            var project = _projects.FirstOrDefault(p => path.StartsWith(p.Directory));
            return Task.FromResult(project?.Directory);
        }

        public async Task<string> GetNameSpaceAsync(IJSRuntime jsRuntime, string path)
        {
            var project = _projects.FirstOrDefault(p => path.StartsWith(p.Directory));
            if (project is null)
            {
                return null;
            }

            if (project.NameSpace is { Length: > 0 })
            {
                return project.NameSpace;
            }

            using var scope = _scopeFactory.CreateScope();
            var fileService = (IFileService)scope.ServiceProvider.GetService(typeof(IFileService));
            if (fileService == null)
            {
                throw new Exception("fileService is null.");
            }
            var projectFileContent = await fileService.ReadAsync(project.FullPath, jsRuntime);
            var index = projectFileContent.IndexOf('?');
            projectFileContent.Remove(index);
            _logger.LogInformation("projectFileContent");
            _logger.LogInformation("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + projectFileContent.TrimStart('?'));
            var projectXml = new XmlDocument();

            // TODO: Why start with '?'?
            projectXml.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + projectFileContent.TrimStart('?'));
            var nameSpaceNode = projectXml.SelectSingleNode("RootNamespace");
            project.NameSpace = nameSpaceNode is not null ? nameSpaceNode.InnerText : project.Name;

            return project.NameSpace;
        }

        private async Task GetProjectAsync(ICollection<FileModel> model)
        {
            if (model == null || model.Count == 0)
                return;

            var project = model.FirstOrDefault(
                f =>
                    f.FileType == FileType.File &&
                    Path.GetExtension(f.FullPath) == ".csproj");
            if (project is not null)
            {
                _projects.Add(new ProjectModel()
                {
                    FullPath = project.FullPath,
                    FileType = project.FileType,
                    Children = project.Children
                });
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
