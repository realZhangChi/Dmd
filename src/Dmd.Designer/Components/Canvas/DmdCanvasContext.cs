using System;
using System.Threading.Tasks;
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

        public async Task AddClassComponentAsync(
            string name,
            string[] properties,
            string[] methods,
            double[] position)
        {
            var js = await JsTask.Value;
            await js.InvokeAsync<string>("addClass",
                new object[]
                {
                    name,
                    properties,
                    methods,
                    position
                });
        }
    }
}
