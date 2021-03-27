using System.Threading.Tasks;

namespace Dmd.Designer.Services
{
    public interface IBrowserService
    {
        Task<BrowserDimension> GetDimensionsAsync();
    }
}
