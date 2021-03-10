using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CSharp;

namespace Dmd.CodeGenerator.CodeGenerators
{
    public class DmdCodeGenerator
    {
        private readonly CodeCompileUnit _codeCompileUnit;

        public DmdCodeGenerator()
        {
            _codeCompileUnit = new CodeCompileUnit();
        }

        public string GenerateClass()
        {
            var nameSpaceName = "Test";
            var nameSpaceImport = new[] { "System", "System.Linq" };
            var className = "Test";

            var nameSpace = new CodeNamespace(nameSpaceName);
            nameSpace.Imports.AddRange(nameSpaceImport.Select(import => new CodeNamespaceImport(import)).ToArray());

            var classDeclaration = new CodeTypeDeclaration(className)
            {
                IsPartial = true,
                IsClass = true
            };
            //classDeclaration.BaseTypes.Add()
            var familyProperty = new CodeMemberProperty()
            {
                Name = "FamilyProperty",
                Type = new CodeTypeReference(typeof(string)),
                Attributes = MemberAttributes.Family
            };
            classDeclaration.Members.Add(familyProperty);
            var familyAndAssemblyProperty = new CodeMemberProperty()
            {
                Name = "FamilyAndAssemblyProperty",
                Type = new CodeTypeReference(typeof(string)),
                Attributes = MemberAttributes.FamilyAndAssembly
            };
            classDeclaration.Members.Add(familyAndAssemblyProperty);
            var familyOrAssemblyProperty = new CodeMemberProperty()
            {
                Name = "FamilyOrAssemblyProperty",
                Type = new CodeTypeReference(typeof(string)),
                Attributes = MemberAttributes.FamilyOrAssembly
            };
            classDeclaration.Members.Add(familyOrAssemblyProperty);
            var privateProperty = new CodeMemberProperty()
            {
                Name = "PrivateProperty",
                Type = new CodeTypeReference(typeof(string)),
                Attributes = MemberAttributes.Private
            };
            classDeclaration.Members.Add(privateProperty);

            nameSpace.Types.Add((classDeclaration));
            _codeCompileUnit.Namespaces.Add(nameSpace);
            var provider = new CSharpCodeProvider();
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter);
            provider.GenerateCodeFromCompileUnit(_codeCompileUnit, indentedTextWriter, new CodeGeneratorOptions());
            indentedTextWriter.Close();
            var code = stringWriter.ToString();
            return code;
        }

        public CodeCompileUnit Compile()
        {
            var code = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace Dmd.CodeGenerator.Options
{
    public static class EntityOptionExtensions
    {
        public static EntityOptions GetEntityOptions(
            this CodeGeneratorOptions codeGeneratorOption)
        {
            return new EntityOptions(codeGeneratorOption);
        }
    }
}
";
            var reader = new StringReader(code);
            var provider = new CSharpCodeProvider();
            var unit = provider.Parse(reader);
            return unit;
        }

        private CodeNamespace GenerateNameSpace(string nameSpaceName)
        {
            return new CodeNamespace(nameSpaceName);
        }

        //private CodeNamespace AddIm
    }
}
