namespace Dmd.Designer.Components.Canvas
{
    public static class DmdCanvasContextExtensions
    {
        public static DmdCanvasContext Context(this DmdCanvasComponent component)
        {
            return new DmdCanvasContext(component);
        }
    }
}
