using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dmd.Designer.Services.File
{
    public interface IFileService
    {
        Task SaveAsync(string directory, string name, string content);

        Task<string> ReadAsync(string fullPath);
    }
}
