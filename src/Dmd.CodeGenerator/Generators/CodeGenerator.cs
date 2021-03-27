using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Dmd.CodeGenerator.CodeBuilders;
using Dmd.SourceOptions;

namespace Dmd.CodeGenerator.Generators
{
    public class CodeGenerator : ICodeGenerator
    {
        private readonly IndentedCodeBuilder _codeBuilder;

        public CodeGenerator()
        {
            _codeBuilder = new IndentedCodeBuilder();
        }

        public string Generate(SourceOption options)
        {
            GenerateImports(options);

            _codeBuilder.AppendLine($"namespace {options.Namespace}");
            _codeBuilder.AppendLine("{");

            using (_codeBuilder.Indent())
            {
                GenerateMainCode(options);
            }

            _codeBuilder.AppendLine("}");
            var content = _codeBuilder.ToString();

            return content;
        }

        private void GenerateImports(SourceOption options)
        {
            foreach (var import in options.Imports)
            {
                _codeBuilder.AppendLine($"using {import};");
            }

            _codeBuilder.AppendLine();
        }

        private void GenerateMainCode(SourceOption options)
        {

            if (!string.IsNullOrWhiteSpace(options.BaseClass) || options.BaseInterfaces?.Count > 0)
            {
                _codeBuilder.AppendLine($"public partial {options.CodeType.ToString("G").ToLower()} {options.Name} :");
                using (_codeBuilder.Indent())
                {
                    if (!string.IsNullOrWhiteSpace(options.BaseClass))
                    {
                        _codeBuilder.AppendLine($"{options.BaseClass},");
                    }

                    if (options.BaseInterfaces?.Count > 0)
                    {
                        foreach (var baseInterface in options.BaseInterfaces)
                        {
                            _codeBuilder.AppendLine($"{baseInterface},");
                        }
                    }

                    const string stringToRemoved = @",
";
                    _codeBuilder.Remove(_codeBuilder.Length - stringToRemoved.Length, stringToRemoved.Length);
                    _codeBuilder.AppendLine();
                }
            }
            else
            {
                _codeBuilder.AppendLine($"public partial {options.CodeType.ToString("G").ToLower()} {options.Name}");
            }

            _codeBuilder.AppendLine("{");

            using (_codeBuilder.Indent())
            {
                GenerateProperties(options);
            }

            _codeBuilder.AppendLine("}");
        }

        private void GenerateProperties(SourceOption options)
        {
            foreach (var property in options.Properties)
            {
                foreach (var attribute in property.Attributes)
                {
                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append("[");
                    stringBuilder.Append(attribute.Name);
                    if (attribute.Parameters?.Length > 0)
                    {
                        const string delimiter = ", ";
                        stringBuilder.Append("(");
                        foreach (var parameter in attribute.Parameters)
                        {
                            stringBuilder.Append(parameter);
                            stringBuilder.Append(delimiter);
                        }

                        stringBuilder.Remove(stringBuilder.Length - delimiter.Length, delimiter.Length);
                        stringBuilder.Append(")");
                    }

                    stringBuilder.Append("]");
                    _codeBuilder.AppendLine(stringBuilder.ToString());
                }

                var getStatement = string.IsNullOrWhiteSpace(property.GetAccessLevel)
                    ? "get;"
                    : $"{property.GetAccessLevel} get;";
                var setStatement = string.IsNullOrWhiteSpace(property.SetAccessLevel)
                    ? "set;"
                    : $"{property.SetAccessLevel} set;";
                _codeBuilder.AppendLine($"{property.AccessLevel} {property.Type} {property.Name} " + "{ " + $"{getStatement} {setStatement} " + "}");
                _codeBuilder.AppendLine();
            }
        }
    }
}