namespace DarcEuphoria.Euphoric.Configs.Structs
{
    public struct SkinData
    {
        public int PaintKit;
        public int Seed;
        public float Wear;
        public int StatTrak;
        public string Name;

        public SkinData(
            int paint = 0,
            int seed = 0,
            float wear = 0.0001f,
            int stat = -1,
            string nam = "")
        {
            if (wear < 0.0001f)
                wear = 0.0001f;

            PaintKit = paint;
            Seed = seed;
            Wear = wear;
            StatTrak = stat;
            Name = nam;
        }
    }
}