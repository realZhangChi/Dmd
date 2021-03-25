using System.IO;
using System.Linq;
using System.Net;
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

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

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
                var path = Path.GetDirectoryName(result[0]);
                Logger.LogInformation($"result[0]:{result[0]}");
                Logger.LogInformation(path);
                NavigationManager.NavigateTo($"designer/{result[0]}");
            }
        }
    }
}
