using Dmd.SourceOptions;

namespace Dmd.CodeGenerator.Generators
{
    public interface ICodeGenerator
    {
        string Generate(SourceOption options);
    }
}