using System;
using System.Threading.Tasks;
using Dmd.Designer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dmd.Designer.Components.Canvas
{
    public class DmdCanvasContext
    {
        public ElementReference Canvas { get; init; }

        public Lazy<Task<IJSObjectReference>> JsTask;
        
        public DmdCanvasContext(DmdCanvasComponent component)
        {
            Canvas = component.CanvasReference;
            JsTask = component.JsTask;
        }

        public async Task AddEntityComponentAsync(EntityModel model)
        {
            var js = await JsTask.Value;
            await js.InvokeVoidAsync("addEntity", model);
        }
    }
}
