using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorContextMenu;
using Blazorise;
using Dmd.Designer.Components.Canvas;
using Dmd.Designer.Models;
using Dmd.Designer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Dmd.Designer.Pages.Designer
{
    public partial class Designer
    {
        private DmdCanvasComponent _dmdCanvasComponent;
        private DmdCanvasContext _dmdCanvasContext;
        private EntityModal entityModal;
        private ClassModel _newClassModel;
        private Lazy<Task<IJSObjectReference>> _fsJsTask;

        [Inject]
        private IBrowserService BrowserService { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Inject]
        private ILogger<Designer> Logger { get; set; }

        [Parameter]
        public string SolutionPath { get; set; }

        protected SolutionTreeNodeModel SolutionRoot { get; set; }

        protected string SiderStyle => $"background: #dee2e6;min-width: 260px;";

        private bool _modalShow;
        private string ModalClass => _modalShow ? "modal fade show" : "modal fade";
        private string ModalStyle => _modalShow ? "display: block;" : "display: none;";

        public Designer()
        {
            _newClassModel = new ClassModel();
            SolutionRoot = new SolutionTreeNodeModel();
        }

        protected override async Task OnInitializedAsync()
        {
            _fsJsTask = new Lazy<Task<IJSObjectReference>>(() => JsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./js/fs.js").AsTask());
            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            Logger.LogInformation("OnParametersSetAsync");
            SolutionPath = WebUtility.UrlDecode(SolutionPath);
            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            Logger.LogInformation($"OnAfterRenderAsync!firstRender:{firstRender}");
            if (firstRender)
            {
                _dmdCanvasContext = _dmdCanvasComponent.Context();
                var solutionPath = Path.GetDirectoryName(SolutionPath);
                var solutionName = Path.GetFileNameWithoutExtension(SolutionPath);
                SolutionRoot.Path = solutionPath;
                SolutionRoot.Name = solutionName;
                var fsJs = await _fsJsTask.Value;
                var childrenJson = await fsJs.InvokeAsync<string>("getDirectoryChildren", solutionPath);
                Logger.LogInformation(childrenJson);
                var children = JsonSerializer.Deserialize<List<SolutionTreeNodeModel>>(
                    childrenJson,
                    new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                if (children is not null)
                {
                    ((List<SolutionTreeNodeModel>)SolutionRoot.Children).AddRange(children);
                }
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private void ShowAddNewModal()
        {
            Logger.LogInformation("ShowAddNewModal");
            _newClassModel = new ClassModel()
            {
                Properties = new List<string>() { string.Empty }
            };
            //_addNewModalRef.Show();
        }

        private void HideAddNewModal()
        {
            //_addNewModalRef.Hide();
        }

        private async Task AddNewEntityAsync(ModalSaveClickEventArgs args)
        {
            if (args.Data is ClassModel model)
            {
                Logger.LogInformation(JsonSerializer.Serialize(model));
                var dimension = await BrowserService.GetDimensionsAsync();
                await _dmdCanvasContext.AddClassComponentAsync(
                    model.Name,
                    model.Properties.ToArray(),
                    new string[] { },
                    new[]
                    {
                        dimension.Width / 2,
                        dimension.Height / 2
                    });
            }
            //_addNewModalRef.Hide();
        }

        private async Task OnAddEntityClickedAsync(ItemClickEventArgs e)
        {
            ShowAddNewModal();
        }

        private void ToggleModal()
        {
            entityModal.OpenAsync();
        }

        private Task OnPropertyAddClickedAsync()
        {
            Logger.LogInformation("OnPropertyAddClickedAsync");
            _newClassModel.Properties.Add(string.Empty);
            Logger.LogInformation(JsonSerializer.Serialize(_newClassModel));
            return Task.CompletedTask;
        }
    }
}
