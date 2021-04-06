using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dmd.Designer.Models;
using Dmd.Designer.Services.File;
using Dmd.Designer.Services.Solution;
using Dmd.SourceOptions;
using Microsoft.Extensions.Logging;

namespace Dmd.Designer.Services.Generator
{
    public class GeneratorService : IGeneratorService
    {
        private readonly IFileService _fileService;
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
            ISolutionManager solutionManager,
            ILogger<GeneratorService> logger) : this()
        {
            _fileService = fileService;
            _solutionManager = solutionManager;
            _logger = logger;
        }

        public async Task GenerateEntityJsonAsync(string key)
        {
            var json = await _fileService.ReadAsync(Path.Combine(_solutionManager.Solution.Directory,
                "dmd_model.json"));

            var jsonDoc = JsonDocument.Parse(json);
            var entities = jsonDoc.RootElement.GetProperty("objects").EnumerateArray()
                .Where(e => e.GetProperty("type").ToString() == "entity")
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
                    .GroupBy(e => e.ProjectDirectory);
                foreach (var group in groups)
                {
                    await _fileService.SaveAsync(group.Key + "\\dmd", "dmd_entity.json",
                        JsonSerializer.Serialize(group.ToList(), _jsonSerializerOptions));
                }
            }
        }
    }
}
