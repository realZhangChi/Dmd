using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Dmd.CodeGenerator
{
    [DependsOn(
        typeof(AbpAutofacModule)
        )]
    public class DmdCodeGeneratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<StringWriter>();
        }
    }
}