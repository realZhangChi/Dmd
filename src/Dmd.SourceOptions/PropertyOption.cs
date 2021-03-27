using System.Collections.Generic;

namespace Dmd.SourceOptions
{
    public class PropertyOption
    {
        public string AccessLevel { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }
        
        public string GetAccessLevel { get; set; }

        public string SetAccessLevel { get; set; }

        public ICollection<AttributeOption> Attributes { get; set; }

        public PropertyOption()
        {
            Attributes = new List<AttributeOption>();
        }
    }
}
