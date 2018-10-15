using DarcEuphoria.Euphoric.Configs.Enums;

namespace DarcEuphoria.Euphoric.Configs.Structs
{
    public struct AimbotWeaponCfgs
    {
        public bool Enabled;
        public bool SpottedCheck;
        public bool VisibilityCheck;
        public bool OnGroundCheck;
        public bool FlashedCheck;
        public bool AutoShoot;
        public bool AutoPistol;
        public bool RcsStandalone;
        public bool FilterHead;
        public bool FilterNeck;
        public bool FilterChest;
        public bool FilterStomach;
        public bool FilterGroin;
        public double Smooth;
        public double Randomize;
        public double Delay;
        public double Rcs;
        public double Fov;
        public PriorityMode PriorityMode;
    }
}