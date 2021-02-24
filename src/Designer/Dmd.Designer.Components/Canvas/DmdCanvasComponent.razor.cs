using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dmd.Designer.Components.Canvas
{
    public partial class DmdCanvasComponent : ComponentBase
    {

        [Parameter]
        public long Height { get; set; }

        [Parameter]
        public long Width { get; set; }

        protected readonly string Id;

        public readonly Lazy<Task<IJSObjectReference>> JsTask;

        public ElementReference CanvasReference;

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        public DmdCanvasComponent()
        {
            Id = Guid.NewGuid().ToString();
            JsTask = new(() => JsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Dmd.Designer.Components/canvas.js").AsTask());
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            var js = await JsTask.Value;
            await js.InvokeAsync<string>("init", Id);
        }

    }
}
