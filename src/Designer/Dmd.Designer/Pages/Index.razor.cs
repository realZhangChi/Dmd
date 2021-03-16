using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Dmd.Designer.Pages
{
    public partial class Index
    {
        [Inject]
        private ILogger<Index> Logger { get; set; }

        private async Task OnOpenBtnClicked()
        {
            var mainWindow = Electron.WindowManager.BrowserWindows.First();
            var options = new OpenDialogOptions()
            {
                Title = "Select a solution",
                Properties = new[] {OpenDialogProperty.openFile},
                Filters = new []{new FileFilter(){Name = "solution", Extensions = new []{"sln"}}}
            };

            var result = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);
            Logger.LogInformation(result?[0]);
        }
    }
}
