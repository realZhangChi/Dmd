﻿using System;
using System.Text;

namespace Dmd.CodeGenerator.CodeBuilders
{
    public class IndentedCodeBuilder
    {
        private const string IndentString = "    ";

        private StringBuilder _stringBuilder;

        private int _indent;

        public int Length => _stringBuilder.Length;

        public IndentedCodeBuilder()
        {
            _stringBuilder = new StringBuilder();
            _indent = 0;
        }

        public IndentedCodeBuilder AppendLine(string value)
        {
            _stringBuilder.AppendLine(GetIndentString() + value);
            return this;
        }

        public IndentedCodeBuilder AppendLine()
        {
            _stringBuilder.AppendLine();
            return this;
        }

        public IndentedCodeBuilder Remove(int startIndex, int length)
        {
            _stringBuilder.Remove(startIndex, length);
            return this;
        }

        private IndentedCodeBuilder IncrementIndent()
        {
            _indent++;
            return this;
        }

        private IndentedCodeBuilder DecrementIndent()
        {
            _indent--;
            return this;
        }

        public override string ToString()
        {
            var code = _stringBuilder.ToString();
            _stringBuilder.Clear();
            return code;
        }

        public IDisposable Indent()
            => new Indenter(this);

        private sealed class Indenter : IDisposable
        {
            private readonly IndentedCodeBuilder _codeBuilder;

            public Indenter(IndentedCodeBuilder stringBuilder)
            {
                _codeBuilder = stringBuilder;

                _codeBuilder.IncrementIndent();
            }

            public void Dispose()
                => _codeBuilder.DecrementIndent();
        }

        private string GetIndentString()
        {
            var indentString = string.Empty;
            for (var i = 0; i < _indent; i++)
            {
                indentString += IndentString;
            }

            return indentString;
        }
    }
}
