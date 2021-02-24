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
            await js.InvokeAsync<string>("addClass", 
                new object[]
                { 
                    "User",
                    new string[]
                    {
                        "Id",
                        "Name",
                        "Sex",
                        "Age"
                    },
                    new string[]
                    {
                        "SetName()",
                        "SetAge()"
                    },
                    new double[]
                    {
                        100,
                        100
                    }
                });
            await js.InvokeAsync<string>("addClass",
                new object[]
                {
                    "Product",
                    new string[]
                    {
                        "Id",
                        "Name",
                        "Price"
                    },
                    new string[]
                    {
                        "SetName()",
                        "SetPrice()"
                    },
                    new double[]
                    {
                        300,
                        300
                    }
                });
        }

    }
}
