using System.Collections.Generic;

namespace Dmd.CodeGenerator.Options
{
    public class PropertyOptions
    {
        public string Name { get; set; }
        
        public string Type { get; set; }

        public ICollection<AttributeOptions> Attributes { get; set; }

        public PropertyOptions()
        {
            Attributes = new List<AttributeOptions>();
        }
    }
}
