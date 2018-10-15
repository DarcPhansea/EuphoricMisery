using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.Configs.Structs;
using DarcEuphoria.Euphoric.CSGO;
using DarcEuphoria.Euphoric.CSGO.Entity;
using DarcEuphoria.Euphoric.Structs;

namespace DarcEuphoria.Hacks
{
    public static class SkinChanger
    {
        private static SkinData SkinDataSettings;

        public static void Start()
        {
            var needsReset = false;

            foreach (var weapon in CSGOEngine.LocalPlayer.AllWeapon)
            {
                if (weapon.IsBomb())
                    continue;

                if (weapon.IsKnife())
                    continue;

                if (weapon.IsGrenade())
                    continue;


                switch (weapon.WeaponID.Value)
                {
                    case 1:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.DesertEagle;
                        break;
                    case 2:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.DualBerettas;
                        break;
                    case 3:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.FiveSeveN;
                        break;
                    case 4:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.Glock18;
                        break;
                    case 7:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.AK47;
                        break;
                    case 8:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.AUG;
                        break;
                    case 9:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.AWP;
                        break;
                    case 10:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.FAMAS;
                        break;
                    case 11:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.G3SG1;
                        break;
                    case 13:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.GalilAR;
                        break;
                    case 14:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.M249;
                        break;
                    case 16:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.M4A4;
                        break;
                    case 17:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.MAC10;
                        break;
                    case 19:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.P90;
                        break;
                    case 23:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.MP5SD;
                        break;
                    case 24:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.UMP45;
                        break;
                    case 25:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.XM1014;
                        break;
                    case 26:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.PPBizon;
                        break;
                    case 27:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.MAG7;
                        break;
                    case 28:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.Negev;
                        break;
                    case 29:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.SawedOff;
                        break;
                    case 30:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.Tec9;
                        break;
                    case 32:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.P2000;
                        break;
                    case 33:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.MP7;
                        break;
                    case 34:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.MP9;
                        break;
                    case 35:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.Nova;
                        break;
                    case 36:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.P250;
                        break;
                    case 38:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.SCAR020;
                        break;
                    case 39:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.SG553;
                        break;
                    case 40:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.SSG08;
                        break;
                    case 69:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.M4A1S;
                        break;
                    case 61:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.USPS;
                        break;
                    case 63:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.CZ75Auto;
                        break;
                    case 64:
                        SkinDataSettings = GlobalVariables.ActiveSettings.SkinSettings.R8Revolver;
                        break;
                }

                if (ApplySkinData(weapon, SkinDataSettings))
                    needsReset = true;
            }

            //if (needsReset)
            //     CSGOEngine.csClient.ForceFullUpdate();
        }

        private static bool ApplySkinData(BaseWeapon weapon, SkinData skinData)
        {
            weapon.Name = skinData.Name;

            if (weapon.PaintKit.Value == skinData.PaintKit &&
                weapon.Seed.Value == skinData.Seed &&
                weapon.Wear.Value == skinData.Wear &&
                weapon.StatTrak.Value == skinData.StatTrak)
                return false;


            WeaponData weaponData;
            weaponData.PaintKit = skinData.PaintKit;
            weaponData.Seed = skinData.Seed;
            weaponData.Wear = skinData.Wear;
            weaponData.StatTrak = skinData.StatTrak;

            Memory.Write(weapon.PaintKit.Address, weaponData);

            weapon.AccountID.Value = weapon.MyAccountID.Value;
            weapon.ItemIDLow.Value = -1;
            return true;
        }
    }
}