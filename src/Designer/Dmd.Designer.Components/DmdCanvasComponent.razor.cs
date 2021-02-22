using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dmd.Designer.Components
{
    public partial class DmdCanvasComponent : ComponentBase
    {

        [Parameter]
        public long Height { get; set; }

        [Parameter]
        public long Width { get; set; }

        protected readonly string Id;

        private Lazy<Task<IJSObjectReference>> _jsTask;

        protected ElementReference CanvasRef;

        public ElementReference CanvasReference => CanvasRef;

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        public DmdCanvasComponent()
        {
            Id = Guid.NewGuid().ToString();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            _jsTask = new(() => JsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Dmd.Designer.Components/canvas.js").AsTask());

            var js = await _jsTask.Value;
            await js.InvokeAsync<string>("init", Id);
        }

    }
}
