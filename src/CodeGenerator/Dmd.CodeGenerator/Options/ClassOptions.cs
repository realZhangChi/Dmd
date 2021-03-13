using System;
using System.Collections.Generic;
using System.Text;

namespace Dmd.CodeGenerator.Options
{
    public class ClassOptions : CodeGeneratorOptions
    {
        public ClassOptions()
        {
            ClassType = ClassType.Class;
        }
    }
}
