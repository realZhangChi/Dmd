using System.Collections.Generic;
using System.Text.Json;
using Dmd.CodeGenerator.Options;
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
            var option = new ClassOptions()
            {
                Name = "TestClass",
                Namespace = "Generated.Test",
                Properties = new List<PropertyOptions>()
                {
                    new()
                    {
                        Name = "Property1",
                        Type = "int",
                        Attributes = new List<AttributeOptions>()
                        {
                            new ()
                            {
                                Name = "System.ComponentModel.DataAnnotations.RequiredAttribute",
                            },
                            new ()
                            {
                                Name = "System.ComponentModel.DataAnnotations.StringLengthAttribute",
                                Parameters = new object[]{10}
                            }
                        }
                    },
                    new()
                    {
                        Name = "Property2",
                        Type = "string"
                    }
                }
            };

            var json = JsonSerializer.Serialize(option);
            json.ShouldNotBeNullOrWhiteSpace();

            var generator = new Generators.CodeGenerator();
            var code = generator.Generate(option);
            code.ShouldNotBeNullOrWhiteSpace();
            ////var testEntity = new TestEntity();
        }

    }
}
