using Volo.Abp;
using Volo.Abp.Testing;

namespace Dmd.CodeGenerator.Test
{
    public class DmdCodeGeneratorTestBase : AbpIntegratedTest<DmdCodeGeneratorTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
