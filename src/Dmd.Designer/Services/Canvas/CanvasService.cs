using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Dmd.Designer.Services.Canvas
{
    public class CanvasService : ICanvasService
    {
        private readonly Lazy<Task<IJSObjectReference>> _jsTask;
        private readonly ILogger _logger;

        public CanvasService(
            IJSRuntime jsRuntime,
            ILogger<CanvasService> logger)
        {
            _logger = logger;
            _jsTask = new Lazy<Task<IJSObjectReference>>(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./js/canvas.js").AsTask());
        }
        public async Task<string> GetJsonAsync()
        {
            _logger.LogInformation("GetJsonAsync");
            var js = await _jsTask.Value;
            var json = await js.InvokeAsync<string>("toJson");
            _logger.LogInformation(json);
            return json;
        }
    }
}
