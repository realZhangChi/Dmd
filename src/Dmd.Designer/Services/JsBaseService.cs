using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Dmd.Designer.Services
{
    public abstract class JsBaseService
    {
        protected Lazy<Task<IJSObjectReference>> JsTask { get; set; }
    }
}
