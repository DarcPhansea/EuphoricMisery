using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.Configs;
using DarcEuphoria.Euphoric.Configs.Enums;
using DarcEuphoria.Euphoric.Configs.Structs;
using DarcEuphoria.Euphoric.Controls;
using DarcEuphoria.Euphoric.CSGO;
using DarcEuphoria.Euphoric.CSGO.Entity;
using DarcEuphoria.Euphoric.Enums;
using DarcEuphoria.Euphoric.Structs;
using DarcEuphoria.Hacks;
using DarcEuphoria.Hacks.Injection;
using DarcEuphoria.Hacks.Visuals;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;

namespace DarcEuphoria
{
    public partial class WndOverlay : Form
    {
        private static int InitialStyle;
        private readonly List<Control> ControlList;
        private readonly Thread THREADmainThread = new Thread(MainThread.Start);

        private int AimWeapSelect;
        private bool Once;
        private int TriggerWeapSelect;
        private int VisualsPlayerSelect;

        public WndOverlay()
        {
            if (!Memory.IsValid)
            {
                MessageBox.Show("Something Went Wrong. Check if CS:GO is open.", "Oops");
                Memory.CloseHandle();
                Environment.Exit(Environment.ExitCode);
            }
            else
            {
                CSGOEngine.csClient = new CSGOClient();
                CSGOEngine.LocalPlayer = new BaseLocalPlayer();
            }

            InitializeComponent();

            CHEATMENU_CHEATS.Location = new Point(0, 2);
            InitialStyle = WinApi.GetWindowLong(Handle, -20);

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.Opaque |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);


            UnlockMouse();

            UpdateDevice();

            ControlList = GetControlHierarchy(this).ToList();

            CHEATMENU_CHEATS.DrawMode = TabDrawMode.OwnerDrawFixed;
            CHEATMENU_CHEATS.Region =
                new Region(new RectangleF(tp_Aimbot.Left, tp_Aimbot.Top, tp_Aimbot.Width, tp_Aimbot.Height));
        }

        public void UnlockMouse()
        {
            if (GlobalVariables.InMenu)
            {
                if (Bounds.Contains(Cursor.Position))
                    WinApi.SetFocus(Handle);

                WinApi.SetWindowLong(Handle, -20, InitialStyle);
            }
            else
            {
                WinApi.SetForegroundWindow(GlobalVariables.CSGO.MainWindowHandle);
                WinApi.SetFocus(GlobalVariables.CSGO.MainWindowHandle);

                WinApi.SetWindowLong(Handle, -20, InitialStyle | 0x20);
            }
        }

        public void UpdateDevice()
        {
            try
            {
                var rect = new RECT();
                WinApi.GetClientRect(GlobalVariables.CSGO.MainWindowHandle, out rect);

                var renderProp = new HwndRenderTargetProperties
                {
                    Hwnd = Handle,
                    PixelSize = new Size2(rect.Right, rect.Bottom),
                    PresentOptions = PresentOptions.Immediately
                };

                GlobalVariables.Device = new WindowRenderTarget(
                    new Factory(),
                    new RenderTargetProperties(
                        new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)
                    ),
                    renderProp);
            }
            catch
            {
            }
        }

        private void UpdateConfigs()
        {
            if (CHEATMENU_CHEATS.SelectedIndex == 6)
            {
                CONFIG_LIST.Items.Clear();
                foreach (var file in Directory.GetFiles(Application.StartupPath + @"\EMConfigs"))
                {
                    if (!file.EndsWith(".dmcfg")) continue;

                    var fileName = file.Substring((Application.StartupPath + @"\EMConfigs").Length + 1);
                    fileName = fileName.Replace(".dmcfg", "");
                    CONFIG_LIST.Items.Add(fileName);
                }
            }
        }

        private void UpdateColors()
        {
            GlobalVariables.PrimaryForeColor = Color.FromArgb(255, GlobalVariables.PrimaryForeColor);
            GlobalVariables.PrimaryBackColor = Color.FromArgb(255, GlobalVariables.PrimaryBackColor);
            GlobalVariables.SecondaryBackColor = Color.FromArgb(255, GlobalVariables.SecondaryBackColor);

            GlobalVariables.ActiveSettings.PrimaryForeColor = GlobalVariables.PrimaryForeColor.ToRaw();
            GlobalVariables.ActiveSettings.PrimaryBackColor = GlobalVariables.PrimaryBackColor.ToRaw();
            GlobalVariables.ActiveSettings.SecondaryBackColor = GlobalVariables.SecondaryBackColor.ToRaw();
            GlobalVariables.ActiveSettings.PrimaryTextColor = GlobalVariables.PrimaryTextColor.ToRaw();

            for (var i = 0; i < 7; i++)
                CHEATMENU_CHEATS.TabPages[i].BackColor = GlobalVariables.PrimaryBackColor;

            panel_TopBar.BackColor = GlobalVariables.SecondaryBackColor;
            CONFIG_LIST.BackColor = GlobalVariables.SecondaryBackColor;
            label25.BackColor = GlobalVariables.SecondaryBackColor;
            skins_SkinSearch.BackColor = GlobalVariables.SecondaryBackColor;
            WeaponName.BackColor = GlobalVariables.SecondaryBackColor;
            StatNum.BackColor = GlobalVariables.SecondaryBackColor;
            SeedNum.BackColor = GlobalVariables.SecondaryBackColor;
            Misc_ClanChanger_Tag.BackColor = GlobalVariables.SecondaryBackColor;
            Misc_NameChanger_Tag.BackColor = GlobalVariables.SecondaryBackColor;


            panel_TopBar.ForeColor = GlobalVariables.PrimaryTextColor;
            CONFIG_LIST.ForeColor = GlobalVariables.PrimaryTextColor;
            skins_SkinSearch.ForeColor = GlobalVariables.PrimaryTextColor;
            WeaponName.ForeColor = GlobalVariables.PrimaryTextColor;
            StatNum.ForeColor = GlobalVariables.PrimaryTextColor;
            SeedNum.ForeColor = GlobalVariables.PrimaryTextColor;
            Misc_ClanChanger_Tag.ForeColor = GlobalVariables.PrimaryTextColor;
            Misc_NameChanger_Tag.ForeColor = GlobalVariables.PrimaryTextColor;

            label1.ForeColor = GlobalVariables.PrimaryTextColor;
            label2.ForeColor = GlobalVariables.PrimaryTextColor;
            label3.ForeColor = GlobalVariables.PrimaryTextColor;
            label4.ForeColor = GlobalVariables.PrimaryTextColor;
            label5.ForeColor = GlobalVariables.PrimaryTextColor;
            label6.ForeColor = GlobalVariables.PrimaryTextColor;
            label7.ForeColor = GlobalVariables.PrimaryTextColor;
            label8.ForeColor = GlobalVariables.PrimaryTextColor;
            label9.ForeColor = GlobalVariables.PrimaryTextColor;
            label10.ForeColor = GlobalVariables.PrimaryTextColor;
            label11.ForeColor = GlobalVariables.PrimaryTextColor;
            label12.ForeColor = GlobalVariables.PrimaryTextColor;
            label13.ForeColor = GlobalVariables.PrimaryTextColor;
            label14.ForeColor = GlobalVariables.PrimaryTextColor;
            label15.ForeColor = GlobalVariables.PrimaryTextColor;
            label16.ForeColor = GlobalVariables.PrimaryTextColor;
            label17.ForeColor = GlobalVariables.PrimaryTextColor;
            label18.ForeColor = GlobalVariables.PrimaryTextColor;
            label19.ForeColor = GlobalVariables.PrimaryTextColor;
            label20.ForeColor = GlobalVariables.PrimaryTextColor;
            label21.ForeColor = GlobalVariables.PrimaryTextColor;
            label22.ForeColor = GlobalVariables.PrimaryTextColor;
            label23.ForeColor = GlobalVariables.PrimaryTextColor;
            label26.ForeColor = GlobalVariables.PrimaryTextColor;

            CONFIGNAME.BackColor = GlobalVariables.SecondaryBackColor;
            CONFIGNAME.ForeColor = GlobalVariables.PrimaryTextColor;


            foreach (var control in ControlList) control.Invalidate();
        }

        private void WndOverlay_Load(object sender, EventArgs e)
        {
            for (var i = CHEATMENU_CHEATS.TabPages.Count; i > 0; i--)
                CHEATMENU_CHEATS.SelectedIndex = i;

            foreach (var control in ControlList)
            {
                control.TabStop = false;

                if (control is DeComboBox)
                    ((DeComboBox) control).SelectedIndex = 0;

                if (control is DeList)
                    if (((DeList) control).Items.Count > 0)
                        ((DeList) control).SelectedIndex = 0;
            }

            WinApi.SetParent(Handle, GlobalVariables.CSGO.MainWindowHandle);

            CHEATMENU_CHEATS.SelectedIndex = 0;

            gui_Thread.RunWorkerAsync();
            THREADmainThread.Start();
            //ThirdPerson.THREADthirdpersonThread.Start();
            SkinParser.SetSkinList(SKINSLIST);
            //SkinParser.Start();
            //for (var i = 0; i < SkinParser.SkinName.Length; i++) SKINSLIST.Items.Add(SkinParser.SkinName[i]);

            InitCheat();

            if (!Directory.Exists(Application.StartupPath + @"\EMConfigs"))
                Directory.CreateDirectory(Application.StartupPath + @"\EMConfigs");
        }

        private void gui_Thread_DoWork(object sender, DoWorkEventArgs e)
        {
            Invoke((MethodInvoker) delegate
            {
                BringToFront();
            });

            while (GlobalVariables.IsActive)
            {
                if (Process.GetProcessesByName("csgo").Length == 0) Memory.CloseCheat();

                Thread.Sleep(100);

                var rect = new RECT();
                WinApi.GetClientRect(GlobalVariables.CSGO.MainWindowHandle, out rect);

                Invoke((MethodInvoker) delegate
                {
                    Size = new Size(rect.Right, rect.Bottom);
                    GlobalVariables.ScreenSize = Size;

                    #region Menu

                    if (!Once)
                    {
                        CHEATMENU.Left = Width / 2 - CHEATMENU.Width / 2;
                        CHEATMENU.Top = Height / 2 - CHEATMENU.Height / 2;
                        Location = new Point(0, 0);
                        Once = true;
                    }

                    if ((WinApi.GetAsyncKeyState(0x2D) & 0x1) > 0)
                    {
                        GlobalVariables.InMenu = !GlobalVariables.InMenu;
                        UnlockMouse();
                    }

                    if (GlobalVariables.InMenu)
                    {
                        panel_TopBar.Visible = true;
                        CHEATMENU.Visible = true;

                        LOGO.Invalidate();

                        if (Process.GetProcessesByName("SnippingTool").Length <= 0)
                            if (Bounds.Contains(Cursor.Position))
                                WinApi.SetForegroundWindow(Handle);
                    }
                    else
                    {
                        panel_TopBar.Visible = false;
                        CHEATMENU.Visible = false;
                    }

                    #endregion

                    if (GlobalVariables.ActiveSettings.MiscSettings.ExternalRadar)
                    {

                        RADAR.SendToBack();

                        Radar.Center = new Point(
                            RADAR.Width / 2 + RADAR.Location.X,
                            RADAR.Height / 2 + RADAR.Location.Y);

                        RADAR.Visible = true;
                        RADAR.Width = RADAR.Height =
                            (int) GlobalVariables.ActiveSettings.MiscSettings.ExternalRadarSize * 4 + 1;


                    }
                    else
                    {
                        RADAR.Visible = false;
                    }

                    if (Aimbot_WeaponSelect.SelectedIndex != AimWeapSelect)
                    {
                        AimWeapSelect = Aimbot_WeaponSelect.SelectedIndex;

                        LoadAimbot();
                    }

                    if (Triggerbot_WeaponSelect.SelectedIndex != TriggerWeapSelect)
                    {
                        TriggerWeapSelect = Triggerbot_WeaponSelect.SelectedIndex;

                        LoadTriggerbot();
                    }

                    if (Visuals_PlayerSelect.SelectedIndex != VisualsPlayerSelect)
                    {
                        VisualsPlayerSelect = Visuals_PlayerSelect.SelectedIndex;

                        LoadVisuals();
                    }

                    if (!GlobalVariables.InMenu && GlobalVariables.ActiveSettings.MiscSettings.ThirdPersonKey != 0)
                    {
                        if ((WinApi.GetAsyncKeyState(GlobalVariables.ActiveSettings.MiscSettings.ThirdPersonKey) &
                             0x1) > 0)
                            GlobalVariables.ActiveSettings.MiscSettings.ThirdPerson =
                                !GlobalVariables.ActiveSettings.MiscSettings.ThirdPerson;

                        Misc_ThirdPerson.Checked = GlobalVariables.ActiveSettings.MiscSettings.ThirdPerson;
                    }
                    else
                    {
                        GlobalVariables.ActiveSettings.MiscSettings.ThirdPerson = Misc_ThirdPerson.Checked;
                    }

                    SaveCheat();
                });
            }
        }

        private IEnumerable<Control> GetControlHierarchy(Control root)
        {
            var queue = new Queue<Control>();

            queue.Enqueue(root);

            do
            {
                var control = queue.Dequeue();

                yield return control;

                foreach (var child in control.Controls.OfType<Control>())
                    queue.Enqueue(child);
            } while (queue.Count > 0);
        }

        private void CheatSelection(object sender, EventArgs e)
        {
            if (!((RadioButton) sender).Checked) return;

            CHEATMENU_CHEATS.SelectedTab.Hide();

            switch (((RadioButton) sender).Name)
            {
                case "Tab_Aimbot":
                    CHEATMENU_CHEATS.SelectedIndex = 0;
                    break;
                case "Tab_Triggerbot":
                    CHEATMENU_CHEATS.SelectedIndex = 1;
                    break;
                case "Tab_Visuals":
                    CHEATMENU_CHEATS.SelectedIndex = 2;
                    break;
                case "Tab_Misc":
                    CHEATMENU_CHEATS.SelectedIndex = 3;
                    break;
                case "Tab_SkinChanger":
                    CHEATMENU_CHEATS.SelectedIndex = 4;
                    break;
                case "Tab_Colors":
                    CHEATMENU_CHEATS.SelectedIndex = 5;
                    break;
                case "Tab_Configs":
                    CHEATMENU_CHEATS.SelectedIndex = 6;
                    break;
                default:
                    CHEATMENU_CHEATS.SelectedIndex = 0;
                    break;
            }

            CHEATMENU_CHEATS.SelectedTab.Show();

            UpdateConfigs();
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            CSGOEngine.csClient.ForceFullUpdate();
        }

        private void CONFIG_LIST_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seletedIndex = CONFIG_LIST.SelectedIndex;
            CONFIGNAME.Text = string.Empty;
            CONFIG_LIST.SelectedIndex = seletedIndex;
        }

        private void CONFIGNAME_TextChanged(object sender, EventArgs e)
        {
            CONFIG_LIST.SelectedIndex = -1;
        }

        private void cfg_CREATENEW_Click(object sender, EventArgs e)
        {
            if (CONFIGNAME.Text == string.Empty) return;

            Handler.Save<MainSettings>(GlobalVariables.ActiveSettings,
                Application.StartupPath + @"\EMConfigs\" + CONFIGNAME.Text + ".dmcfg");

            UpdateConfigs();
        }

        private void cfg_SAVE_Click(object sender, EventArgs e)
        {
            if (CONFIG_LIST.SelectedIndex == -1) return;
            Handler.Save<MainSettings>(GlobalVariables.ActiveSettings,
                Application.StartupPath + @"\EMConfigs\" + CONFIG_LIST.SelectedItem + ".dmcfg");

            UpdateConfigs();
        }

        private void cfg_LOAD_Click(object sender, EventArgs e)
        {
            if (CONFIG_LIST.SelectedIndex == -1) return;
            var newSettings = Handler.Load<MainSettings>(
                Application.StartupPath + @"\EMConfigs\" + CONFIG_LIST.SelectedItem + ".dmcfg");

            if (newSettings.ConfigVersion != GlobalVariables.CHEATVERSION)
            {
                GlobalVariables.InMenu = false;

                MessageBox.Show(
                    "This config is out of date!! We will still try to load the config but it may cause instabilities.", "WARNING - Old Config Version");
            }

            GlobalVariables.ActiveSettings = newSettings;
            GlobalVariables.ActiveSettings.ConfigVersion = GlobalVariables.CHEATVERSION;

            GlobalVariables.PrimaryForeColor = GlobalVariables.ActiveSettings.PrimaryForeColor.ToColor();
            GlobalVariables.PrimaryBackColor = GlobalVariables.ActiveSettings.PrimaryBackColor.ToColor();
            GlobalVariables.SecondaryBackColor = GlobalVariables.ActiveSettings.SecondaryBackColor.ToColor();
            GlobalVariables.PrimaryTextColor = GlobalVariables.ActiveSettings.PrimaryTextColor.ToColor();

            UpdateConfigs();
            UpdateColors();
            LoadCheat();
        }

        private void cfg_DELETE_Click(object sender, EventArgs e)
        {
            if (CONFIG_LIST.SelectedIndex == -1) return;
            File.Delete(Application.StartupPath + @"\EMConfigs\" + CONFIG_LIST.SelectedItem + ".dmcfg");
            UpdateConfigs();
        }

        private void deButton2_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + @"\EMConfigs\");
            GlobalVariables.InMenu = false;
        }

        #region Config Loading

        private void InitCheat()
        {
            GlobalVariables.ActiveSettings.ConfigVersion =
                GlobalVariables.CHEATVERSION;

            GlobalVariables.ActiveSettings.PrimaryForeColor = GlobalVariables.PrimaryForeColor.ToRaw();
            GlobalVariables.ActiveSettings.PrimaryBackColor = GlobalVariables.PrimaryBackColor.ToRaw();
            GlobalVariables.ActiveSettings.SecondaryBackColor = GlobalVariables.SecondaryBackColor.ToRaw();
            GlobalVariables.ActiveSettings.PrimaryTextColor = GlobalVariables.PrimaryTextColor.ToRaw();

            InitAimbot();
            LoadAimbot();

            InitTriggerbot();
            LoadTriggerbot();

            InitVisuals();
            LoadVisuals();

            InitMisc();
            LoadMisc();

            InitSkins();
            LoadSkins();
        }

        private void SaveCheat()
        {
            SaveAimbot();
            SaveTriggerbot();
            SaveVisuals();
            SaveMisc();
        }

        private void LoadCheat()
        {
            LoadAimbot();
            LoadTriggerbot();
            LoadMisc();
            LoadVisuals();
            LoadSkins();
        }

        #region Aimbot

        private void InitAimbot()
        {
            AimbotWeaponCfgs WeaponCfg;
            WeaponCfg.Enabled = false;
            WeaponCfg.VisibilityCheck = false;
            WeaponCfg.SpottedCheck = false;
            WeaponCfg.OnGroundCheck = false;
            WeaponCfg.FlashedCheck = false;
            WeaponCfg.AutoShoot = false;
            WeaponCfg.AutoPistol = false;
            WeaponCfg.FilterHead = true;
            WeaponCfg.FilterNeck = false;
            WeaponCfg.FilterChest = false;
            WeaponCfg.FilterStomach = false;
            WeaponCfg.FilterGroin = false;
            WeaponCfg.PriorityMode = 0;
            WeaponCfg.Fov = 0;
            WeaponCfg.Smooth = 0;
            WeaponCfg.Randomize = 0;
            WeaponCfg.Delay = 0;
            WeaponCfg.RcsStandalone = false;
            WeaponCfg.Rcs = 0;

            GlobalVariables.ActiveSettings.AimbotSettings.Active = false;
            GlobalVariables.ActiveSettings.AimbotSettings.Key = 0x1;
            GlobalVariables.ActiveSettings.AimbotSettings.PistolCfg = WeaponCfg;
            GlobalVariables.ActiveSettings.AimbotSettings.SmgCfg = WeaponCfg;
            GlobalVariables.ActiveSettings.AimbotSettings.RifleCfg = WeaponCfg;
            GlobalVariables.ActiveSettings.AimbotSettings.ShotgunCfg = WeaponCfg;
            GlobalVariables.ActiveSettings.AimbotSettings.LmgCfg = WeaponCfg;
            GlobalVariables.ActiveSettings.AimbotSettings.SniperCfg = WeaponCfg;
        }

        private void LoadAimbot()
        {
            var ActiveAimbot = GlobalVariables.ActiveSettings.AimbotSettings;
            AimbotWeaponCfgs WeaponCfg;

            switch (Aimbot_WeaponSelect.SelectedIndex)
            {
                case 0:
                    WeaponCfg = ActiveAimbot.PistolCfg;
                    break;
                case 1:
                    WeaponCfg = ActiveAimbot.SmgCfg;
                    break;
                case 2:
                    WeaponCfg = ActiveAimbot.RifleCfg;
                    break;
                case 3:
                    WeaponCfg = ActiveAimbot.ShotgunCfg;
                    break;
                case 4:
                    WeaponCfg = ActiveAimbot.LmgCfg;
                    break;
                case 5:
                    WeaponCfg = ActiveAimbot.SniperCfg;
                    break;
                default:
                    WeaponCfg = ActiveAimbot.PistolCfg;
                    break;
            }

            if (Aimbot_WeaponSelect.SelectedIndex == 0)
                Aimbot_AutoPst.Visible = true;
            else
                Aimbot_AutoPst.Visible = false;

            mt_Aimbot_Active.Checked = ActiveAimbot.Active;
            mt_Aimbot_Enable.Checked = WeaponCfg.Enabled;
            Aimbot_VisChk.Checked = WeaponCfg.VisibilityCheck;
            Aimbot_SptdChk.Checked = WeaponCfg.SpottedCheck;
            Aimbot_JmpChk.Checked = WeaponCfg.OnGroundCheck;
            Aimbot_FlshChk.Checked = WeaponCfg.FlashedCheck;
            Aimbot_AutoSht.Checked = WeaponCfg.AutoShoot;
            Aimbot_AutoPst.Checked = WeaponCfg.AutoPistol;
            Aimbot_Filter_Head.Checked = WeaponCfg.FilterHead;
            Aimbot_Filter_Neck.Checked = WeaponCfg.FilterNeck;
            Aimbot_Filter_Chest.Checked = WeaponCfg.FilterChest;
            Aimbot_Filter_Stomach.Checked = WeaponCfg.FilterStomach;
            Aimbot_Filter_Groin.Checked = WeaponCfg.FilterGroin;
            Aimbot_PriorityMode.SelectedIndex = (int) WeaponCfg.PriorityMode;
            Aimbot_Fov.Value = WeaponCfg.Fov;
            Aimbot_Smooth.Value = WeaponCfg.Smooth;
            Aimbot_Randomize.Value = WeaponCfg.Randomize;
            Aimbot_Delay.Value = WeaponCfg.Delay;
            Aimbot_RcsStandalone.Checked = WeaponCfg.RcsStandalone;
            Aimbot_Rcs.Value = WeaponCfg.Rcs;
        }

        private void SaveAimbot()
        {
            AimbotWeaponCfgs WeaponCfg;
            GlobalVariables.ActiveSettings.AimbotSettings.Active = mt_Aimbot_Active.Checked;
            WeaponCfg.Enabled = mt_Aimbot_Enable.Checked;
            WeaponCfg.VisibilityCheck = Aimbot_VisChk.Checked;
            WeaponCfg.SpottedCheck = Aimbot_SptdChk.Checked;
            WeaponCfg.OnGroundCheck = Aimbot_JmpChk.Checked;
            WeaponCfg.FlashedCheck = Aimbot_FlshChk.Checked;
            WeaponCfg.AutoShoot = Aimbot_AutoSht.Checked;
            WeaponCfg.AutoPistol = Aimbot_AutoPst.Checked;
            WeaponCfg.FilterHead = Aimbot_Filter_Head.Checked;
            WeaponCfg.FilterNeck = Aimbot_Filter_Neck.Checked;
            WeaponCfg.FilterChest = Aimbot_Filter_Chest.Checked;
            WeaponCfg.FilterStomach = Aimbot_Filter_Stomach.Checked;
            WeaponCfg.FilterGroin = Aimbot_Filter_Groin.Checked;
            WeaponCfg.PriorityMode = (PriorityMode) Aimbot_PriorityMode.SelectedIndex;
            WeaponCfg.Fov = Aimbot_Fov.Value;
            WeaponCfg.Smooth = Aimbot_Smooth.Value;
            WeaponCfg.Randomize = Aimbot_Randomize.Value;
            WeaponCfg.Delay = Aimbot_Delay.Value;
            WeaponCfg.RcsStandalone = Aimbot_RcsStandalone.Checked;
            WeaponCfg.Rcs = Aimbot_Rcs.Value;

            switch (Aimbot_WeaponSelect.SelectedIndex)
            {
                case 0:
                    GlobalVariables.ActiveSettings.AimbotSettings.PistolCfg = WeaponCfg;
                    break;
                case 1:
                    GlobalVariables.ActiveSettings.AimbotSettings.SmgCfg = WeaponCfg;
                    break;
                case 2:
                    GlobalVariables.ActiveSettings.AimbotSettings.RifleCfg = WeaponCfg;
                    break;
                case 3:
                    GlobalVariables.ActiveSettings.AimbotSettings.ShotgunCfg = WeaponCfg;
                    break;
                case 4:
                    GlobalVariables.ActiveSettings.AimbotSettings.LmgCfg = WeaponCfg;
                    break;
                case 5:
                    GlobalVariables.ActiveSettings.AimbotSettings.SniperCfg = WeaponCfg;
                    break;
                default:
                    GlobalVariables.ActiveSettings.AimbotSettings.PistolCfg = WeaponCfg;
                    break;
            }
        }

        #endregion

        #region Triggerbot

        private void InitTriggerbot()
        {
            TriggerbotWeaponCfgs WeaponCfg;
            WeaponCfg.Enabled = false;
            WeaponCfg.OnGroundCheck = false;
            WeaponCfg.FlashedCheck = false;
            WeaponCfg.Delay = 0;

            GlobalVariables.ActiveSettings.TriggerbotSettings.Key = 0x0;
            GlobalVariables.ActiveSettings.TriggerbotSettings.KnifeBot = false;
            GlobalVariables.ActiveSettings.TriggerbotSettings.PistolCfg = WeaponCfg;
            GlobalVariables.ActiveSettings.TriggerbotSettings.SmgCfg = WeaponCfg;
            GlobalVariables.ActiveSettings.TriggerbotSettings.RifleCfg = WeaponCfg;
            GlobalVariables.ActiveSettings.TriggerbotSettings.ShotgunCfg = WeaponCfg;
            GlobalVariables.ActiveSettings.TriggerbotSettings.LmgCfg = WeaponCfg;
            GlobalVariables.ActiveSettings.TriggerbotSettings.SniperCfg = WeaponCfg;
        }

        private void LoadTriggerbot()
        {
            var ActiveTrigger = GlobalVariables.ActiveSettings.TriggerbotSettings;
            mt_Triggerbot_Active.Checked = GlobalVariables.ActiveSettings.TriggerbotSettings.Active;
            Triggerbot_KnifeBot.Checked = GlobalVariables.ActiveSettings.TriggerbotSettings.KnifeBot;

            TriggerbotWeaponCfgs WeaponCfg;

            switch (Triggerbot_WeaponSelect.SelectedIndex)
            {
                case 0:
                    WeaponCfg = ActiveTrigger.PistolCfg;
                    break;
                case 1:
                    WeaponCfg = ActiveTrigger.SmgCfg;
                    break;
                case 2:
                    WeaponCfg = ActiveTrigger.RifleCfg;
                    break;
                case 3:
                    WeaponCfg = ActiveTrigger.ShotgunCfg;
                    break;
                case 4:
                    WeaponCfg = ActiveTrigger.LmgCfg;
                    break;
                case 5:
                    WeaponCfg = ActiveTrigger.SniperCfg;
                    break;
                default:
                    WeaponCfg = ActiveTrigger.PistolCfg;
                    break;
            }

            Triggerbot_Enabled.Checked = WeaponCfg.Enabled;
            Triggerbot_JmpChk.Checked = WeaponCfg.OnGroundCheck;
            Triggerbot_FlshChk.Checked = WeaponCfg.FlashedCheck;
            Triggerbot_Delay.Value = WeaponCfg.Delay;
        }

        private void SaveTriggerbot()
        {
            TriggerbotWeaponCfgs WeaponCfg;
            GlobalVariables.ActiveSettings.TriggerbotSettings.Active = mt_Triggerbot_Active.Checked;
            GlobalVariables.ActiveSettings.TriggerbotSettings.KnifeBot = Triggerbot_KnifeBot.Checked;
            WeaponCfg.Enabled = Triggerbot_Enabled.Checked;
            WeaponCfg.OnGroundCheck = Triggerbot_JmpChk.Checked;
            WeaponCfg.FlashedCheck = Triggerbot_FlshChk.Checked;
            WeaponCfg.Delay = Triggerbot_Delay.Value;

            switch (Triggerbot_WeaponSelect.SelectedIndex)
            {
                case 0:
                    GlobalVariables.ActiveSettings.TriggerbotSettings.PistolCfg = WeaponCfg;
                    break;
                case 1:
                    GlobalVariables.ActiveSettings.TriggerbotSettings.SmgCfg = WeaponCfg;
                    break;
                case 2:
                    GlobalVariables.ActiveSettings.TriggerbotSettings.RifleCfg = WeaponCfg;
                    break;
                case 3:
                    GlobalVariables.ActiveSettings.TriggerbotSettings.ShotgunCfg = WeaponCfg;
                    break;
                case 4:
                    GlobalVariables.ActiveSettings.TriggerbotSettings.LmgCfg = WeaponCfg;
                    break;
                case 5:
                    GlobalVariables.ActiveSettings.TriggerbotSettings.SniperCfg = WeaponCfg;
                    break;
                default:
                    GlobalVariables.ActiveSettings.TriggerbotSettings.PistolCfg = WeaponCfg;
                    break;
            }
        }

        #endregion

        #region Misc

        private void InitMisc()
        {
            MiscSettings miscSettings;
            miscSettings.InGameRadar = false;
            miscSettings.C4Countdown = false;
            miscSettings.NoPostProcessing = false;
            miscSettings.RankRevealer = false;
            miscSettings.ChatSpam = false;
            miscSettings.ThirdPerson = false;
            miscSettings.ThirdPersonKey = (int) VirtualKeys.None;
            miscSettings.AutoHop = false;
            miscSettings.AutoHopChance = 100;
            miscSettings.FakeLag = false;
            miscSettings.FakeLagAmount = 0;
            miscSettings.ExternalRadar = false;
            miscSettings.ExternalRadarScale = 10;
            miscSettings.ExternalRadarSize = 50;
            miscSettings.ClanChanger = false;
            miscSettings.ClanChangerMode = ClanMode.Off;
            miscSettings.ClanTag = string.Empty;
            miscSettings.NameChanger = false;
            miscSettings.NameTag = string.Empty;
            miscSettings.FlashAlpha = 255;
            miscSettings.FieldOfView = 90;
            miscSettings.ExternalRadarPosition = RADAR.Location;
            GlobalVariables.ActiveSettings.MiscSettings = miscSettings;
        }

        private void LoadMisc()
        {
            var miscSettings = GlobalVariables.ActiveSettings.MiscSettings;
            Misc_InGameRadar.Checked = miscSettings.InGameRadar;
            miscSettings.C4Countdown = Misc_C4_Countdown.Checked;
            Misc_PostProcessing.Checked = miscSettings.NoPostProcessing;
            Misc_RankRevealer.Checked = miscSettings.RankRevealer;
            Misc_ChatSpam.Checked = miscSettings.ChatSpam;
            Misc_ThirdPerson.Checked = miscSettings.ThirdPerson;
            Misc_AutoHop_Enable.Checked = miscSettings.AutoHop;
            Misc_AutoHop_Chance.Value = miscSettings.AutoHopChance;
            Misc_FakeLag_Enabled.Checked = miscSettings.FakeLag;
            Misc_FakeLag_Amount.Value = miscSettings.FakeLagAmount;
            Misc_ExternalRadar.Checked = miscSettings.ExternalRadar;
            Misc_ExternalRadar_Scale.Value = miscSettings.ExternalRadarScale;
            Misc_ExternalRadar_Size.Value = miscSettings.ExternalRadarSize;
            Misc_ClanChanger.Checked = miscSettings.ClanChanger;
            Misc_ClanMode.SelectedIndex = (int) miscSettings.ClanChangerMode;
            Misc_ClanChanger_Tag.Text = miscSettings.ClanTag;
            Misc_NameChanger.Checked = miscSettings.NameChanger;
            Misc_NameChanger_Tag.Text = miscSettings.NameTag;
            Misc_FlashAlpha.Value = miscSettings.FlashAlpha;
            Misc_FieldofView.Value = miscSettings.FieldOfView;
            Misc_ThirdPersonKey.Text = ((VirtualKeys) miscSettings.ThirdPersonKey).ToString();
            RADAR.Location = miscSettings.ExternalRadarPosition;
        }

        private void SaveMisc()
        {
            var miscSettings = new MiscSettings();
            miscSettings.InGameRadar = Misc_InGameRadar.Checked;
            miscSettings.C4Countdown = Misc_C4_Countdown.Checked;
            miscSettings.NoPostProcessing = Misc_PostProcessing.Checked;
            miscSettings.RankRevealer = Misc_RankRevealer.Checked;
            miscSettings.ChatSpam = Misc_ChatSpam.Checked;
            miscSettings.ThirdPerson = Misc_ThirdPerson.Checked;
            miscSettings.AutoHop = Misc_AutoHop_Enable.Checked;
            miscSettings.AutoHopChance = Misc_AutoHop_Chance.Value;
            miscSettings.FakeLag = Misc_FakeLag_Enabled.Checked;
            miscSettings.FakeLagAmount = Misc_FakeLag_Amount.Value;
            miscSettings.ExternalRadar = Misc_ExternalRadar.Checked;
            miscSettings.ExternalRadarScale = Misc_ExternalRadar_Scale.Value;
            miscSettings.ExternalRadarSize = Misc_ExternalRadar_Size.Value;
            miscSettings.ClanChanger = Misc_ClanChanger.Checked;
            miscSettings.ClanChangerMode = (ClanMode) Misc_ClanMode.SelectedIndex;
            miscSettings.ClanTag = Misc_ClanChanger_Tag.Text;
            miscSettings.NameChanger = Misc_NameChanger.Checked;
            miscSettings.NameTag = Misc_NameChanger_Tag.Text;
            miscSettings.FlashAlpha = Misc_FlashAlpha.Value;
            miscSettings.FieldOfView = Misc_FieldofView.Value;
            miscSettings.ExternalRadarPosition = RADAR.Location;
            miscSettings.ThirdPersonKey = (int) (VirtualKeys) Enum.Parse(typeof(VirtualKeys), Misc_ThirdPersonKey.Text);
            GlobalVariables.ActiveSettings.MiscSettings = miscSettings;
        }

        #endregion

        #region Visuals

        private void InitVisuals()
        {
            var visualSettings = new VisualSettings();
            var visualsPlayer = new VisualsPlayer();
            visualsPlayer.BoxMode = BoxMode.Off;
            visualsPlayer.BoxOutline = false;
            visualsPlayer.Enabled = false;
            visualsPlayer.GlowMode = GlowMode.Off;
            visualsPlayer.HeadSpot = false;
            visualsPlayer.HealthMode = HealthMode.Off;
            visualsPlayer.Name = false;
            visualsPlayer.Rank = false;
            visualsPlayer.Snaplines = false;
            visualsPlayer.VisableOnly = false;

            #region Colors Enemy

            visualsPlayer.vBoxColor = new RawColor4(1, 0, 0, 1);
            visualsPlayer.vGlowColor = new RawColor4(1, 0, 0, 1);
            visualsPlayer.vTextColor = new RawColor4(1, 1, 1, 1);
            visualsPlayer.vHeadSpotColor = new RawColor4(0, 1, 0, 1);
            visualsPlayer.vSnapLinesColor = new RawColor4(1, 0, 0, 1);

            visualsPlayer.hBoxColor = new RawColor4(1, 165 / 255f, 0, 1);
            visualsPlayer.hGlowColor = new RawColor4(1, 165 / 255f, 0, 1);
            visualsPlayer.hTextColor = new RawColor4(1, 1, 1, 1);
            visualsPlayer.hHeadSpotColor = new RawColor4(0, 1, 0, 1);
            visualsPlayer.hSnapLinesColor = new RawColor4(1, 165 / 255f, 0, 1);

            #endregion

            visualSettings.Enemy = visualsPlayer;

            #region Colors Team

            visualsPlayer.vBoxColor = new RawColor4(0, 0, 1, 1);
            visualsPlayer.vGlowColor = new RawColor4(0, 0, 1, 1);
            visualsPlayer.vTextColor = new RawColor4(1, 1, 1, 1);
            visualsPlayer.vHeadSpotColor = new RawColor4(0, 1, 0, 1);
            visualsPlayer.vSnapLinesColor = new RawColor4(0, 0, 1, 1);

            visualsPlayer.hBoxColor = new RawColor4(0, 165 / 255f, 1, 1);
            visualsPlayer.hGlowColor = new RawColor4(0, 165 / 255f, 1, 1);
            visualsPlayer.hTextColor = new RawColor4(1, 1, 1, 1);
            visualsPlayer.hHeadSpotColor = new RawColor4(0, 1, 0, 1);
            visualsPlayer.hSnapLinesColor = new RawColor4(0, 165 / 255f, 1, 1);

            #endregion

            visualSettings.Team = visualsPlayer;
            visualSettings.Yourself = visualsPlayer;

            visualSettings.Active = false;
            visualSettings.wWeapon = false;
            visualSettings.wDroppedC4 = false;
            visualSettings.wPlantedC4 = false;
            visualSettings.wDefuser = false;
            visualSettings.wGrenades = false;
            visualSettings.wHostage = false;
            visualSettings.wChicken = false;
            visualSettings.DrawAimbotFov = false;
            visualSettings.DrawRecoil = false;
            visualSettings.DrawSniper = false;
            visualSettings.NoHands = false;
            visualSettings.NoScope = false;
            GlobalVariables.ActiveSettings.VisualSettings = visualSettings;
        }

        private void LoadVisuals()
        {
            var visualSettings = GlobalVariables.ActiveSettings.VisualSettings;
            var visualsPlayer = new VisualsPlayer();

            Visuals_Active.Checked = visualSettings.Active;
            Visuals_wWeapon.Checked = visualSettings.wWeapon;
            Visuals_wDroppedC4.Checked = visualSettings.wDroppedC4;
            Visuals_wPlantedC4.Checked = visualSettings.wPlantedC4;
            Visuals_wDefuser.Checked = visualSettings.wDefuser;
            Visuals_wGrenades.Checked = visualSettings.wGrenades;
            Visuals_wHostage.Checked = visualSettings.wHostage;
            Visuals_wChicken.Checked = visualSettings.wChicken;
            Visuals_oAimbotFov.Checked = visualSettings.DrawAimbotFov;
            Visuals_oRecoil.Checked = visualSettings.DrawRecoil;
            Visuals_oSniper.Checked = visualSettings.DrawSniper;
            Visuals_oNoHands.Checked = visualSettings.NoHands;
            Visuals_oNoScope.Checked = visualSettings.NoScope;

            switch (Visuals_PlayerSelect.SelectedIndex)
            {
                case 0:
                    visualsPlayer = visualSettings.Enemy;
                    break;
                case 1:
                    visualsPlayer = visualSettings.Team;
                    break;
                case 2:
                    visualsPlayer = visualSettings.Yourself;
                    break;
            }

            Visuals_BoxMode.SelectedIndex = (int) visualsPlayer.BoxMode;
            Visuals_BoxOutline.Checked = visualsPlayer.BoxOutline;
            Visuals_Enable.Checked = visualsPlayer.Enabled;
            Visuals_Glow.SelectedIndex = (int) visualsPlayer.GlowMode;
            Visuals_HeadSpot.Checked = visualsPlayer.HeadSpot;
            Visuals_Health.SelectedIndex = (int) visualsPlayer.HealthMode;
            Visuals_Name.Checked = visualsPlayer.Name;
            Visuals_Rank.Checked = visualsPlayer.Rank;
            Visuals_Snaplines.Checked = visualsPlayer.Snaplines;
            Visuals_VisibleOnly.Checked = visualsPlayer.VisableOnly;
            Visuals_ActiveWeapon.Checked = visualsPlayer.Weapon;
        }

        private void SaveVisuals()
        {
            var visualSettings = GlobalVariables.ActiveSettings.VisualSettings;
            var visualsPlayer = new VisualsPlayer();

            visualsPlayer.BoxMode = (BoxMode) Visuals_BoxMode.SelectedIndex;
            visualsPlayer.BoxOutline = Visuals_BoxOutline.Checked;
            visualsPlayer.Enabled = Visuals_Enable.Checked;
            visualsPlayer.GlowMode = (GlowMode) Visuals_Glow.SelectedIndex;
            visualsPlayer.HeadSpot = Visuals_HeadSpot.Checked;
            visualsPlayer.HealthMode = (HealthMode) Visuals_Health.SelectedIndex;
            visualsPlayer.Name = Visuals_Name.Checked;
            visualsPlayer.Rank = Visuals_Rank.Checked;
            visualsPlayer.Snaplines = Visuals_Snaplines.Checked;
            visualsPlayer.VisableOnly = Visuals_VisibleOnly.Checked;
            visualsPlayer.Weapon = Visuals_ActiveWeapon.Checked;

            visualSettings.Enemy = GlobalVariables.ActiveSettings.VisualSettings.Enemy;
            visualSettings.Team = GlobalVariables.ActiveSettings.VisualSettings.Team;
            visualSettings.Yourself = GlobalVariables.ActiveSettings.VisualSettings.Yourself;

            switch (Visuals_PlayerSelect.SelectedIndex)
            {
                case 0:
                    visualsPlayer.vBoxColor = visualSettings.Enemy.vBoxColor;
                    visualsPlayer.vGlowColor = visualSettings.Enemy.vGlowColor;
                    visualsPlayer.vTextColor = visualSettings.Enemy.vTextColor;
                    visualsPlayer.vHeadSpotColor = visualSettings.Enemy.vHeadSpotColor;
                    visualsPlayer.vSnapLinesColor = visualSettings.Enemy.vSnapLinesColor;
                    visualsPlayer.hBoxColor = visualSettings.Enemy.hBoxColor;
                    visualsPlayer.hGlowColor = visualSettings.Enemy.hGlowColor;
                    visualsPlayer.hTextColor = visualSettings.Enemy.hTextColor;
                    visualsPlayer.hHeadSpotColor = visualSettings.Enemy.hHeadSpotColor;
                    visualsPlayer.hSnapLinesColor = visualSettings.Enemy.hSnapLinesColor;

                    visualSettings.Enemy = visualsPlayer;
                    break;
                case 1:
                    visualsPlayer.vBoxColor = visualSettings.Team.vBoxColor;
                    visualsPlayer.vGlowColor = visualSettings.Team.vGlowColor;
                    visualsPlayer.vTextColor = visualSettings.Team.vTextColor;
                    visualsPlayer.vHeadSpotColor = visualSettings.Team.vHeadSpotColor;
                    visualsPlayer.vSnapLinesColor = visualSettings.Team.vSnapLinesColor;
                    visualsPlayer.hBoxColor = visualSettings.Team.hBoxColor;
                    visualsPlayer.hGlowColor = visualSettings.Team.hGlowColor;
                    visualsPlayer.hTextColor = visualSettings.Team.hTextColor;
                    visualsPlayer.hHeadSpotColor = visualSettings.Team.hHeadSpotColor;
                    visualsPlayer.hSnapLinesColor = visualSettings.Team.hSnapLinesColor;

                    visualSettings.Team = visualsPlayer;
                    break;
                case 2:
                    visualsPlayer.vBoxColor = visualSettings.Yourself.vBoxColor;
                    visualsPlayer.vGlowColor = visualSettings.Yourself.vGlowColor;
                    visualsPlayer.vTextColor = visualSettings.Yourself.vTextColor;
                    visualsPlayer.vHeadSpotColor = visualSettings.Yourself.vHeadSpotColor;
                    visualsPlayer.vSnapLinesColor = visualSettings.Yourself.vSnapLinesColor;
                    visualsPlayer.hBoxColor = visualSettings.Yourself.hBoxColor;
                    visualsPlayer.hGlowColor = visualSettings.Yourself.hGlowColor;
                    visualsPlayer.hTextColor = visualSettings.Yourself.hTextColor;
                    visualsPlayer.hHeadSpotColor = visualSettings.Yourself.hHeadSpotColor;
                    visualsPlayer.hSnapLinesColor = visualSettings.Yourself.hSnapLinesColor;

                    visualSettings.Yourself = visualsPlayer;
                    break;
            }

            visualSettings.Active = Visuals_Active.Checked;
            visualSettings.wWeapon = Visuals_wWeapon.Checked;
            visualSettings.wDroppedC4 = Visuals_wDroppedC4.Checked;
            visualSettings.wPlantedC4 = Visuals_wPlantedC4.Checked;
            visualSettings.wDefuser = Visuals_wDefuser.Checked;
            visualSettings.wGrenades = Visuals_wGrenades.Checked;
            visualSettings.wHostage = Visuals_wHostage.Checked;
            visualSettings.wChicken = Visuals_wChicken.Checked;
            visualSettings.DrawAimbotFov = Visuals_oAimbotFov.Checked;
            visualSettings.DrawRecoil = Visuals_oRecoil.Checked;
            visualSettings.DrawSniper = Visuals_oSniper.Checked;
            visualSettings.NoHands = Visuals_oNoHands.Checked;
            visualSettings.NoScope = Visuals_oNoScope.Checked;
            GlobalVariables.ActiveSettings.VisualSettings = visualSettings;
        }

        #endregion

        #region Skins

        private void InitSkins()
        {
            var BaseSkinData = new SkinData();
            GlobalVariables.ActiveSettings.SkinSettings.AK47 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.AUG = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.AWP = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.CZ75Auto = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.DesertEagle = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.DualBerettas = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.FAMAS = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.FiveSeveN = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.G3SG1 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.GalilAR = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.Glock18 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.M249 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.M4A1S = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.M4A4 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.MAC10 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.MAG7 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.MP5SD = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.MP7 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.MP9 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.Negev = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.Nova = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.P2000 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.P250 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.P90 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.PPBizon = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.R8Revolver = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.SawedOff = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.SCAR020 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.SG553 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.SSG08 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.Tec9 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.UMP45 = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.USPS = BaseSkinData;
            GlobalVariables.ActiveSettings.SkinSettings.XM1014 = BaseSkinData;
        }

        private void LoadSkins()
        {
            var SkinDataSettings = new SkinData();

            switch (WEAPONLIST.SelectedIndex)
            {
                case 0:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.AK47;
                    break;
                case 1:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.AUG;
                    break;
                case 2:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.AWP;
                    break;
                case 3:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.CZ75Auto;
                    break;
                case 4:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.DesertEagle;
                    break;
                case 5:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.DualBerettas;
                    break;
                case 6:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.FAMAS;
                    break;
                case 7:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.FiveSeveN;
                    break;
                case 8:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.G3SG1;
                    break;
                case 9:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.GalilAR;
                    break;
                case 10:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.Glock18;
                    break;
                case 11:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.M249;
                    break;
                case 12:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.M4A1S;
                    break;
                case 13:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.M4A4;
                    break;
                case 14:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.MAC10;
                    break;
                case 15:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.MAG7;
                    break;
                case 16:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.MP5SD;
                    break;
                case 17:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.MP7;
                    break;
                case 18:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.MP9;
                    break;
                case 19:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.Negev;
                    break;
                case 20:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.Nova;
                    break;
                case 21:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.P2000;
                    break;
                case 22:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.P250;
                    break;
                case 23:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.P90;
                    break;
                case 24:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.PPBizon;
                    break;
                case 25:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.R8Revolver;
                    break;
                case 26:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.SawedOff;
                    break;
                case 27:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.SCAR020;
                    break;
                case 28:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.SG553;
                    break;
                case 29:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.SSG08;
                    break;
                case 30:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.Tec9;
                    break;
                case 31:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.UMP45;
                    break;
                case 32:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.USPS;
                    break;
                case 33:
                    SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.XM1014;
                    break;
            }

            if (SkinDataSettings.PaintKit != 0)
                skins_SkinSearch.Text = ((SkinList) SkinDataSettings.PaintKit).ToString();

            SKINSLIST.SelectedIndex = 0;

            WearSlider.Value = SkinDataSettings.Wear + 0.0001f;
            SeedNum.Value = SkinDataSettings.Seed;
            StatNum.Value = SkinDataSettings.StatTrak;
            WeaponName.Text = SkinDataSettings.Name;
        }

        #endregion

        #endregion

        #region Control Interaction

        private void RADAR_SizeChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Skins_UpdateSkins_Click(object sender, EventArgs e)
        {
            File.Delete(SkinParser.skindata_dta);
            SkinParser.Start();
        }

        private void WndOverlay_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClientCmd.Exec("bind mouse1 +attack");
        }

        private void Misc_ThirdPersonKey_Click(object sender, EventArgs e)
        {
            Misc_ThirdPersonKey.Text = "";
            Misc_ThirdPersonKey.Refresh();
            var key = 0;
            while (true)
            {
                for (var i = 0; i < 200; i++)
                    if ((WinApi.GetAsyncKeyState(i) & 0x8000) > 0)
                    {
                        key = i;
                        break;
                    }

                if (key != 0)
                    break;
            }

            if (key == 0x1B)
            {
                GlobalVariables.ActiveSettings.MiscSettings.ThirdPersonKey = 0;
                Misc_ThirdPersonKey.Text = "None";
            }
            else
            {
                var vkey = (VirtualKeys) key;
                Misc_ThirdPersonKey.Text = vkey.ToString();
                GlobalVariables.ActiveSettings.MiscSettings.ThirdPersonKey = key;
            }
        }

        private void Aimbot_AimKey_Click(object sender, EventArgs e)
        {
            Aimbot_AimKey.Text = "";
            Aimbot_AimKey.Refresh();
            var key = 0;
            while (true)
            {
                for (var i = 0; i < 200; i++)
                    if ((WinApi.GetAsyncKeyState(i) & 0x8000) > 0)
                    {
                        key = i;
                        break;
                    }

                if (key != 0)
                    break;
            }

            if (key == 0x1B)
            {
                GlobalVariables.ActiveSettings.AimbotSettings.Key = 0;
                Aimbot_AimKey.Text = "None";
            }
            else
            {
                var vkey = (VirtualKeys) key;
                Aimbot_AimKey.Text = vkey.ToString();
                GlobalVariables.ActiveSettings.AimbotSettings.Key = key;
            }
        }

        private void Triggerbot_TriggerKey_Click(object sender, EventArgs e)
        {
            Triggerbot_TriggerKey.Text = "";
            Triggerbot_TriggerKey.Refresh();
            var key = 0;
            while (true)
            {
                for (var i = 0; i < 200; i++)
                    if ((WinApi.GetAsyncKeyState(i) & 0x8000) > 0)
                    {
                        key = i;
                        break;
                    }

                if (key != 0)
                    break;
            }

            if (key == 0x1B)
            {
                GlobalVariables.ActiveSettings.TriggerbotSettings.Key = 0;
                Triggerbot_TriggerKey.Text = "None";
            }
            else
            {
                var vkey = (VirtualKeys) key;
                Triggerbot_TriggerKey.Text = vkey.ToString();
                GlobalVariables.ActiveSettings.TriggerbotSettings.Key = key;
            }
        }

        private void Configs_UnloadCheat_Click(object sender, EventArgs e)
        {
            Memory.CloseCheat();
        }

        private void Misc_ApplyName_Click(object sender, EventArgs e)
        {
            if (GlobalVariables.ActiveSettings.MiscSettings.NameChanger)
                NameChanger.Set(GlobalVariables.ActiveSettings.MiscSettings.NameTag);
        }

        private void Visuals_Active_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is DeCheckBox)
                if (!((DeCheckBox) sender).Checked)
                    CSGOEngine.csClient.ForceFullUpdate();
        }

        private void WndOverlay_SizeChanged(object sender, EventArgs e)
        {
            UpdateDevice();
        }

        private void LOGO_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new RectangleF(5, 5, LOGO.Width - 11, LOGO.Height - 11);
            var rect2 = new RectangleF(LOGO.Width / 2 - 6, LOGO.Width / 2 - 6, 12, 12);

            using (var b = new SolidBrush(GlobalVariables.PrimaryBackColor))
            {
                e.Graphics.FillEllipse(b, rect);
            }

            using (var b = new SolidBrush(GlobalVariables.PrimaryTextColor))
            {
                e.Graphics.DrawString("I", RADAR.Font, b, LOGO.Width / 2 - 6, 6);
                e.Graphics.DrawString("III", RADAR.Font, b, LOGO.Width - 32, LOGO.Height / 2 - 11);
                e.Graphics.DrawString("VI", RADAR.Font, b, LOGO.Width / 2 - 13, LOGO.Height - 28);
                e.Graphics.DrawString("IX", RADAR.Font, b, 8, LOGO.Height / 2 - 11);
            }

            using (var p = new Pen(GlobalVariables.PrimaryForeColor, 2))
            {
                e.Graphics.DrawEllipse(p, rect);
            }


            using (var p = new Pen(GlobalVariables.SecondaryBackColor, 2))
            {
                e.Graphics.DrawLine(p, new PointF(LOGO.Width / 2f, LOGO.Height / 2f), Maths.mCoord(LOGO.Height / 2f));
                e.Graphics.DrawLine(p, new PointF(LOGO.Width / 2f, LOGO.Height / 2f), Maths.hrCoord(LOGO.Height / 2f));
            }

            using (var p = new Pen(GlobalVariables.PrimaryForeColor, 1))
            {
                e.Graphics.DrawLine(p, new PointF(LOGO.Width / 2f, LOGO.Height / 2f), Maths.sCoord(LOGO.Height / 2f));
            }


            using (var b = new SolidBrush(GlobalVariables.SecondaryBackColor))
            {
                e.Graphics.FillEllipse(b, rect2);
            }
        }

        private void panel_TopBar_DoubleClick(object sender, EventArgs e)
        {
            if (panel_TopBar.Dock == DockStyle.Right)
                panel_TopBar.Dock = DockStyle.Left;
            else
                panel_TopBar.Dock = DockStyle.Right;
        }

        private void skins_SkinSearch_TextChanged(object sender, EventArgs e)
        {
            SkinParser.SearchSkin(SKINSLIST, skins_SkinSearch.Text);
        }

        private void SKINSAPPLY_Click(object sender, EventArgs e)
        {
            var SkinDataSettings = new SkinData();

            var text = SKINSLIST.SelectedItem.ToString();


            if (SKINSLIST.SelectedIndex >= 0)
                SkinDataSettings.PaintKit = int.Parse(text.Substring(text.IndexOf("|") + 3));
            else
                SkinDataSettings.PaintKit = 0;


            SkinDataSettings.Wear = (float) WearSlider.Value;
            SkinDataSettings.Seed = (int) SeedNum.Value;
            SkinDataSettings.StatTrak = (int) StatNum.Value;
            SkinDataSettings.Name = WeaponName.Text;


            switch (WEAPONLIST.SelectedIndex)
            {
                case 0:
                    GlobalVariables.ActiveSettings.SkinSettings.AK47 = SkinDataSettings;
                    break;
                case 1:
                    GlobalVariables.ActiveSettings.SkinSettings.AUG = SkinDataSettings;
                    break;
                case 2:
                    GlobalVariables.ActiveSettings.SkinSettings.AWP = SkinDataSettings;
                    break;
                case 3:
                    GlobalVariables.ActiveSettings.SkinSettings.CZ75Auto = SkinDataSettings;
                    break;
                case 4:
                    GlobalVariables.ActiveSettings.SkinSettings.DesertEagle = SkinDataSettings;
                    break;
                case 5:
                    GlobalVariables.ActiveSettings.SkinSettings.DualBerettas = SkinDataSettings;
                    break;
                case 6:
                    GlobalVariables.ActiveSettings.SkinSettings.FAMAS = SkinDataSettings;
                    break;
                case 7:
                    GlobalVariables.ActiveSettings.SkinSettings.FiveSeveN = SkinDataSettings;
                    break;
                case 8:
                    GlobalVariables.ActiveSettings.SkinSettings.G3SG1 = SkinDataSettings;
                    break;
                case 9:
                    GlobalVariables.ActiveSettings.SkinSettings.GalilAR = SkinDataSettings;
                    break;
                case 10:
                    GlobalVariables.ActiveSettings.SkinSettings.Glock18 = SkinDataSettings;
                    break;
                case 11:
                    GlobalVariables.ActiveSettings.SkinSettings.M249 = SkinDataSettings;
                    break;
                case 12:
                    GlobalVariables.ActiveSettings.SkinSettings.M4A1S = SkinDataSettings;
                    break;
                case 13:
                    GlobalVariables.ActiveSettings.SkinSettings.M4A4 = SkinDataSettings;
                    break;
                case 14:
                    GlobalVariables.ActiveSettings.SkinSettings.MAC10 = SkinDataSettings;
                    break;
                case 15:
                    GlobalVariables.ActiveSettings.SkinSettings.MAG7 = SkinDataSettings;
                    break;
                case 16:
                    GlobalVariables.ActiveSettings.SkinSettings.MP5SD = SkinDataSettings;
                    break;
                case 17:
                    GlobalVariables.ActiveSettings.SkinSettings.MP7 = SkinDataSettings;
                    break;
                case 18:
                    GlobalVariables.ActiveSettings.SkinSettings.MP9 = SkinDataSettings;
                    break;
                case 19:
                    GlobalVariables.ActiveSettings.SkinSettings.Negev = SkinDataSettings;
                    break;
                case 20:
                    GlobalVariables.ActiveSettings.SkinSettings.Nova = SkinDataSettings;
                    break;
                case 21:
                    GlobalVariables.ActiveSettings.SkinSettings.P2000 = SkinDataSettings;
                    break;
                case 22:
                    GlobalVariables.ActiveSettings.SkinSettings.P250 = SkinDataSettings;
                    break;
                case 23:
                    GlobalVariables.ActiveSettings.SkinSettings.P90 = SkinDataSettings;
                    break;
                case 24:
                    GlobalVariables.ActiveSettings.SkinSettings.PPBizon = SkinDataSettings;
                    break;
                case 25:
                    GlobalVariables.ActiveSettings.SkinSettings.R8Revolver = SkinDataSettings;
                    break;
                case 26:
                    GlobalVariables.ActiveSettings.SkinSettings.SawedOff = SkinDataSettings;
                    break;
                case 27:
                    GlobalVariables.ActiveSettings.SkinSettings.SCAR020 = SkinDataSettings;
                    break;
                case 28:
                    GlobalVariables.ActiveSettings.SkinSettings.SG553 = SkinDataSettings;
                    break;
                case 29:
                    GlobalVariables.ActiveSettings.SkinSettings.SSG08 = SkinDataSettings;
                    break;
                case 30:
                    GlobalVariables.ActiveSettings.SkinSettings.Tec9 = SkinDataSettings;
                    break;
                case 31:
                    GlobalVariables.ActiveSettings.SkinSettings.UMP45 = SkinDataSettings;
                    break;
                case 32:
                    GlobalVariables.ActiveSettings.SkinSettings.USPS = SkinDataSettings;
                    break;
                case 33:
                    GlobalVariables.ActiveSettings.SkinSettings.XM1014 = SkinDataSettings;
                    break;
            }

            MessageBox.Show(GlobalVariables.ActiveSettings.SkinSettings.AK47.PaintKit.ToString());
        }

        private void COLORSAPPLY_Click(object sender, EventArgs e)
        {
            if (colorPicker.SelectedColor.R == 0 &&
                colorPicker.SelectedColor.G == 0 &&
                colorPicker.SelectedColor.B == 0)
                colorPicker.SelectedColor = Color.FromArgb(colorPicker.SelectedColor.A, 1, 0, 0);

            switch (color_PickerList.SelectedIndex + 1)
            {
                case 1:
                    GlobalVariables.ActiveSettings.VisualSettings.Enemy.hBoxColor =
                        colorPicker.RawColor;
                    break;
                case 2:
                    GlobalVariables.ActiveSettings.VisualSettings.Enemy.hGlowColor =
                        colorPicker.RawColor;
                    break;
                case 3:
                    GlobalVariables.ActiveSettings.VisualSettings.Enemy.hHeadSpotColor =
                        colorPicker.RawColor;
                    break;
                case 4:
                    GlobalVariables.ActiveSettings.VisualSettings.Enemy.hSnapLinesColor =
                        colorPicker.RawColor;
                    break;
                case 5:
                    GlobalVariables.ActiveSettings.VisualSettings.Enemy.hTextColor =
                        colorPicker.RawColor;
                    break;
                case 6:
                    GlobalVariables.ActiveSettings.VisualSettings.Enemy.vBoxColor =
                        colorPicker.RawColor;
                    break;
                case 7:
                    GlobalVariables.ActiveSettings.VisualSettings.Enemy.vGlowColor =
                        colorPicker.RawColor;
                    break;
                case 8:
                    GlobalVariables.ActiveSettings.VisualSettings.Enemy.vHeadSpotColor =
                        colorPicker.RawColor;
                    break;
                case 9:
                    GlobalVariables.ActiveSettings.VisualSettings.Enemy.vSnapLinesColor =
                        colorPicker.RawColor;
                    break;
                case 10:
                    GlobalVariables.ActiveSettings.VisualSettings.Enemy.vTextColor =
                        colorPicker.RawColor;
                    break;

                case 11:
                    GlobalVariables.ActiveSettings.VisualSettings.Team.hBoxColor =
                        colorPicker.RawColor;
                    break;
                case 12:
                    GlobalVariables.ActiveSettings.VisualSettings.Team.hGlowColor =
                        colorPicker.RawColor;
                    break;
                case 13:
                    GlobalVariables.ActiveSettings.VisualSettings.Team.hHeadSpotColor =
                        colorPicker.RawColor;
                    break;
                case 14:
                    GlobalVariables.ActiveSettings.VisualSettings.Team.hSnapLinesColor =
                        colorPicker.RawColor;
                    break;
                case 15:
                    GlobalVariables.ActiveSettings.VisualSettings.Team.hTextColor =
                        colorPicker.RawColor;
                    break;
                case 16:
                    GlobalVariables.ActiveSettings.VisualSettings.Team.vBoxColor =
                        colorPicker.RawColor;
                    break;
                case 17:
                    GlobalVariables.ActiveSettings.VisualSettings.Team.vGlowColor =
                        colorPicker.RawColor;
                    break;
                case 18:
                    GlobalVariables.ActiveSettings.VisualSettings.Team.vHeadSpotColor =
                        colorPicker.RawColor;
                    break;
                case 19:
                    GlobalVariables.ActiveSettings.VisualSettings.Team.vSnapLinesColor =
                        colorPicker.RawColor;
                    break;
                case 20:
                    GlobalVariables.ActiveSettings.VisualSettings.Team.vTextColor =
                        colorPicker.RawColor;
                    break;

                case 21:
                    GlobalVariables.ActiveSettings.VisualSettings.Yourself.hBoxColor =
                        colorPicker.RawColor;
                    break;
                case 22:
                    GlobalVariables.ActiveSettings.VisualSettings.Yourself.hGlowColor =
                        colorPicker.RawColor;
                    break;
                case 23:
                    GlobalVariables.ActiveSettings.VisualSettings.Yourself.hHeadSpotColor =
                        colorPicker.RawColor;
                    break;
                case 24:
                    GlobalVariables.ActiveSettings.VisualSettings.Yourself.hSnapLinesColor =
                        colorPicker.RawColor;
                    break;
                case 25:
                    GlobalVariables.ActiveSettings.VisualSettings.Yourself.hTextColor =
                        colorPicker.RawColor;
                    break;
                case 26:
                    GlobalVariables.ActiveSettings.VisualSettings.Yourself.vBoxColor =
                        colorPicker.RawColor;
                    break;
                case 27:
                    GlobalVariables.ActiveSettings.VisualSettings.Yourself.vGlowColor =
                        colorPicker.RawColor;
                    break;
                case 28:
                    GlobalVariables.ActiveSettings.VisualSettings.Yourself.vHeadSpotColor =
                        colorPicker.RawColor;
                    break;
                case 29:
                    GlobalVariables.ActiveSettings.VisualSettings.Yourself.vSnapLinesColor =
                        colorPicker.RawColor;
                    break;
                case 30:
                    GlobalVariables.ActiveSettings.VisualSettings.Yourself.vTextColor =
                        colorPicker.RawColor;
                    break;

                case 31:
                    GlobalVariables.PrimaryForeColor = colorPicker.SelectedColor;
                    break;
                case 32:
                    GlobalVariables.PrimaryBackColor = colorPicker.SelectedColor;
                    break;
                case 33:
                    GlobalVariables.SecondaryBackColor = colorPicker.SelectedColor;
                    break;
                case 34:
                    GlobalVariables.PrimaryTextColor = colorPicker.SelectedColor;
                    break;
            }

            UpdateColors();
        }

        private void color_PickerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (color_PickerList.SelectedIndex + 1)
                {
                    case 1:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Enemy.hBoxColor;
                        break;
                    case 2:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Enemy.hGlowColor;
                        break;
                    case 3:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Enemy.hHeadSpotColor;
                        break;
                    case 4:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Enemy.hSnapLinesColor;
                        break;
                    case 5:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Enemy.hTextColor;
                        break;
                    case 6:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Enemy.vBoxColor;
                        break;
                    case 7:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Enemy.vGlowColor;
                        break;
                    case 8:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Enemy.vHeadSpotColor;
                        break;
                    case 9:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Enemy.vSnapLinesColor;
                        break;
                    case 10:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Enemy.vTextColor;
                        break;

                    case 11:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Team.hBoxColor;
                        break;
                    case 12:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Team.hGlowColor;
                        break;
                    case 13:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Team.hHeadSpotColor;
                        break;
                    case 14:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Team.hSnapLinesColor;
                        break;
                    case 15:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Team.hTextColor;
                        break;
                    case 16:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Team.vBoxColor;
                        break;
                    case 17:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Team.vGlowColor;
                        break;
                    case 18:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Team.vHeadSpotColor;
                        break;
                    case 19:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Team.vSnapLinesColor;
                        break;
                    case 20:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Team.vTextColor;
                        break;

                    case 21:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Yourself.hBoxColor;
                        break;
                    case 22:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Yourself.hGlowColor;
                        break;
                    case 23:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Yourself.hHeadSpotColor;
                        break;
                    case 24:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Yourself.hSnapLinesColor;
                        break;
                    case 25:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Yourself.hTextColor;
                        break;
                    case 26:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Yourself.vBoxColor;
                        break;
                    case 27:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Yourself.vGlowColor;
                        break;
                    case 28:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Yourself.vHeadSpotColor;
                        break;
                    case 29:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Yourself.vSnapLinesColor;
                        break;
                    case 30:
                        colorPicker.RawColor = GlobalVariables.ActiveSettings.VisualSettings.Yourself.vTextColor;
                        break;

                    case 31:
                        colorPicker.SelectedColor = GlobalVariables.PrimaryForeColor;
                        break;
                    case 32:
                        colorPicker.SelectedColor = GlobalVariables.PrimaryBackColor;
                        break;
                    case 33:
                        colorPicker.SelectedColor = GlobalVariables.SecondaryBackColor;
                        break;
                    case 34:
                        colorPicker.SelectedColor = GlobalVariables.PrimaryTextColor;
                        break;
                }
            }
            catch
            {
            }
        }

        private void btn_Reload_Click(object sender, EventArgs e)
        {
            UpdateDevice();
        }

        #endregion
    }
}