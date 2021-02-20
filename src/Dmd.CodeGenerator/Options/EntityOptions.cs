namespace Dmd.CodeGenerator.Options
{
    public class EntityOptions : ClassOptions
    {
        public EntityOptions()
            : base(new CodeGeneratorOptions())
        {

        }

        public EntityOptions(CodeGeneratorOptions options)
            : base(options)
        {

        }
    }
}