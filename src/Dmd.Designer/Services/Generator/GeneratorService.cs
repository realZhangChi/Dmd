using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dmd.Designer.Models;
using Dmd.Designer.Services.Canvas;
using Dmd.Designer.Services.File;
using Dmd.Designer.Services.Solution;
using Dmd.SourceOptions;
using Microsoft.Extensions.Logging;

namespace Dmd.Designer.Services.Generator
{
    public class GeneratorService : IGeneratorService
    {
        private readonly IFileService _fileService;
        private readonly ICanvasService _canvasService;
        private readonly ISolutionManager _solutionManager;
        private readonly ILogger _logger;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public GeneratorService()
        {
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public GeneratorService(
            IFileService fileService,
            ICanvasService canvasService,
            ISolutionManager solutionManager,
            ILogger<GeneratorService> logger) : this()
        {
            _fileService = fileService;
            _canvasService = canvasService;
            _solutionManager = solutionManager;
            _logger = logger;
        }

        public async Task GenerateEntityJsonAsync(string key)
        {
            var json = await _canvasService.GetJsonAsync();

            var jsonDoc = JsonDocument.Parse(json);
            var entities = jsonDoc.RootElement.GetProperty("objects").EnumerateArray()
                .Where(e => e.GetProperty("model_type").ToString() == "entity")
                .Select(e =>
                    JsonSerializer.Deserialize<EntityModel>(
                        e.GetProperty("model").GetRawText(),
                        _jsonSerializerOptions))
                .Distinct()
                .ToList();
            _logger.LogInformation(JsonSerializer.Serialize(entities));
            if (entities is { Count: > 0 })
            {
                var groups = entities
                    .GroupBy(e => e.ProjectFullPath);
                foreach (var group in groups)
                {
                    await _fileService.SaveAsync(Path.GetDirectoryName(group.Key) + "\\dmd", "entity.json",
                        JsonSerializer.Serialize(group.ToList(), _jsonSerializerOptions));
                }
            }
        }
    }
}
