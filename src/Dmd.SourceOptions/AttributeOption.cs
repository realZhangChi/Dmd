namespace Dmd.SourceOptions
{
    public class AttributeOption
    {
        public string Name { get; set; }

        public object[] Parameters { get; set; }

        public AttributeOption()
        {
            Parameters = new object[] { };
        }
    }
}
