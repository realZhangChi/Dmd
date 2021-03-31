using Dmd.Designer.Events;
using Dmd.Designer.Models;
using Dmd.Designer.Services.Canvas;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dmd.Designer.EventHandlers
{
    public class EntityCreatedHandler : INotificationHandler<EntityCreatedEvent>
    {
        private readonly ICanvasService _canvasService;

        public EntityCreatedHandler(ICanvasService canvasService)
        {
            _canvasService = canvasService;
        }

        public Task Handle(EntityCreatedEvent notification, CancellationToken cancellationToken)
        {
            return _canvasService.SaveToJsonAsync();
        }
    }
}
