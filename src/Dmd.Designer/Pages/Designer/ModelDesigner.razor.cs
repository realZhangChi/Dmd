using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorContextMenu;
using Dmd.Designer.Components.Canvas;
using Dmd.Designer.Events;
using Dmd.Designer.Models;
using Dmd.Designer.Models.Solution;
using Dmd.Designer.Services;
using Dmd.Designer.Services.Canvas;
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

        protected EntityModel EntityModel { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Inject]
        private ILogger<ModelDesigner> Logger { get; set; }

        [Inject]
        private ISolutionManager SolutionManager { get; set; }

        [Inject]
        private IMediator Mediator { get; set; }

        [Parameter]
        public string SolutionPath { get; set; }

        protected string SiderStyle => $"background: #dee2e6;min-width: 260px;";

        public ModelDesigner()
        {
            EntityModel = new EntityModel();
        }

        protected override async Task OnInitializedAsync()
        {
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
            if (firstRender)
            {
                _dmdCanvasContext = _dmdCanvasComponent.Context();

                await SolutionManager.SetSolutionPathAsync(JsRuntime, SolutionPath);
                Logger.LogInformation(JsonSerializer.Serialize(SolutionManager));

                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task AddNewEntityAsync()
        {
            await _dmdCanvasContext.AddEntityComponentAsync(EntityModel);
            await Mediator.Publish(new EntityCreatedEvent(EntityModel));
        }

        private async Task ToggleModal(ItemClickEventArgs args)
        {
            if (args.Data is string path)
            {
                EntityModel = new EntityModel()
                {
                    Properties = new List<PropertyModel>() { new() },
                    ProjectDirectory = await SolutionManager.GetProjectDirectoryAsync(path),
                    Namespace = await SolutionManager.GetNameSpaceAsync(JsRuntime, path)
                };
                await _entityModal.OpenAsync();
            }
        }
    }
}
