using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dmd.Designer.Models
{
    public class DirectoryModel
    {
        public string Path { get; set; }

        public string Name { get; set; }

        public IEnumerable<DirectoryModel> Children { get; set; }
    }
}
