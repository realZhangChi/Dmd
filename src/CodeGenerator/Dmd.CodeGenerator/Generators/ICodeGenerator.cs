using System.CodeDom.Compiler;
using System.Threading.Tasks;
using Dmd.CodeGenerator.Options;

namespace Dmd.CodeGenerator.Generators
{
    public interface ICodeGenerator
    {
        Task Generate(Options.CodeGeneratorOptions options);
    }
}