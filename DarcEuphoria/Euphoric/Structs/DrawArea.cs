namespace DarcEuphoria.Euphoric.Structs
{
    public struct DrawArea
    {
        public float Left, Top;
        public float Width, Height;

        public DrawArea(float left, float top, float width, float height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }
    }
}