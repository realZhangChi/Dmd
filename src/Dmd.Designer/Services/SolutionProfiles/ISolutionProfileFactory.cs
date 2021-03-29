using System.Threading.Tasks;

namespace Dmd.Designer.Services.SolutionProfiles
{
    public interface ISolutionProfileFactory
    {
        Task<SolutionProfile> CreateAsync(string absolutePath);
    }
}
