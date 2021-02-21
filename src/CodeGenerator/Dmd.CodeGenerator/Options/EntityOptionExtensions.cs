using System;
using System.Collections.Generic;
using System.Text;

namespace Dmd.CodeGenerator.Options
{
    public static class EntityOptionExtensions
    {
        public static EntityOptions GetEntityOptions(
            this CodeGeneratorOptions codeGeneratorOption)
        {
            return new EntityOptions(codeGeneratorOption);
        }
    }
}
