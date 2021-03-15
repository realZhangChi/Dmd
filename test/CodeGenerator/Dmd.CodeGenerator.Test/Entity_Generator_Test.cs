using System.Collections.Generic;
using System.Text.Json;
using Shouldly;
using Xunit;

namespace Dmd.CodeGenerator.Test
{
    public class Entity_Generator_Test : DmdCodeGeneratorTestBase
    {
        [Fact]
        public void Shoud_Exists_Genereted_Entity()
        {

            //HelloWorldGenerated.HelloWorld.SayHello();
            //var option = new ClassOptions()
            //{
            //    Name = "TestClass",
            //    Namespace = "Generated.Test",
            //    Properties = new List<PropertyOptions>()
            //    {
            //        new()
            //        {
            //            Name = "Property1",
            //            Type = "int"
            //        },
            //        new()
            //        {
            //            Name = "Property2",
            //            Type = "string"
            //        }
            //    }
            //};

            //var json = JsonSerializer.Serialize(option);
            ////json.ShouldNotBeNullOrWhiteSpace();

            //var generator = new Generators.CodeGenerator(new IndentedCodeBuilder());
            //var code = await generator.Generate(option);
            //code.ShouldNotBeNullOrWhiteSpace();
            ////var testEntity = new TestEntity();
        }
        
    }
}
