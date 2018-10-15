using System;
using System.Text;
using System.Threading.Tasks;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.Configs.Enums;
using DarcEuphoria.Euphoric.CSGO;

namespace DarcEuphoria.Hacks.Injection
{
    internal class ClanChanger
    {
        public static byte[] Shellcode =
        {
            0xB9, 0x00, 0x00, 0x00, 0x00,
            0xBA, 0x00, 0x00, 0x00, 0x00,
            0xB8, 0x00, 0x00, 0x00, 0x00,
            0xFF, 0xD0,
            0xC3,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        public static int Size = Shellcode.Length;
        public static IntPtr Address;
        public static string PREVNAME = string.Empty;

        public static int clanPrevNum = -1;
        public static int clanPrevNum2 = -1;

        public static void Set(string tag)
        {
            if (Address == IntPtr.Zero)
            {
                Address = GlobalVariables.Allocator.Alloc(Size);
                GlobalVariables.Allocator.Free();

                if (Address == IntPtr.Zero)
                    return;

                Buffer.BlockCopy(BitConverter.GetBytes((int) (Address + 18)), 0, Shellcode, 1, 4);
                Buffer.BlockCopy(BitConverter.GetBytes((int) (Address + 18)), 0, Shellcode, 6, 4);
                Buffer.BlockCopy(BitConverter.GetBytes((int) Memory.Engine.Base + Offsets.dwSetClanTag), 0, Shellcode,
                    11, 4);
            }

            if (!CSGOEngine.csClient.InGame) return;

            if (tag == PREVNAME) return;
            PREVNAME = tag;

            var tag_bytes = Encoding.UTF8.GetBytes(tag + "\0");
            byte[] reset =
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            Buffer.BlockCopy(reset, 0, Shellcode, 18, reset.Length);
            Buffer.BlockCopy(tag_bytes, 0, Shellcode, 18, tag.Length > 15 ? 15 : tag.Length);
            CreateThread.Create(Address, Shellcode);
        }

        public static Task Start()
        {
            return Task.Factory.StartNew(() =>
            {
                if (GlobalVariables.ActiveSettings.MiscSettings.ClanChangerMode ==
                    ClanMode.Off)
                    Set(string.Empty);

                if (GlobalVariables.ActiveSettings.MiscSettings.ClanChangerMode ==
                    ClanMode.Animated)
                {
                    var tag = string.Empty;
                    var t = (int) (CSGOEngine.csClient.GlobalVarsBase.Value.curtime * 2.4) % 15;

                    if (clanPrevNum == t) return;

                    clanPrevNum = t;

                    switch (t)
                    {
                        case 0:
                            tag = "EuphoricMisery_";
                            break;
                        case 1:
                            tag = "uphoricMisery_E";
                            break;
                        case 2:
                            tag = "phoricMisery_Eu";
                            break;
                        case 3:
                            tag = "horicMisery_Eup";
                            break;
                        case 4:
                            tag = "oricMisery_Euph";
                            break;
                        case 5:
                            tag = "ricMisery_Eupho";
                            break;
                        case 6:
                            tag = "icMisery_Euphor";
                            break;
                        case 7:
                            tag = "cMisery_Euphori";
                            break;
                        case 8:
                            tag = "Misery_Euphoric";
                            break;
                        case 9:
                            tag = "isery_EuphoricM";
                            break;
                        case 10:
                            tag = "sery_EuphoricMi";
                            break;
                        case 11:
                            tag = "ery_EuphoricMis";
                            break;
                        case 12:
                            tag = "ry_EuphoricMise";
                            break;
                        case 13:
                            tag = "y_EuphoricMiser";
                            break;
                        case 14:
                            tag = "_EuphoricMisery";
                            break;
                        default: break;
                    }

                    Set(tag);
                }

                if (GlobalVariables.ActiveSettings.MiscSettings.ClanChangerMode == ClanMode.Static)
                    Set("EuphoricMisery");

                if (GlobalVariables.ActiveSettings.MiscSettings.ClanChangerMode == ClanMode.Custom)
                    Set(GlobalVariables.ActiveSettings.MiscSettings.ClanTag);

                if (GlobalVariables.ActiveSettings.MiscSettings.ClanChangerMode == ClanMode.HiddenCustom)
                    Set(GlobalVariables.ActiveSettings.MiscSettings.ClanTag + "\n");

                if (GlobalVariables.ActiveSettings.MiscSettings.ClanChangerMode == ClanMode.Hidden)
                    Set(" \n \n ");
            });
        }
    }
}