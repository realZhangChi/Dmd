using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Dmd.Designer.Components.Canvas;
using Dmd.Designer.Models;
using Dmd.Designer.Services;
using Microsoft.AspNetCore.Components;

namespace Dmd.Designer.Pages
{
    public partial class Designer
    {
        private DmdCanvasComponent _dmdCanvasComponent;
        private DmdCanvasContext _dmdCanvasContext;
        private Modal _addNewModalRef;
        private ClassModel _newClassModel;

        private double windowWidth;
        private double windowHeight;

        [Inject]
        private IBrowserService BrowserService { get; set; }

        public Designer()
        {
            _newClassModel = new ClassModel();
        }

        protected override async Task OnInitializedAsync()
        {
            var dimensions = await BrowserService.GetDimensionsAsync();
            windowHeight = dimensions.Height;
            windowWidth = dimensions.Width;
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await Task.Run(() =>
            {
                _dmdCanvasContext = _dmdCanvasComponent.Context();
            });
        }

        private void ShowAddNewModal()
        {
            _newClassModel = new ClassModel();
            _addNewModalRef.Show();
        }

        private void HideAddNewModal()
        {
            _addNewModalRef.Hide();
        }

        private async Task AddNewEntityAsync()
        {
            await _dmdCanvasContext.AddClassComponentAsync(
                _newClassModel.Name,
                new[]
                {
                    _newClassModel.Properties
                },
                new[]
                {
                    _newClassModel.Methods
                },
                new []
                {
                    windowWidth / 2,
                    windowHeight / 2
                });
            _addNewModalRef.Hide();
        }
    }
}
