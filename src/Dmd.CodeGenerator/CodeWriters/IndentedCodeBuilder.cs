using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Dmd.CodeGenerator.CodeWriters
{
    public class IndentedCodeBuilder : ITransientDependency
    {
        private readonly StringWriter _stringWriter;

        private readonly IndentedTextWriter _indentedTextWriter;

        public IndentedCodeBuilder()
        {
            _stringWriter = new StringWriter();
            _indentedTextWriter = new IndentedTextWriter(_stringWriter);
        }

        public async Task<IndentedCodeBuilder> AppendLineAsync([NotNull] string value)
        {
            await _indentedTextWriter.WriteLineAsync(value);
            return this;
        }

        public async Task<IndentedCodeBuilder> AppendLineAsync()
        {
            await _indentedTextWriter.WriteLineAsync(string.Empty);
            return this;
        }

        public IndentedCodeBuilder IncrementIndent()
        {
            _indentedTextWriter.Indent++;
            return this;
        }

        public IndentedCodeBuilder DecrementIndent()
        {
            _indentedTextWriter.Indent--;
            return this;
        }

        public override string ToString()
        {
            return _stringWriter.ToString();
        }

        public IDisposable Indent()
            => new Indenter(this);

        private sealed class Indenter : IDisposable
        {
            private readonly IndentedCodeBuilder _stringBuilder;

            public Indenter(IndentedCodeBuilder stringBuilder)
            {
                _stringBuilder = stringBuilder;

                _stringBuilder.IncrementIndent();
            }

            public void Dispose()
                => _stringBuilder.DecrementIndent();
        }
    }
}
