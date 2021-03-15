using System;
using System.IO;
using System.Threading.Tasks;
using Dmd.CodeGenerator.CodeBuilders;
using Dmd.CodeGenerator.Options;

namespace Dmd.CodeGenerator.Generators
{
    public class CodeGenerator : ICodeGenerator
    {
        private readonly IndentedCodeBuilder _codeBuilder;

        public CodeGenerator()
        {
            _codeBuilder = new IndentedCodeBuilder();
        }

        public string Generate(CodeGeneratorOptions options)
        {
            _codeBuilder.AppendLine($"//{DateTime.Now}");
            _codeBuilder.AppendLine("using System;");
            _codeBuilder.AppendLine("using System.Collections.Generic;");
            _codeBuilder.AppendLine();

            _codeBuilder.AppendLine($"namespace {options.Namespace}");
            _codeBuilder.AppendLine("{");

            using (_codeBuilder.Indent())
            {
                GenerateInternal(options);
            }

            _codeBuilder.AppendLine("}");
            var content = _codeBuilder.ToString();

            return content;
        }

        private void GenerateInternal(CodeGeneratorOptions options)
        {
            _codeBuilder.AppendLine($"public partial {options.ClassType.ToString("G").ToLower()} {options.Name}");
            _codeBuilder.AppendLine("{");

            using (_codeBuilder.Indent())
            {
                GenerateProperties(options);
            }

            _codeBuilder.AppendLine("}");
        }

        private void GenerateProperties(CodeGeneratorOptions options)
        {
            foreach (var property in options.Properties)
            {
                _codeBuilder.AppendLine($"public {property.Type} {property.Name} " + "{ get; set; }");
                _codeBuilder.AppendLine();
            }
        }
    }
}