using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Dmd.Designer.Components.Canvas;
using Dmd.Designer.Events;
using Dmd.Designer.Models;
using Dmd.Designer.Models.Solution;
using Dmd.Designer.Services;
using Dmd.Designer.Services.Canvas;
using Dmd.Designer.Services.File;
using Dmd.Designer.Services.Solution;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Dmd.Designer.Pages.Designer
{
    public partial class ModelDesigner
    {
        private DmdCanvasComponent _dmdCanvasComponent;
        private DmdCanvasContext _dmdCanvasContext;
        private EntityModal _entityModal;
        private Lazy<Task<IJSObjectReference>> _fsJsTask;

        [Inject]
        private IBrowserService BrowserService { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Inject]
        private ILogger<ModelDesigner> Logger { get; set; }

        [Inject]
        private ISolutionManager SolutionManager { get; set; }

        [Inject]
        private ICanvasService CanvasService { get; set; }

        [Inject]
        private IMediator Mediator { get; set; }

        [Parameter]
        public string SolutionPath { get; set; }

        private string SolutionName { get; set; }
        private ICollection<FileModel> SolutionTree { get; set; }

        protected string SiderStyle => $"background: #dee2e6;min-width: 260px;";

        public ModelDesigner()
        {
            SolutionTree = new List<FileModel>();
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

                await SolutionManager.SetSolutionPathAsync(JsRuntime, SolutionPath);
                SolutionTree = SolutionManager.DirectoryTree;
                SolutionName = SolutionManager.Solution.Name;
                Logger.LogInformation(SolutionManager.Solution.Directory);

                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task AddNewEntityAsync(ModalSaveClickEventArgs args)
        {
            if (args.Data is EntityModel model)
            {
                var dimension = await BrowserService.GetDimensionsAsync();
                await _dmdCanvasContext.AddClassComponentAsync(
                    model.Name,
                    model.Properties.Select(p => p.Name).ToArray(),
                    Array.Empty<string>(),
                    new[]
                    {
                        dimension.Width / 2,
                        dimension.Height / 2
                    });
                await Mediator.Publish(new EntityCreatedEvent());
            }
        }

        private void ToggleModal()
        {
            _entityModal.OpenAsync();
        }
    }
}
