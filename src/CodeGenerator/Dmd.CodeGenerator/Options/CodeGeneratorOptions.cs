using System.Collections.Generic;

namespace Dmd.CodeGenerator.Options
{
    public abstract class CodeGeneratorOptions
    {

        public string Namespace { get; set; }

        public string Directory { get; set; }

        public string Name { get; set; }

        public string BaseClass { get; set; }

        public ClassType ClassType { get; set; }

        public ICollection<PropertyOptions> Properties { get; set; }

    }
}