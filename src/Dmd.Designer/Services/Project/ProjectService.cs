using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Xml;
using Dmd.Designer.Services.File;
using Dmd.Designer.Services.Solution;
using Microsoft.Extensions.Logging;

namespace Dmd.Designer.Services.Project
{
    public class ProjectService : IProjectService
    {
        private readonly IFileService _fileService;
        private readonly ISolutionManager _solutionManager;
        private readonly ILogger _logger;

        public ProjectService(
            IFileService fileService,
            ISolutionManager solutionManager,
            ILogger<ProjectService> logger)
        {
            _fileService = fileService;
            _solutionManager = solutionManager;
            _logger = logger;
        }

        public async Task EnsurePropsReferenceAsync(string projectPath)
        {
            var content = await _fileService.ReadAsync(projectPath);

            // Avoid file reading errors. (.csproj content start with '?')
            content = content.Substring(content.IndexOf("<Project", StringComparison.Ordinal));
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);
            _logger.LogInformation(xmlDoc.InnerXml);
            var projectNode = xmlDoc.SelectSingleNode("//Project");
            if (projectNode is null)
            {
                // TODO: handle error
                return;
            }
            var importNodes = projectNode.SelectNodes("/Import");
            var containsDmd = false;
            if (importNodes is not null)
            {
                if (importNodes.Cast<XmlNode>().Any(node => node.Attributes is not null &&
                                                            node.Attributes.Cast<XmlAttribute>()
                                                                .Any(attribute => attribute.Value.Contains("dmd.props"))))
                {
                    containsDmd = true;
                }

                if (containsDmd)
                {
                    return;
                }
            }

            var level = projectPath.Substring(_solutionManager.Solution.Directory.Length)
                .Count(s => s == '\\');
            var importValue = "dmd.props";
            for (var i = 0; i < level - 1; i++)
            {
                importValue = "..\\" + importValue;
            }

            var importNode = xmlDoc.CreateElement("Import");
            importNode.SetAttribute("Project", importValue);
            projectNode.PrependChild(importNode);

            await using var stringWriter = new StringWriter();
            await using var writer = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
            xmlDoc.WriteContentTo(writer);
            writer.Close();
            var formattedXml = stringWriter.ToString();
            stringWriter.Close();

            _logger.LogInformation(Path.GetDirectoryName(projectPath));
            await _fileService.SaveAsync(Path.GetDirectoryName(projectPath), Path.GetFileName(projectPath), formattedXml);
        }
    }
}
