using System;
using System.IO;
using System.Threading.Tasks;
using Dmd.CodeGenerator.CodeWriters;
using Dmd.CodeGenerator.Options;

namespace Dmd.CodeGenerator.Generators
{
    public class CodeGenerator : ICodeGenerator
    {
        private readonly IndentedCodeBuilder _codeBuilder;
        private CodeGeneratorOptions _options;

        public CodeGenerator(IndentedCodeBuilder codeBuilder)
        {
            _codeBuilder = codeBuilder;
        }

        public async Task<string> GenerateAsync(CodeGeneratorOptions options)
        {
            _options = options;

            await _codeBuilder.AppendLineAsync("using System;");
            await _codeBuilder.AppendLineAsync("using System.Collections.Generic;");
            await _codeBuilder.AppendLineAsync();

            await _codeBuilder.AppendLineAsync($"namespace {_options.Namespace}");
            await _codeBuilder.AppendLineAsync("{");

            using (_codeBuilder.Indent())
            {
                await GenerateInternalAsync();
            }

            await _codeBuilder.AppendLineAsync("}");
            var content = _codeBuilder.ToString();

            return content;
        }

        private async Task GenerateInternalAsync()
        {
            await _codeBuilder.AppendLineAsync($"public partial {_options.ClassType.ToString("G").ToLower()} {_options.Name}");
            await _codeBuilder.AppendLineAsync("{");

            using (_codeBuilder.Indent())
            {
                await GeneratePropertiesAsync(_options);
            }

            await _codeBuilder.AppendLineAsync("}");
        }

        private async Task GeneratePropertiesAsync(CodeGeneratorOptions options)
        {
            foreach (var property in options.Properties)
            {
                await _codeBuilder.AppendLineAsync($"public {property.Type} {property.Name} " + "{ get; set; }");
                await _codeBuilder.AppendLineAsync();
            }
        }
    }
}