using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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

                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task AddNewEntityAsync(ModalSaveClickEventArgs args)
        {
            if (args.Data is EntityModel model)
            {
                await _dmdCanvasContext.AddEntityComponentAsync(model);
                await Mediator.Publish(new EntityCreatedEvent(model));
            }
        }

        private void ToggleModal()
        {
            _entityModal.OpenAsync();
        }
    }
}
