namespace Dmd.CodeGenerator.Options
{
    public abstract class ClassOptions
    {
        private readonly CodeGeneratorOptions _codeGeneratorOptions;

        public CodeGeneratorOptions Options => _codeGeneratorOptions;

        public string Namespace
        {
            get => _codeGeneratorOptions.GetOrDefault<string>(ClassOptionNames.Namespace);
            set => _codeGeneratorOptions.Set(ClassOptionNames.Namespace, value);
        }

        public string Directory
        {
            get => _codeGeneratorOptions.GetOrDefault<string>(ClassOptionNames.Directory);
            set => _codeGeneratorOptions.Set(ClassOptionNames.Directory, value);
        }

        public string Name
        {
            get => _codeGeneratorOptions.GetOrDefault<string>(ClassOptionNames.Name);
            set => _codeGeneratorOptions.Set(ClassOptionNames.Name, value);
        }

        public string BaseClass
        {
            get => _codeGeneratorOptions.GetOrDefault<string>(ClassOptionNames.BaseClass);
            set => _codeGeneratorOptions.Set(ClassOptionNames.BaseClass, value);
        }

        protected ClassOptions(CodeGeneratorOptions codeGeneratorOptions)
        {
            _codeGeneratorOptions = codeGeneratorOptions;
        }
    }
}