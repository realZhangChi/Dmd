using System.Threading.Tasks;
using Dmd.CodeGenerator.Options;

namespace Dmd.CodeGenerator.Generators
{
    public interface ICodeGenerator
    {
        string Generate(CodeGeneratorOptions options);
    }
}