using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Dmd.CodeGenerator.Generators;
using Dmd.CodeGenerator.Options;
using Xunit;

namespace Dmd.CodeGenerator.Test
{
    public class Entity_Generator_Test : DmdCodeGeneratorTestBase
    {
        [Fact]
        public async void Shoud_Exists_Genereted_Entity()
        {
            var generator = GetRequiredService<IEntityGenerator>();
            var entityOptions = new EntityOptions();
            entityOptions.Name = "TestEntity";
            entityOptions.Namespace = "Generated";
            entityOptions.Directory = @"C:\Users\Chi\source\repos\Dmd\Generated";
            await generator.Generate(entityOptions.Options);
            var result = File.Exists(@"C:\Users\Chi\source\repos\Dmd\Generated\TestEntity.cs");
            result.ShouldBeTrue();
        }
    }
}
