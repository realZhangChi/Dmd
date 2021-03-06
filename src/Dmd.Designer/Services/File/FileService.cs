﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Dmd.Designer.Services.File
{
    public class FileService : JsBaseService, IFileService
    {
        private readonly ILogger _logger;
        public FileService(IJSRuntime jsRuntime,
            ILogger<FileService> logger)
        {
            JsTask = new Lazy<Task<IJSObjectReference>>(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./js/file.js").AsTask());
            _logger = logger;
        }

        public async Task SaveAsync(string directory, string name, string content, IJSRuntime jsRuntime = null)
        {
            // TODO: directory and name validate
            //directory = directory.Replace(@"\\", "/");
            var js = await GetJsObjectReferenceAsync(jsRuntime);
            await js.InvokeVoidAsync("save", directory, name, content);
        }

        public async Task<string> ReadAsync(string fullPath, IJSRuntime jsRuntime = null)
        {
            fullPath = fullPath.Replace(@"\\", "/");
            var js = await GetJsObjectReferenceAsync(jsRuntime);
            var result = await js.InvokeAsync<string>("readFile", fullPath);
            return result;
        }

        public async Task<bool> AccessAsync(string fullPath)
        {
            fullPath = fullPath.Replace(@"\\", "/");
            var js = await JsTask.Value;
            var access = await js.InvokeAsync<bool>("access", fullPath);
            return access;
        }

        private async Task<IJSObjectReference> GetJsObjectReferenceAsync(IJSRuntime jsRuntime = null)
        {
            return jsRuntime is null ?
                await JsTask.Value :
                await jsRuntime.InvokeAsync<IJSObjectReference>(
                    "import", "./js/file.js");
        }
    }
}
