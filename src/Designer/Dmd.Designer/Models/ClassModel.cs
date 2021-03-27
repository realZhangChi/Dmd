using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dmd.Designer.Models
{
    public class ClassModel
    {
        public string Name { get; set; }

        public List<string> Properties { get; set; }

        public string Methods { get; set; }

        public ClassModel()
        {
            Properties = new List<string>();
        }
    }
}
