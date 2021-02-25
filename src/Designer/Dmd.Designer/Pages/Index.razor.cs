using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Dmd.Designer.Components.Canvas;
using Dmd.Designer.Models;

namespace Dmd.Designer.Pages
{
    public partial class Index
    {
        private DmdCanvasComponent _dmdCanvasComponent;
        private DmdCanvasContext _dmdCanvasContext;
        private Modal _addNewModalRef;
        private ClassModel _newClassModel;

        public Index()
        {
            _newClassModel = new ClassModel();
        }

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
                new double[]
                {
                    100,
                    100
                });
            _addNewModalRef.Hide();
        }
    }
}
