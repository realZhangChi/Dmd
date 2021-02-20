using System.Threading.Tasks;
using Dmd.CodeGenerator.CodeWriters;
using Dmd.CodeGenerator.Options;

namespace Dmd.CodeGenerator.Generators
{
    public sealed class EntityGenerator : IEntityGenerator
    {
        private readonly IndentedCodeBuilder _codeBuilder;

        public EntityGenerator(IndentedCodeBuilder codeBuilder)
        {
            _codeBuilder = codeBuilder;
        }

        public Task Generate(CodeGeneratorOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}