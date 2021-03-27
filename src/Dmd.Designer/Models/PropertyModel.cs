using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dmd.Designer.Models
{
    public class PropertyModel
    {
        public string AccessLevel { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string GetAccessLevel { get; set; }

        public string SetAccessLevel { get; set; }

        // TODO: set value asynchronously https://stackoverflow.com/questions/6602244/how-to-call-an-async-method-from-a-getter-or-setter
        private const string CollapseHide = "collapse";
        private const string CollapseHiding = "collapsing";
        private const string CollapseShowing = "collapsing show";
        private const string CollapseShow = "collapse show";
        public string CollapseStyle => Collapse ? CollapseShow : CollapseHide;
        private const string CollapseHiddenIconStyle = "fa-angle-right";
        private const string CollapseShowIconStyle = "fa-angle-down";
        public string CollapseIconStyle => Collapse ? CollapseShowIconStyle : CollapseHiddenIconStyle;
        public bool Collapse { get; set; }
    }
}
