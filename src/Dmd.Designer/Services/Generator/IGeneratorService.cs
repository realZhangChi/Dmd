using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmd.SourceOptions;

namespace Dmd.Designer.Services.Generator
{
    public interface IGeneratorService
    {
        Task GenerateEntityJsonAsync(string key);
    }
}
