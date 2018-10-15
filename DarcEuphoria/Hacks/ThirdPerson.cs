using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.CSGO;
using static DarcEuphoria.Euphoric.CSGO.CSGOEngine;

namespace DarcEuphoria.Hacks
{
    public static class ThirdPerson
    {
        //public static Thread THREADthirdpersonThread = new Thread(Start);

        public static void Start()
        {
            while (GlobalVariables.IsActive)
            {
                if (!csClient.InGame)
                    continue;

                Memory.Write(Memory.Client.Base + Offsets.dwInput + 0xA5,
                    GlobalVariables.ActiveSettings.MiscSettings.ThirdPerson);


                // Memory.Write(Memory.Client.Base + Offsets.dwInput + 0xA8,
                //new Vector3(LocalPlayer.ViewAngles.Y, LocalPlayer.ViewAngles.X,
                //GlobalVariables.ActiveSettings.MiscSettings.ThirdPerson ? 150 : 0));
            }
        }
    }
}