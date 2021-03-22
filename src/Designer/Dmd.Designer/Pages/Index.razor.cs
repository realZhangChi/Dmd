using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dmd.Designer.Models;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Dmd.Designer.Pages
{
    public partial class Index
    {
        [Inject]
        private ILogger<Index> Logger { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        public Lazy<Task<IJSObjectReference>> FsJsTask;

        //protected override Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        FsJsTask = new Lazy<Task<IJSObjectReference>>(() => JsRuntime.InvokeAsync<IJSObjectReference>(
        //            "import", "./js/fs.js").AsTask());
        //    }
        //    return base.OnAfterRenderAsync(firstRender);
        //}

        private async Task OnOpenBtnClicked()
        {

            var mainWindow = Electron.WindowManager.BrowserWindows.First();
            var options = new OpenDialogOptions()
            {
                Title = "Select a solution",
                Properties = new[] { OpenDialogProperty.openFile },
                Filters = new[] { new FileFilter() { Name = "solution", Extensions = new[] { "sln" } } }
            };

            var result = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);
            if (result is { Length: > 0 })
            {
                //var path = Path.GetDirectoryName(result[0]);
                //Logger.LogInformation(path);
                NavigationManager.NavigateTo($"designer");
                //var fsJs = await FsJsTask.Value;
                //var directoryInfo = await fsJs.InvokeAsync<string>("getDirectoryInfo", path);
                //Logger.LogInformation(directoryInfo);
                //var items = JsonSerializer.Deserialize<DirectoryModel>(directoryInfo, new JsonSerializerOptions()
                //{
                //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                //});
                //Logger.LogInformation("Serialized");
                //Logger.LogInformation(JsonSerializer.Serialize(items));
            }
        }
    }
}
