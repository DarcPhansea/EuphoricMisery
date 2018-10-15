using System;
using System.Threading.Tasks;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.CSGO;

namespace DarcEuphoria.Hacks
{
    public class FakeLag
    {
        private static int _lastLag;

        private static Task ThreadTask;

        public static void Start()
        {
            if (ThreadTask != null)
                if (!ThreadTask.IsCompleted)
                    return;

            ThreadTask = Task.Factory.StartNew(() =>
            {
                var endLag = _lastLag + GlobalVariables.ActiveSettings.MiscSettings.FakeLagAmount;

                if (endLag > Environment.TickCount)
                {
                    CSGOEngine.csClient.SendPackets = false;
                    return;
                }

                if (endLag + 15 > Environment.TickCount)
                {
                    CSGOEngine.csClient.SendPackets = true;
                    return;
                }

                _lastLag = Environment.TickCount;
            });
        }
    }
}