using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dmd.Designer.Models
{
    public class SolutionTreeNodeModel
    {
        public string Path { get; set; }

        public string Name { get; set; }

        public IEnumerable<SolutionTreeNodeModel> Children { get; set; }

        public SolutionTreeNodeModel()
        {
            Children = new List<SolutionTreeNodeModel>();
        }
    }
}
