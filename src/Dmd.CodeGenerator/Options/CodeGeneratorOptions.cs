using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Volo.Abp;

namespace Dmd.CodeGenerator.Options
{
    public abstract class CodeGeneratorOptions
    {
        private readonly Dictionary<string, object> _properties;

        protected CodeGeneratorOptions()
        {
            _properties = new Dictionary<string, object>();
        }

        [CanBeNull]
        public T GetOrDefault<T>(string name, T defaultValue = default)
        {
            return (T)GetOrNull(name, defaultValue);
        }

        private object GetOrNull(string name, object defaultValue = null)
        {
            return _properties.GetOrDefault(name) ??
                   defaultValue;
        }

        [NotNull]
        public CodeGeneratorOptions Set([NotNull] string name, [CanBeNull] object value)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(value, nameof(value));

            _properties[name] = value;

            return this;
        }
    }
}
