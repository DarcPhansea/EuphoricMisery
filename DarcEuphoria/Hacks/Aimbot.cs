using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.Configs.Enums;
using DarcEuphoria.Euphoric.Configs.Structs;
using DarcEuphoria.Euphoric.CSGO.Entity;
using DarcEuphoria.Euphoric.Enums;
using DarcEuphoria.Euphoric.Structs;
using static DarcEuphoria.Euphoric.CSGO.CSGOEngine;

namespace DarcEuphoria.Hacks
{
    public class Aimbot
    {
        private static readonly Random rnd = new Random();

        public static AimbotWeaponCfgs WeaponCfg;

        private static BasePlayer ClosestStored = new BasePlayer(-1);
        private static int BoneStored = -1;

        private static Task ThreadTask;

        private static int prevShot;
        private static Vector2 oldAngle;

        private static BasePlayer ClosestPlayer
        {
            get
            {
                var playerIndex = -1;
                var lowestDistance = double.MaxValue;
                var lowestFov = double.MaxValue;
                var lowestHealth = double.MaxValue;

                if (!GlobalVariables.ActiveSettings.AimbotSettings.Key.IsKeyDown())
                    return new BasePlayer(playerIndex);

                foreach (var player in GlobalVariables.PlayerList)
                {
                    if (player.IsLocalPlayer())
                        continue;

                    if (player.IsDormant.Value) continue;
                    if (player.LifeState != LifeState.Alive) continue;
                    if (player.Health.Value <= 0) continue;
                    if (player.IsSameTeam()) continue;
                    if (!player.IsVisible && WeaponCfg.VisibilityCheck) continue;
                    if (!player.SpottedByMask.Value && WeaponCfg.SpottedCheck) continue;

                    var fov = FovFromBone(player, (int) TargetBones.Head);

                    if (fov == int.MaxValue) continue;

                    if (WeaponCfg.PriorityMode == PriorityMode.Fov)
                    {
                        if (lowestFov < fov) continue;
                    }
                    else if (WeaponCfg.PriorityMode == PriorityMode.Distance)
                    {
                        if (lowestDistance < Maths.Vector3Distance(LocalPlayer.Position.Value, player.Position.Value))
                            continue;
                    }
                    else
                    {
                        if (lowestHealth < player.Health.Value) continue;
                    }

                    lowestDistance = Maths.Vector3Distance(LocalPlayer.Position.Value, player.Position.Value);
                    lowestHealth = player.Health.Value;
                    lowestFov = fov;
                    playerIndex = player.Index;
                }

                return new BasePlayer(playerIndex);
            }
        }

        private static void LoadAimbotCfg()
        {
            if (LocalPlayer.ActiveWeapon.IsPistol())
                WeaponCfg = GlobalVariables.ActiveSettings.AimbotSettings.PistolCfg;
            else if (LocalPlayer.ActiveWeapon.IsSmg())
                WeaponCfg = GlobalVariables.ActiveSettings.AimbotSettings.SmgCfg;
            else if (LocalPlayer.ActiveWeapon.IsRifle())
                WeaponCfg = GlobalVariables.ActiveSettings.AimbotSettings.RifleCfg;
            else if (LocalPlayer.ActiveWeapon.IsShotgun())
                WeaponCfg = GlobalVariables.ActiveSettings.AimbotSettings.ShotgunCfg;
            else if (LocalPlayer.ActiveWeapon.IsLmg())
                WeaponCfg = GlobalVariables.ActiveSettings.AimbotSettings.LmgCfg;
            else if (LocalPlayer.ActiveWeapon.IsSniper())
                WeaponCfg = GlobalVariables.ActiveSettings.AimbotSettings.SniperCfg;
        }

        public static void Start()
        {
            if (ThreadTask != null)
                if (!ThreadTask.IsCompleted)
                    return;

            ThreadTask = Task.Factory.StartNew(() =>
            {
                if (GlobalVariables.InMenu) return;

                if (!LocalPlayer.ActiveWeapon.AbleToFire) return;

                WeaponCfg = new AimbotWeaponCfgs();

                LoadAimbotCfg();

                if (!WeaponCfg.Enabled) return;

                Thread.Sleep((int) WeaponCfg.Delay);

                if (WeaponCfg.OnGroundCheck && !LocalPlayer.OnGround()) return;

                using (var Closest = ClosestPlayer)
                {
                    if (Closest.Index == -1)
                    {
                        ClosestStored = new BasePlayer(-1);
                        BoneStored = -1;
                        if (WeaponCfg.RcsStandalone) LocalPlayer.ViewAngles = StandaloneRcs();
                    }
                    else
                    {
                        if (WeaponCfg.FlashedCheck && LocalPlayer.IsFlashed()) return;

                        oldAngle.X = (float) (LocalPlayer.AimPunchAngle.Value.X * (WeaponCfg.Rcs * 2) / 100);
                        oldAngle.Y = (float) (LocalPlayer.AimPunchAngle.Value.Y * (WeaponCfg.Rcs * 2) / 100);

                        Vector2 ang;

                        if (Closest.Index == ClosestStored.Index && ClosestStored.Index != -1)
                            ang = CalculateAimAngle(LocalPlayer.EyeLevel, ClosestStored, true, BoneStored).ClampAngle();
                        else
                            ang = CalculateAimAngle(LocalPlayer.EyeLevel, Closest, true).ClampAngle();

                        LocalPlayer.ViewAngles = ang;

                        if (WeaponCfg.AutoShoot)
                        {
                            Vector2 val1, val2;
                            if (Closest.Index == ClosestStored.Index && ClosestStored.Index != -1)
                            {
                                val1 = CalculateAimAngle(LocalPlayer.EyeLevel, ClosestStored, true, BoneStored);
                                val2 = CalculateAimAngle(LocalPlayer.EyeLevel, ClosestStored, false, BoneStored);
                            }
                            else
                            {
                                val1 = CalculateAimAngle(LocalPlayer.EyeLevel, Closest, true);
                                val2 = CalculateAimAngle(LocalPlayer.EyeLevel, Closest);
                            }

                            if (-2 < val1.X - val2.X && val1.X - val2.X < 2)
                                if (-2 < val1.Y - val2.Y && val1.Y - val2.Y < 2)
                                    LocalPlayer.Attack();
                            return;
                        }
                    }
                }

                if (LocalPlayer.ActiveWeapon.IsPistol() && WeaponCfg.AutoPistol)
                    if (WinApi.GetAsyncKeyState(1) > 0 || (WinApi.GetAsyncKeyState(1) & 0x8000) > 0)
                        if (LocalPlayer.ActiveWeapon.WeaponID.Value != 64)
                            LocalPlayer.Attack();
            });
        }

        private static float RandomizeAim(double amount)
        {
            var val = rnd.NextDouble();
            val -= 0.5;
            val *= amount;
            return (float) val;
        }

        private static double FovFromBone(BasePlayer player, int Bone)
        {
            var newAimAngle = Maths.CalcAngle(LocalPlayer.EyeLevel, player.BonePosition(Bone));
            var FOV = Maths.CalculateFOV(LocalPlayer.ViewAndPunch, newAimAngle);

            if (FOV > WeaponCfg.Fov)
                FOV = Maths.CalculateFOV(LocalPlayer.ViewAndPunch.Normalize(), newAimAngle.Normalize());


            if (FOV > WeaponCfg.Fov) return int.MaxValue;

            return FOV;
        }

        private static int ClosestBonePos(BasePlayer player)
        {
            var PotentialBones = new List<Bone>();

            if (WeaponCfg.FilterHead)
                PotentialBones.Add(new Bone(
                    FovFromBone(player, (int) TargetBones.Head),
                    (int) TargetBones.Head));
            if (WeaponCfg.FilterNeck)
                PotentialBones.Add(new Bone(
                    FovFromBone(player, (int) TargetBones.Neck),
                    (int) TargetBones.Neck));
            if (WeaponCfg.FilterChest)
                PotentialBones.Add(new Bone(
                    FovFromBone(player, (int) TargetBones.Chest),
                    (int) TargetBones.Chest));
            if (WeaponCfg.FilterStomach)
                PotentialBones.Add(new Bone(
                    FovFromBone(player, (int) TargetBones.Stomach),
                    (int) TargetBones.Stomach));
            if (WeaponCfg.FilterGroin)
                PotentialBones.Add(new Bone(
                    FovFromBone(player, (int) TargetBones.Groin),
                    (int) TargetBones.Groin));

            if (PotentialBones.ToArray().Length == 0) return -1;

            var sorted = PotentialBones.OrderBy(bone => bone.BoneFov);

            if (sorted.ToArray()[0].BoneFov == int.MaxValue)
                return -1;

            return sorted.ToArray()[0].TargBone;
        }

        private static Vector2 CalculateAimAngle(Vector3 src, BasePlayer target, bool Smoothing = false,
            int BoneOverride = -1)
        {
            var newAimAngle = Vector2.Zero;

            var targetBone = ClosestBonePos(target);
            if (targetBone == -1) goto NoTargetBone;

            if (BoneOverride != -1) targetBone = BoneOverride;

            BoneStored = BoneOverride;
            ClosestStored = target;

            var targetBonePosition = target.BonePosition(targetBone);

            targetBonePosition += target.VectorVelocity.Value * csClient.GlobalVarsBase.Value.interval_per_tick;

            var rando = new Vector3
            {
                X = RandomizeAim(WeaponCfg.Randomize),
                Y = RandomizeAim(WeaponCfg.Randomize),
                Z = RandomizeAim(WeaponCfg.Randomize)
            };

            targetBonePosition += rando;
            var srcEye = src;
            srcEye += LocalPlayer.VectorVelocity.Value * csClient.GlobalVarsBase.Value.interval_per_tick;

            var delta = targetBonePosition - srcEye;
            var magn = delta.Length;

            newAimAngle = new Vector2
            {
                X = (float) Math.Atan2(delta.Y, delta.X),
                Y = (float) -Math.Atan2(delta.Z, magn)
            };

            NoTargetBone:
            if (targetBone == -1)
                return LocalPlayer.ViewAngles;

            newAimAngle *= 180f / 3.14f;

            newAimAngle.X -= (float) (LocalPlayer.AimPunchAngle.Value.X * (WeaponCfg.Rcs * 2) / 100);
            newAimAngle.Y -= (float) (LocalPlayer.AimPunchAngle.Value.Y * (WeaponCfg.Rcs * 2) / 100);

            if (Smoothing)
            {
                var LocalViewAngle = LocalPlayer.ViewAngles;

                var fov1 = Maths.CalculateFOV(LocalViewAngle, newAimAngle);
                var fov2 = Maths.CalculateFOV(LocalViewAngle.Normalize(), newAimAngle.Normalize());

                if (fov1 < fov2)
                {
                    if (WeaponCfg.Smooth > 0)
                    {
                        newAimAngle.X = (newAimAngle - LocalViewAngle).X /
                                        (float) WeaponCfg.Smooth + LocalViewAngle.X;

                        newAimAngle.Y = (newAimAngle - LocalViewAngle).Y /
                                        (float) WeaponCfg.Smooth + LocalViewAngle.Y;
                    }
                }
                else
                {
                    if (WeaponCfg.Smooth > 0)
                    {
                        newAimAngle.X = (newAimAngle.Normalize() - LocalViewAngle.Normalize()).X /
                                        (float) WeaponCfg.Smooth + LocalViewAngle.Normalize().X;

                        newAimAngle.Y = (newAimAngle.Normalize() - LocalViewAngle.Normalize()).Y /
                                        (float) WeaponCfg.Smooth + LocalViewAngle.Normalize().Y;
                    }
                }
            }

            return newAimAngle.ClampAngle();
        }

        private static Vector2 StandaloneRcs()
        {
            if (LocalPlayer.ShotsFired.Value > 2 && LocalPlayer.ShotsFired.Value > prevShot)
            {
                prevShot = LocalPlayer.ShotsFired.Value;

                var viewAngle = LocalPlayer.ViewAngles + oldAngle;

                viewAngle.X -= (float) (LocalPlayer.AimPunchAngle.Value.X * (WeaponCfg.Rcs * 2) / 100);
                viewAngle.Y -= (float) (LocalPlayer.AimPunchAngle.Value.Y * (WeaponCfg.Rcs * 2) / 100);

                oldAngle.X = (float) (LocalPlayer.AimPunchAngle.Value.X * (WeaponCfg.Rcs * 2) / 100);
                oldAngle.Y = (float) (LocalPlayer.AimPunchAngle.Value.Y * (WeaponCfg.Rcs * 2) / 100);

                return viewAngle.ClampAngle();
            }

            if (LocalPlayer.ShotsFired.Value < 1)
            {
                oldAngle.X = 0;
                oldAngle.Y = 0;
                prevShot = 0;
                return LocalPlayer.ViewAngles;
            }

            return LocalPlayer.ViewAngles;
        }
    }
}