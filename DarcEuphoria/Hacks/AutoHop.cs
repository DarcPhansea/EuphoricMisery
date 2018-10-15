using System;
using System.Threading.Tasks;
using DarcEuphoria.Euphoric;
using static DarcEuphoria.Euphoric.CSGO.CSGOEngine;

namespace DarcEuphoria.Hacks
{
    public class AutoHop
    {
        private static bool _failedJump;
        private static readonly Random _rnd = new Random();

        private static Task ThreadTask;

        public static void Start()
        {
            if (ThreadTask != null)
                if (!ThreadTask.IsCompleted)
                    return;

            ThreadTask = Task.Factory.StartNew(() =>
            {
                if (GlobalVariables.InMenu) return;

                if (LocalPlayer.VectorVelocity.Value.X == 0 &&
                    LocalPlayer.VectorVelocity.Value.Y == 0 &&
                    LocalPlayer.VectorVelocity.Value.Z == 0) return;

                if (!32.IsKeyDown())
                {
                    _failedJump = false;
                    return;
                }

                if (!LocalPlayer.OnGround()) return;

                var r = _rnd.Next(0, 100);

                if (r > GlobalVariables.ActiveSettings.MiscSettings.AutoHopChance)
                    _failedJump = true;

                if (_failedJump)
                    return;

                csClient.SendPackets = true;
                LocalPlayer.Jump();
            });
        }
    }
}