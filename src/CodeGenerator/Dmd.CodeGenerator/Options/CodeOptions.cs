using System;
using System.Collections.Generic;

namespace Dmd.CodeGenerator.Options
{
    public abstract class CodeOptions
    {

        public string Namespace { get; set; }

        public string Directory { get; set; }

        public string Name { get; set; }

        public string BaseClass { get; set; }

        public ICollection<string> BaseInterfaces { get; set; }

        public ClassType ClassType { get; set; }

        public ICollection<PropertyOptions> Properties { get; set; }

        protected CodeOptions()
        {
            BaseInterfaces = new List<string>();
            Properties = new List<PropertyOptions>();
        }

    }
}