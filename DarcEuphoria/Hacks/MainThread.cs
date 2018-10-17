using System.Threading;
using System.Windows.Forms;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.CSGO.Entity;
using DarcEuphoria.Euphoric.Enums;
using DarcEuphoria.Hacks.Injection;
using DarcEuphoria.Hacks.Visuals;
using SharpDX.DirectWrite;
using static DarcEuphoria.Euphoric.CSGO.CSGOEngine;

namespace DarcEuphoria.Hacks
{
    public class MainThread
    {
        private static bool onceSniper;
        private static bool onceMouse = true;
        private static bool onceThird;
        private static bool onceRank = false;
        private static bool loadedMap = false;

        public static void Start()
        {
            VMaths.txtForm.TextAlignment = TextAlignment.Center;
            VMaths.txtForm.WordWrapping = WordWrapping.NoWrap;
            GlobalVariables.textFormat.WordWrapping = WordWrapping.NoWrap;

            #region starting

            ClientCmd.Exec(
                string.Format("bind mouse1 +attack; " +
                              "clear; " +
                              "echo ###{0} v{1} Started Successfully###", GlobalVariables.CHEATNAME,
                    GlobalVariables.CHEATVERSION.ToString($"F{1}")));
            ClanChanger.Set(string.Empty);
            csClient.ForceFullUpdate();

            #endregion

            while (GlobalVariables.IsActive)
            {
                GlobalVariables.GlobalRefresh++;

                if (!csClient.InGame)
                {
                    csClient.SendPackets = true;
                    onceRank = false;
                    VisualMain.Start(GlobalVariables.Device);
                    loadedMap = false;
                    Thread.Sleep(1000);
                    continue;
                }

                if (LocalPlayer.ActiveWeapon.IsSniper() && !onceSniper)
                {
                    ClientCmd.Exec("unbind mouse1");
                    onceSniper = true;
                }
                else if (!LocalPlayer.ActiveWeapon.IsSniper() && onceSniper)
                {
                    ClientCmd.Exec("bind mouse1 +attack");
                    onceSniper = false;
                }

                GlobalVariables.PlayerList = BasePlayer.PlayerList;
                GlobalVariables.EntityList = BaseWeapon.EntityList;

                #region BSP
                if (!loadedMap)
                {
                    var MapPath = string.Format(@"{0}\csgo\maps\{1}.bsp",
                        GlobalVariables.CSGO.Modules[0].FileName
                            .Substring(0, GlobalVariables.CSGO.Modules[0].FileName.Length - 9),
                        csClient.MapName);

                    if (!csClient.InGame)
                        LoadBSPFile("null");

                    if (bspMap == null)
                        LoadBSPFile(MapPath);
                    else if (bspMap.FileName != MapPath)
                        LoadBSPFile(MapPath);

                    loadedMap = true;
                }
                #endregion

                #region LocalPlayer

                if (!(LocalPlayer.Flags.Value >= 250 && LocalPlayer.Flags.Value <= 270) &&
                    !(LocalPlayer.Flags.Value >= 1280 && LocalPlayer.Flags.Value <= 1290))
                    LocalPlayer = new BaseLocalPlayer();

                #endregion

                if (GlobalVariables.InMenu)
                {
                    csClient.PostProcessDisabled.Value =
                        GlobalVariables.ActiveSettings.MiscSettings.NoPostProcessing;

                    LocalPlayer.FlashAlpha.Value =
                        (float)GlobalVariables.ActiveSettings.MiscSettings.FlashAlpha;
                }

                if (GlobalVariables.ActiveSettings.MiscSettings.FakeLag)
                    FakeLag.Start();
                else
                    csClient.SendPackets = true;

                if (GlobalVariables.ActiveSettings.AimbotSettings.Active)
                    Aimbot.Start();

                if (GlobalVariables.ActiveSettings.TriggerbotSettings.Active)
                    Triggerbot.Start();

                if (GlobalVariables.ActiveSettings.MiscSettings.AutoHop)
                    AutoHop.Start();

                VisualMain.Start(GlobalVariables.Device);

                #region FOV

                if (LocalPlayer.DefaultFOV.Value != (int)GlobalVariables.ActiveSettings.MiscSettings.FieldOfView ||
                    LocalPlayer.ActiveWeapon.WeaponID.Value == 8 ||
                    LocalPlayer.ActiveWeapon.WeaponID.Value == 39)
                {
                    if (LocalPlayer.ActiveWeapon.WeaponID.Value != 8 &&
                        LocalPlayer.ActiveWeapon.WeaponID.Value != 39)
                    {
                        if (LocalPlayer.ActiveWeapon.ZoomLevel.Value == 0)
                            LocalPlayer.DefaultFOV.Value =
                                (int)GlobalVariables.ActiveSettings.MiscSettings.FieldOfView;
                        else if (LocalPlayer.ActiveWeapon.ZoomLevel.Value == 1)
                            LocalPlayer.DefaultFOV.Value = 40;
                        else if (LocalPlayer.ActiveWeapon.ZoomLevel.Value == 2)
                            LocalPlayer.DefaultFOV.Value = 10;
                    }
                    else
                    {
                        if (LocalPlayer.ActiveWeapon.ZoomLevel.Value == 0)
                        {
                            if (LocalPlayer.DefaultFOV.Value !=
                                (int)GlobalVariables.ActiveSettings.MiscSettings.FieldOfView)
                                LocalPlayer.DefaultFOV.Value =
                                    (int)GlobalVariables.ActiveSettings.MiscSettings.FieldOfView;
                        }
                        else if (LocalPlayer.DefaultFOV.Value != 90)
                        {
                            LocalPlayer.DefaultFOV.Value = 90;
                        }
                    }
                }

                if (LocalPlayer.FOV.Value != (int)GlobalVariables.ActiveSettings.MiscSettings.FieldOfView ||
                    !LocalPlayer.ActiveWeapon.AbleToFire)
                {
                    if (LocalPlayer.ActiveWeapon.WeaponID.Value != 8 &&
                        LocalPlayer.ActiveWeapon.WeaponID.Value != 39)
                    {
                        if (LocalPlayer.ActiveWeapon.ZoomLevel.Value == 0 || !LocalPlayer.ActiveWeapon.AbleToFire)
                        {
                            LocalPlayer.FOV.Value = (int)GlobalVariables.ActiveSettings.MiscSettings.FieldOfView;
                            LocalPlayer.DefaultFOV.Value =
                                (int)GlobalVariables.ActiveSettings.MiscSettings.FieldOfView;
                        }
                        else if (LocalPlayer.ActiveWeapon.ZoomLevel.Value == 1)
                        {
                            LocalPlayer.FOV.Value = 40;
                        }
                        else if (LocalPlayer.ActiveWeapon.ZoomLevel.Value == 2)
                        {
                            LocalPlayer.FOV.Value = 10;
                        }
                    }
                    else
                    {
                        LocalPlayer.FOV.Value = (int)GlobalVariables.ActiveSettings.MiscSettings.FieldOfView;
                    }
                }

                if (LocalPlayer.Scoped.Value && LocalPlayer.ActiveWeapon.IsSniper())
                    LocalPlayer.DrawViewModel.Value = false;
                else if (!LocalPlayer.DrawViewModel.Value)
                    LocalPlayer.DrawViewModel.Value = true;

                #endregion

                LocalPlayer.ThirdPerson = GlobalVariables.ActiveSettings.MiscSettings.ThirdPerson;
                //ThirdPerson.Start();

                if (GlobalVariables.ActiveSettings.MiscSettings.ClanChanger)
                    ClanChanger.Start();
                else ClanChanger.Set(string.Empty);

                if (GlobalVariables.ActiveSettings.MiscSettings.ChatSpam)
                    ChatSpam.Start();

                if (1.IsKeyDown() && LocalPlayer.ActiveWeapon.IsSniper()
                                  && !GlobalVariables.InMenu)
                    LocalPlayer.Attack();

                if (1.IsKeyDown() && LocalPlayer.ThirdPerson)
                    LocalPlayer.Attack();

                if (GlobalVariables.InMenu && onceMouse)
                {
                    ClientCmd.Exec("unbind mouse1; unbind mouse2");
                    onceMouse = false;
                }
                else if (!GlobalVariables.InMenu && !onceMouse)
                {
                    ClientCmd.Exec("bind mouse1 +attack; bind mouse2 +attack2");
                    onceMouse = true;
                }

                //SkinChanger.Start();

                if (GlobalVariables.ActiveSettings.MiscSettings.RankRevealer)
                {
                    if (!onceRank)
                    {
                        onceRank = true;
                        RankRevealer.Start();
                    }
                }
                else
                {
                    onceRank = false;
                }

                Thread.Sleep(10);
            }

            GlobalVariables.SHUTDOWN = true;
            Memory.CloseCheat();
        }
    }
}