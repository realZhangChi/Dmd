using System;
using System.Threading.Tasks;
using Dmd.Designer.Services.Canvas;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Dmd.Designer.Components.Canvas
{
    public partial class DmdCanvasComponent
    {

        [Parameter]
        public double Height { get; set; }

        [Parameter]
        public double Width { get; set; }

        protected readonly string Id;

        public Lazy<Task<IJSObjectReference>> JsTask;

        public ElementReference CanvasReference;

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Inject]
        private ICanvasService CanvasService { get; set; }

        [Inject]
        private ILogger<DmdCanvasComponent> Logger { get; set; }

        public DmdCanvasComponent()
        {
            Id = Guid.NewGuid().ToString();
        }

        protected override Task OnInitializedAsync()
        {
            JsTask = new(() => JsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./js/canvas.js").AsTask());
            return base.OnInitializedAsync();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var js = await JsTask.Value;
                await js.InvokeVoidAsync("init", Id);

                var json = await CanvasService.GetJsonAsync();

                Logger.LogInformation(json);
                if (json is { Length: > 0 })
                {
                    await js.InvokeVoidAsync("loadFromJSON", json);
                }
            }
        }

    }
}
