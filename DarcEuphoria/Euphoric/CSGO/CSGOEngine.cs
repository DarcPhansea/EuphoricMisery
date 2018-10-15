using System.IO;
using DarcEuphoria.Euphoric.BspParsing;
using DarcEuphoria.Euphoric.CSGO.Entity;

namespace DarcEuphoria.Euphoric.CSGO
{
    public static class CSGOEngine
    {
        public static CSGOClient csClient;
        public static BaseLocalPlayer LocalPlayer;
        public static BSPFile bspMap;


        public static void LoadBSPFile(string MapPath)
        {
            if (MapPath == "null")
            {
                bspMap = new BSPFile(MapPath);
                return;
            }

            if (!csClient.InGame)
                return;

            if (File.Exists(MapPath)) bspMap = new BSPFile(MapPath);
        }
    }
}