using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Dmd.CodeGenerator.CodeWriters;
using Dmd.CodeGenerator.Options;

namespace Dmd.CodeGenerator.Generators
{
    public sealed class EntityGenerator : IEntityGenerator, ITransientDependency
    {
        private readonly IndentedCodeBuilder _codeBuilder;
        private EntityOptions _entityOptions;

        public EntityGenerator(IndentedCodeBuilder codeBuilder)
        {
            _codeBuilder = codeBuilder;
        }

        public async Task Generate(CodeGeneratorOptions options)
        {
            _entityOptions = options.GetEntityOptions();

            await _codeBuilder.AppendLineAsync("using System;");
            await _codeBuilder.AppendLineAsync("using System.Collections.Generic;");
            await _codeBuilder.AppendLineAsync();

            await _codeBuilder.AppendLineAsync($"namespace {_entityOptions.Namespace}");
            await _codeBuilder.AppendLineAsync("{");

            using (_codeBuilder.Indent())
            {
                await GenerateClass();
            }

            await _codeBuilder.AppendLineAsync("}");
            var content = _codeBuilder.ToString();

            var directory = _entityOptions.Directory;
            var fileName = _entityOptions.Name + ".cs";
            var path = Path.Combine(directory, fileName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(Path.GetFullPath(path), content);
        }

        private async Task GenerateClass()
        {
            await _codeBuilder.AppendLineAsync($"public partial class {_entityOptions.Name}");
            await _codeBuilder.AppendLineAsync("{");

            using (_codeBuilder.Indent())
            {
                
            }

            await _codeBuilder.AppendLineAsync("}");
        }
    }
}