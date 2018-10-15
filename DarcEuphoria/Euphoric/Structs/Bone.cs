namespace DarcEuphoria.Euphoric.Structs
{
    public struct Bone
    {
        public double BoneFov;
        public int TargBone;

        public Bone(double fov, int b)
        {
            BoneFov = fov;
            TargBone = b;
        }
    }
}