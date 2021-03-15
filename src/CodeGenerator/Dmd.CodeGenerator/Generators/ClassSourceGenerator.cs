using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dmd.CodeGenerator.Options;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;

namespace Dmd.CodeGenerator.Generators
{
    [Generator]
    public class ClassSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var codeGenerator = new CodeGenerator();
            var profile = context.AdditionalFiles.FirstOrDefault(f => f.Path.EndsWith("dmd.json"));
            if (profile is null) 
                return;
            var options = JsonConvert.DeserializeObject<ClassOptions>(profile.GetText()!.ToString());
            if (options is null) 
                return;

            var source = codeGenerator.Generate(options);
            context.AddSource($"{options.Name}_dmd.cs", SourceText.From(source, Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
