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

namespace Dmd.Designer.EventHandlers
{
    public class EntityCreatedHandler : INotificationHandler<EntityCreatedEvent>
    {
        private readonly ICanvasService _canvasService;
        private readonly IGeneratorService _generatorService;

        public EntityCreatedHandler(
            ICanvasService canvasService,
            IGeneratorService generatorService)
        {
            _canvasService = canvasService;
            _generatorService = generatorService;
        }

        public async Task Handle(EntityCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _canvasService.SaveToJsonAsync();
            await _generatorService.GenerateEntityJsonAsync(notification.EntityModel.Namespace + "." +
                                                            notification.EntityModel.Name);
        }
    }
}
