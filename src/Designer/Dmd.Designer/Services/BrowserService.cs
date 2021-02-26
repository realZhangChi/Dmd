using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Dmd.Designer.Services
{
    public class BrowserService : IBrowserService
    {
        private readonly IJSRuntime _js;

        public BrowserService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<BrowserDimension> GetDimensionsAsync()
        {
            return await _js.InvokeAsync<BrowserDimension>("getDimensions");
        }

    }

    public class BrowserDimension
    {
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
