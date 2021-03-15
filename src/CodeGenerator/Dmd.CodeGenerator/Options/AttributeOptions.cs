using System;
using System.Collections.Generic;
using System.Text;

namespace Dmd.CodeGenerator.Options
{
    public class AttributeOptions
    {
        public string Name { get; set; }

        public object[] Parameters { get; set; }

        public AttributeOptions()
        {
            Parameters = new object[] { };
        }
    }
}
