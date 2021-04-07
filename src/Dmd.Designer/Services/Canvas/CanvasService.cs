using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dmd.Designer.Services.File;
using Dmd.Designer.Services.Solution;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Dmd.Designer.Services.Canvas
{
    public class CanvasService : ICanvasService
    {
        private readonly Lazy<Task<IJSObjectReference>> _jsTask;
        private readonly ILogger _logger;
        private readonly ISolutionManager _solutionManager;
        private readonly IFileService _fileService;

        public CanvasService(
            ISolutionManager solutionManager,
            IFileService fileService,
            IJSRuntime jsRuntime,
            ILogger<CanvasService> logger)
        {
            _logger = logger;
            _solutionManager = solutionManager;
            _fileService = fileService;
            _jsTask = new Lazy<Task<IJSObjectReference>>(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./js/canvas.js").AsTask());
        }

        public async Task SaveToJsonAsync()
        {
            if (_solutionManager.Solution.FullPath is not { Length: > 0 })
            {
                _logger.LogWarning("Save to json failed! solution full path is null or empty!");
                return;
            }

            var js = await _jsTask.Value;
            var json = await js.InvokeAsync<string>("toJson");
            const string name = "dmd_model.json";
            await _fileService.SaveAsync(_solutionManager.Solution.Directory, name, json);
        }

        public async Task<string> GetJsonAsync()
        {
            if (_solutionManager.Solution.FullPath is not { Length: > 0 })
            {
                _logger.LogWarning("Get json failed! solution full path is null or empty!");
                return null;
            }

            return await _fileService.ReadAsync(Path.Combine(_solutionManager.Solution.Directory,
                "dmd_model.json"));
        }
    }
}
