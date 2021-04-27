using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using Dmd.Designer.Events;
using Dmd.Designer.Models.Solution;
using Dmd.Designer.Services.File;
using MediatR;
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

            // TODO: Better way to set dmd.props
            //using var scope = _scopeFactory.CreateScope();
            //var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            //await mediator.Publish(new SolutionOpenedEvent());
            await SetDmdPropsAsync(jsRuntime);

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

        // The IJSRuntime parameter is passed in because it reports a JavaScript interop calls error
        // TODO: Better way to use JavaScript interop 
        public async Task<string> GetNameSpaceAsync(IJSRuntime jsRuntime, string path)
        {
            var project = _projects.FirstOrDefault(p => path.StartsWith(p.Directory));
            if (project is null)
            {
                return null;
            }

            var rootNameSpace = await GetProjectNameSpaceAsync(jsRuntime, project);
            _logger.LogInformation(rootNameSpace);
            var relativePath = path.TrimStart(project.Directory.ToCharArray());
            _logger.LogInformation(relativePath);
            var nameSpaceList = relativePath.Split('\\').ToList();
            nameSpaceList.RemoveAll(string.IsNullOrWhiteSpace);

            return nameSpaceList.Aggregate(rootNameSpace, (current, item) => current + $".{item}");
        }

        // TODO: Better way to get project namespace
        private async Task<string> GetProjectNameSpaceAsync(IJSRuntime jsRuntime, ProjectModel project)
        {
            if (project.NameSpace is { Length: > 0 })
            {
                return project.NameSpace;
            }

            using var scope = _scopeFactory.CreateScope();
            var fileService = scope.ServiceProvider.GetService<IFileService>();
            if (fileService == null)
            {
                throw new Exception("fileService is null.");
            }
            var projectFileContent = await fileService.ReadAsync(project.FullPath, jsRuntime);

            const string rootNameSpaceNodeStart = "<RootNamespace>";
            const string rootNameSpaceNodeEnd = "</RootNamespace>";
            string nameSpace;
            if (projectFileContent.Contains(rootNameSpaceNodeStart) &&
                projectFileContent.Contains(rootNameSpaceNodeEnd))
            {
                var startIndex = projectFileContent.IndexOf(rootNameSpaceNodeStart, StringComparison.OrdinalIgnoreCase) + rootNameSpaceNodeStart.Length;
                var endIndex = projectFileContent.IndexOf(rootNameSpaceNodeEnd, StringComparison.OrdinalIgnoreCase);

                nameSpace = projectFileContent.Substring(startIndex, endIndex - startIndex);
            }
            else
            {
                nameSpace = project.Name;
            }

            project.NameSpace = nameSpace;
            _logger.LogInformation(nameSpace);
            return nameSpace;
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

        private async Task SetDmdPropsAsync(IJSRuntime jsRuntime)
        {
            const string dmdPropsName = "dmd.props";

            const string dmdPropsContent =
@"<Project>
	<ItemGroup>
		<PackageReference Include=""Newtonsoft.Json"" Version=""13.0.1"" GeneratePathProperty=""true""/>
	</ItemGroup>
	<PropertyGroup>
		<GetTargetPathDependsOn>$(GetTargetPathDependsOn);GetDependencyTargetPaths</GetTargetPathDependsOn>
	</PropertyGroup>

	<Target Name=""GetDependencyTargetPaths"">
		<ItemGroup>
			<TargetPathWithTargetPlatformMoniker Include=""$(PkgNewtonsoft_Json)\lib\netstandard2.0\Newtonsoft.Json.dll"" IncludeRuntimeDependency=""false"" />
		</ItemGroup>
	</Target>
</Project>";
            var scope = _scopeFactory.CreateScope();
            var fileService = scope.ServiceProvider.GetRequiredService<IFileService>();
            await fileService.SaveAsync(_solution.Directory, dmdPropsName, dmdPropsContent, jsRuntime);
        }

        private ValueTask<IJSObjectReference> GetJsObjectReferenceAsync(IJSRuntime jsRuntime)
        {
            return jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./js/solution.js");
        }
    }
}
