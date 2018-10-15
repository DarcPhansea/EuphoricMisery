using System.Threading;
using System.Threading.Tasks;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.Configs.Structs;
using DarcEuphoria.Euphoric.CSGO.Entity;
using DarcEuphoria.Euphoric.Enums;
using static DarcEuphoria.Euphoric.CSGO.CSGOEngine;

namespace DarcEuphoria.Hacks
{
    public class Triggerbot
    {
        public static TriggerbotWeaponCfgs WeaponCfg;

        private static Task ThreadTask;

        private static void LoadTriggerbotCfg()
        {
            if (LocalPlayer.ActiveWeapon.IsPistol())
                WeaponCfg = GlobalVariables.ActiveSettings.TriggerbotSettings.PistolCfg;
            else if (LocalPlayer.ActiveWeapon.IsSmg())
                WeaponCfg = GlobalVariables.ActiveSettings.TriggerbotSettings.SmgCfg;
            else if (LocalPlayer.ActiveWeapon.IsRifle())
                WeaponCfg = GlobalVariables.ActiveSettings.TriggerbotSettings.RifleCfg;
            else if (LocalPlayer.ActiveWeapon.IsShotgun())
                WeaponCfg = GlobalVariables.ActiveSettings.TriggerbotSettings.ShotgunCfg;
            else if (LocalPlayer.ActiveWeapon.IsLmg())
                WeaponCfg = GlobalVariables.ActiveSettings.TriggerbotSettings.LmgCfg;
            else if (LocalPlayer.ActiveWeapon.IsSniper())
                WeaponCfg = GlobalVariables.ActiveSettings.TriggerbotSettings.SniperCfg;
        }

        public static void Start()
        {
            if (ThreadTask != null)
                if (!ThreadTask.IsCompleted)
                    return;

            ThreadTask = Task.Factory.StartNew(() =>
            {
                if (GlobalVariables.InMenu) return;

                if (GlobalVariables.ActiveSettings.TriggerbotSettings.KnifeBot &&
                    LocalPlayer.ActiveWeapon.IsKnife() && CanKnife())
                    goto KnifeBot;

                if (LocalPlayer.ActiveWeapon.IsKnife()) return;


                WeaponCfg = new TriggerbotWeaponCfgs();

                LoadTriggerbotCfg();

                if (!WeaponCfg.Enabled) return;


                if (GlobalVariables.ActiveSettings.TriggerbotSettings.Key.IsKeyDown() &&
                    GlobalVariables.ActiveSettings.TriggerbotSettings.Key != 0) return;

                if (WeaponCfg.OnGroundCheck && !LocalPlayer.OnGround()) return;

                if (WeaponCfg.FlashedCheck && LocalPlayer.IsFlashed()) return;


                if (LocalPlayer.CrosshairID <= 0 ||
                    LocalPlayer.CrosshairID >= 65) return;

                var target = new BasePlayer(LocalPlayer.CrosshairID);

                if (target.IsDormant.Value) return;

                if (target.IsSameTeam()) return;

                if (target.LifeState != LifeState.Alive) return;

                if (target.Health.Value <= 0) return;

                Thread.Sleep((int) WeaponCfg.Delay);


                LocalPlayer.Attack();

                return;

                KnifeBot:

                LocalPlayer.Attack2();
            });
        }

        private static bool CanKnife()
        {
            foreach (var player in GlobalVariables.PlayerList)
            {
                if (player.IsLocalPlayer())
                    continue;

                if (player.IsDormant.Value) continue;
                if (player.LifeState != LifeState.Alive ||
                    player.Health.Value <= 0) continue;
                if (player.IsSameTeam()) continue;

                if (80 < Maths.Vector3Distance(LocalPlayer.Position.Value, player.Position.Value)) continue;

                var newAimAngle = Maths.CalcAngle(LocalPlayer.EyeLevel, player.BonePosition(4));

                var fov = Maths.CalculateFOV(LocalPlayer.ViewAngles, newAimAngle);

                if (fov > 70)
                    fov = Maths.CalculateFOV(LocalPlayer.ViewAngles.Normalize(),
                        newAimAngle.Normalize());

                if (fov > 70) continue;

                return true;
            }

            return false;
        }
    }
}