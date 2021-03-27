using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dmd.Designer.Pages.Designer
{
    public class ModalSaveClickEventArgs
    {
        public ModalSaveClickEventArgs(object data)
        {
            Data = data;
        }

        public object Data { get; protected set; }
    }
}
