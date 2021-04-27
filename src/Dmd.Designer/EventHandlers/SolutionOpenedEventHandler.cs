using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dmd.Designer.Events;
using Dmd.Designer.Services.File;
using Dmd.Designer.Services.Solution;
using MediatR;

namespace Dmd.Designer.EventHandlers
{
    public class SolutionOpenedEventHandler : INotificationHandler<SolutionOpenedEvent>
    {
        private const string DmdPropsName = "dmd.props";

        private const string DmdPropsContent = @"
<Project>
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

        private readonly ISolutionManager _solutionManager;
        private readonly IFileService _fileService;

        public SolutionOpenedEventHandler(
            ISolutionManager solutionManager,
            IFileService fileService)
        {
            _solutionManager = solutionManager;
            _fileService = fileService;
        }

        public async Task Handle(SolutionOpenedEvent notification, CancellationToken cancellationToken)
        {
            await _fileService.SaveAsync(_solutionManager.Solution.Directory, DmdPropsName, DmdPropsContent);
        }
    }
}
