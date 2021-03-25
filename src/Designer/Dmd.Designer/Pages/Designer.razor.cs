﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorContextMenu;
using Blazorise;
using Dmd.Designer.Components.Canvas;
using Dmd.Designer.Models;
using Dmd.Designer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Dmd.Designer.Pages
{
    public partial class Designer
    {
        private DmdCanvasComponent _dmdCanvasComponent;
        private DmdCanvasContext _dmdCanvasContext;
        private Modal _addNewModalRef;
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

        public Designer()
        {
            _newClassModel = new ClassModel();
            SolutionRoot = new SolutionTreeNodeModel();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _fsJsTask = new Lazy<Task<IJSObjectReference>>(() => JsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./js/fs.js").AsTask());
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            var solutionPath = Path.GetDirectoryName(SolutionPath);
            var solutionName = Path.GetFileNameWithoutExtension(SolutionPath);
            SolutionRoot = new SolutionTreeNodeModel()
            {
                Path = solutionPath,
                Name = solutionName
            };
            var fsJs = await _fsJsTask.Value;
            var childrenJson = await fsJs.InvokeAsync<string>("getDirectoryChildren", solutionPath);
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
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Run(() =>
                {
                    _dmdCanvasContext = _dmdCanvasComponent.Context();
                });
            }
        }

        private void ShowAddNewModal()
        {
            Logger.LogInformation("ShowAddNewModal");
            _newClassModel = new ClassModel()
            {
                Properties = new List<string>() { string.Empty }
            };
            _addNewModalRef.Show();
        }

        private void HideAddNewModal()
        {
            _addNewModalRef.Hide();
        }

        private async Task AddNewEntityAsync()
        {
            //await _dmdCanvasContext.AddClassComponentAsync(
            //    _newClassModel.Name,
            //    //new[]
            //    //{
            //    //    _newClassModel.Properties
            //    //},
            //    new[]
            //    {
            //        _newClassModel.Methods
            //    },
            //    new[]
            //    {
            //        _windowWidth / 2,
            //        _windowHeight / 2
            //    });
            _addNewModalRef.Hide();
        }

        private async Task OnAddEntityClickedAsync(ItemClickEventArgs e)
        {
            ShowAddNewModal();
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
