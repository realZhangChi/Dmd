using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Blazorise;
using Dmd.Designer.Components.Canvas;
using Dmd.Designer.Models;
using Dmd.Designer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Dmd.Designer.Pages
{
    public partial class Designer : ComponentBase
    {
        //private DmdCanvasComponent _dmdCanvasComponent;
        //private DmdCanvasContext _dmdCanvasContext;
        private Modal _addNewModalRef;
        private ClassModel _newClassModel;
        private Lazy<Task<IJSObjectReference>> _fsJsTask;

        private double windowWidth;
        private double windowHeight;

        [Inject]
        private IBrowserService BrowserService { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }


        [Inject]
        private ILogger<Designer> Logger { get; set; }

        //[Parameter]
        //public string SolutionPath { get; set; }

        protected List<DirectoryModel> DirectoryModel { get; set; }

        protected string Content { get; set; }


        public Designer()
        {
            _newClassModel = new ClassModel();
            DirectoryModel = new List<DirectoryModel>();
        }

        protected override async Task OnInitializedAsync()
        {
            _fsJsTask = new Lazy<Task<IJSObjectReference>>(() => JsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./js/fs.js").AsTask());
            var dimensions = await BrowserService.GetDimensionsAsync();
            windowHeight = dimensions.Height;
            windowWidth = dimensions.Width;

            var path = Path.GetDirectoryName("C:/Users/Chi/source/repos/Dmd/Dmd.sln");
            Logger.LogInformation(path);
            var fsJs = await _fsJsTask.Value;
            var directoryInfo = await fsJs.InvokeAsync<string>("getDirectoryInfo", path);
            Logger.LogInformation(directoryInfo);
            DirectoryModel.Add(JsonSerializer.Deserialize<DirectoryModel>(directoryInfo, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
            Content = JsonSerializer.Serialize(DirectoryModel);
            Logger.LogInformation(Content);
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
            }
            
            //await Task.Run(() =>
            //{
            //    _dmdCanvasContext = _dmdCanvasComponent.Context();
            //});
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

        //private async Task AddNewEntityAsync()
        //{
        //    await _dmdCanvasContext.AddClassComponentAsync(
        //        _newClassModel.Name,
        //        new[]
        //        {
        //            _newClassModel.Properties
        //        },
        //        new[]
        //        {
        //            _newClassModel.Methods
        //        },
        //        new[]
        //        {
        //            windowWidth / 2,
        //            windowHeight / 2
        //        });
        //    _addNewModalRef.Hide();
        //}
    }
}
