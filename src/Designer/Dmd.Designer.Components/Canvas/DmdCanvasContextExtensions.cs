using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dmd.Designer.Components.Canvas
{
    public static class DmdCanvasContextExtensions
    {
        public static DmdCanvasContext Context(this DmdCanvasComponent component)
        {
            return new DmdCanvasContext(component);
        }
    }
}
