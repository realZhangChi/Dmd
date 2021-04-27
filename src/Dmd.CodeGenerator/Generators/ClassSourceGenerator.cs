using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            try
            {
                var profile = context.AdditionalFiles.FirstOrDefault(f => f.Path.EndsWith("entity.json"));
                if (profile is null)
                    return;
                var options = JsonConvert.DeserializeObject<List<SourceOption>>(profile.GetText()!.ToString());
                if (options is null)
                    return;

                var codeGenerator = new CodeGenerator();
                var source = codeGenerator.Generate(options);
                context.AddSource($"dmd_Entity.cs", SourceText.From(source, Encoding.UTF8));
            }
            catch (Exception e)
            {
#if DEBUG
                if (!Debugger.IsAttached)
                {
                    Console.WriteLine(e);
                    Debugger.Launch();
                }
#endif
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
