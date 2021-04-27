using System.Collections.Generic;
using Dmd.CodeGenerator.Options;

namespace Dmd.CodeGenerator.Generators
{
    public interface ICodeGenerator
    {
        string Generate(List<SourceOption> options);
    }
}