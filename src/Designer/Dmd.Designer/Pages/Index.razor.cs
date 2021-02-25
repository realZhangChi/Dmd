using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmd.Designer.Components.Canvas;

namespace Dmd.Designer.Pages
{
    public partial class Index
    {
        private DmdCanvasComponent _dmdCanvasComponent;
        private DmdCanvasContext _dmdCanvasContext;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await Task.Run(() =>
            {
                _dmdCanvasContext = _dmdCanvasComponent.Context();
            });
        }

        private async Task AddEntityAsync()
        {
            await _dmdCanvasContext.AddClassComponentAsync(
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
                });
            await _dmdCanvasContext.AddClassComponentAsync(
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
                });
        }
    }
}
