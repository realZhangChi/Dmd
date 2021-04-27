using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dmd.Designer.Services.Project
{
    public interface IProjectService
    {
        Task EnsurePropsReferenceAsync(string projectPath);
    }
}
