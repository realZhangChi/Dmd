using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dmd.Designer.Services.SolutionProfiles
{
    public class SolutionProfileFactory
    {
        private Lazy<Task<IJSObjectReference>> _jsTask;

        public SolutionProfileFactory(IJSRuntime jsRuntime)
        {
            _jsTask = new Lazy<Task<IJSObjectReference>>(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./js/fs.js").AsTask());
        }
    }
}
