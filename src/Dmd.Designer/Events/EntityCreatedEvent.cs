using Dmd.Designer.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dmd.Designer.Events
{
    public class EntityCreatedEvent : INotification
    {
        public EntityModel EntityModel { get; init; }

        public EntityCreatedEvent(EntityModel entityModel)
        {
            EntityModel = entityModel;
        }
    }
}
