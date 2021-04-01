using Dmd.SourceOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dmd.Designer.Models
{
    public class EntityModel : ClassOption
    {
        //public string Name { get; set; }

        public new IList<PropertyModel> Properties { get; set; }

        public EntityModel()
        {
            Properties = new List<PropertyModel>();
        }

    }
}
