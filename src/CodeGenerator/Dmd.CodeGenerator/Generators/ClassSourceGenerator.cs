using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Dmd.CodeGenerator.CodeWriters;
using Dmd.CodeGenerator.Options;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Dmd.CodeGenerator.Generators
{
    [Generator]
    public class ClassSourceGenerator : ISourceGenerator
    {
        private readonly ICodeGenerator _codeGenerator;

        private readonly ClassOptions _options;

        public ClassSourceGenerator()
        {
            _codeGenerator = new CodeGenerator(new IndentedCodeBuilder());
            _options = new ClassOptions
            {
                Name = "TestEntity",
                Namespace = "Generated",
                Directory = @"C:\Users\Chi\source\repos\Dmd\Generated",
                Properties = new List<PropertyOptions>()
                {
                    new PropertyOptions() {Name = "Property1", Type = "string"},
                    new PropertyOptions() {Name = "Property2", Type = "int"}
                }
            };
        }

        public void Execute(GeneratorExecutionContext context)
        {
            StringBuilder sourceBuilder = new StringBuilder(@"
using System;
namespace HelloWorldGenerated
{
    public static class HelloWorld
    {
        public static void SayHello() 
        {
            Console.WriteLine(""Hello from generated code!"");
            Console.WriteLine(""The following syntax trees existed in the compilation that created this program:"");
");

            // using the context, get a list of syntax trees in the users compilation
            IEnumerable<SyntaxTree> syntaxTrees = context.Compilation.SyntaxTrees;

            // add the filepath of each tree to the class we're building
            foreach (SyntaxTree tree in syntaxTrees)
            {
                sourceBuilder.AppendLine($@"Console.WriteLine(@"" - {tree.FilePath}"");");
            }

            foreach (var additionalText in context.AdditionalFiles)
            {
                sourceBuilder.AppendLine($@"Console.WriteLine(@"" - {additionalText.Path}"");");
            }

            // finish creating the source to inject
            sourceBuilder.Append(@"
        }
    }
}");

            // inject the created source into the users compilation
            context.AddSource("helloWorldGenerated", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
