using Dmd.Designer.Events;
using Dmd.Designer.Models;
using Dmd.Designer.Services.Canvas;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dmd.Designer.Services.Generator;
using Dmd.Designer.Services.Project;

namespace Dmd.Designer.EventHandlers
{
    public class EntityCreatedHandler : INotificationHandler<EntityCreatedEvent>
    {
        private readonly ICanvasService _canvasService;
        private readonly IGeneratorService _generatorService;
        private readonly IProjectService _projectService;

        public EntityCreatedHandler(
            ICanvasService canvasService,
            IGeneratorService generatorService,
            IProjectService projectService)
        {
            _canvasService = canvasService;
            _generatorService = generatorService;
            _projectService = projectService;
        }

        public async Task Handle(EntityCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _canvasService.SaveToJsonAsync();
            await _projectService.EnsurePropsReferenceAsync(notification.EntityModel.ProjectFullPath);
            await _generatorService.GenerateEntityJsonAsync(notification.EntityModel.Namespace + "." +
                                                            notification.EntityModel.Name);
        }
    }
}
