using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Modularity;
using Volo.Abp;

namespace Dmd.CodeGenerator.Test
{
    [DependsOn(
        typeof(AbpTestBaseModule)
        )]
    public class DmdCodeGeneratorTestModule : AbpModule
    {
    }
}
