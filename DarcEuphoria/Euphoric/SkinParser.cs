using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DarcEuphoria.Euphoric.Controls;
using DarcEuphoria.Euphoric.Enums;

namespace DarcEuphoria.Euphoric
{
    public static class SkinParser
    {
        public static Dictionary<int, string> Skins = new Dictionary<int, string>();

        public static string[] SkinName;
        public static List<int> SkinID = new List<int>();
        private static readonly List<string> SkinDescription = new List<string>();

        private static readonly string items_game_txt = "csgo/scripts/items/items_game.txt";
        private static readonly string csgo_english_txt = "csgo/resource/csgo_english.txt";
        public static string skindata_dta = Application.LocalUserAppDataPath + @"\skindata.dta";

        public static int GetNthIndex(string s, char t, int n)
        {
            var count = 0;
            for (var i = 0; i < s.Length; i++)
                if (s[i] == t)
                {
                    count++;
                    if (count == n) return i;
                }

            return -1;
        }

        public static void Start()
        {
            if (!File.Exists(skindata_dta))
            {
                GetID();
                SkinName = new string[SkinID.Count];
                GetName();
                SetDictionary();

                var skinsData = string.Empty;
                var skinsData2 = string.Empty;

                foreach (var skin in Skins)
                {
                    skinsData += skin.Value + ":" + skin.Key + "\n";
                    skinsData2 += skin.Value + " = " + skin.Key + ",\n";
                }

                File.WriteAllText(skindata_dta, skinsData);
                File.WriteAllText(skindata_dta + ".sav", skinsData2);
                return;
            }

            var fileRead = File.ReadAllLines(skindata_dta);

            SkinName = new string[fileRead.Length];

            for (var i = 0; i < fileRead.Length; i++)
                if (fileRead[i].Contains(":"))
                {
                    var split = fileRead[i].Split(':');

                    SkinID.Add(int.Parse(split[1]));

                    SkinName[i] = split[0];

                    Skins.Add(int.Parse(split[1]), split[0]);
                }
        }

        private static void GetID()
        {
            var start = false;
            var count = 0;

            foreach (var line in File.ReadAllLines(Memory.SteamPath + items_game_txt))
            {
                if (start)
                {
                    var id = line;

                    if (id.Contains("{"))
                        count += 1;
                    if (id.Contains("}"))
                        count -= 1;

                    if (count == 0)
                    {
                        start = false;
                        continue;
                    }

                    if (id.Contains("{") || id.Contains("}")) continue;

                    if (!id.Contains("\"")) continue;

                    if (id.Contains("description_tag"))
                    {
                        id = id.Substring(id.IndexOf("#") + 1, id.Length - id.IndexOf("#") - 2);
                        if (id == "PaintKit_Default_Tag") continue;

                        SkinDescription.Add(id);
                        continue;
                    }

                    if (Regex.Matches(id, @"[a-zA-Z]").Count > 0) continue;

                    id = id.Substring(id.IndexOf('"') + 1, id.Length - id.IndexOf('"') - 2);

                    if (int.Parse(id) == 0 || int.Parse(id) == 9001) continue;

                    SkinID.Add(int.Parse(id));
                }

                if (line.Substring(1) == "\"paint_kits_rarity\"")
                    start = false;

                if (line.Substring(1) == "\"paint_kits\"")
                    start = true;
            }
        }

        private static void GetName()
        {
            for (var i = 0; i < SkinID.Count; i++)
            {
                var start = false;

                foreach (var line in File.ReadAllLines(Memory.SteamPath + csgo_english_txt))
                {
                    if (line.Contains("//Recipes"))
                        continue;

                    if (start)
                    {
                        var id = line;

                        if (!id.Contains("\"")) continue;

                        if (!id.Substring(id.IndexOf("\"")).ToLower().StartsWith("\"paintkit")) continue;
                        if (!id.ToLower().Contains("_tag\"")) continue;

                        if (id.ToLower().Contains(SkinDescription[i].ToLower()))
                        {
                            var index = GetNthIndex(id, '"', 3);
                            if (index != -1)
                            {
                                id = id.Substring(index + 1, id.Length - index - 2);
                                SkinName[i] = id;
                            }

                            continue;
                        }
                    }

                    if (line.Contains("// Paint Kits"))
                        start = true;
                }
            }
        }

        private static void SetDictionary()
        {
            for (var i = 0; i < SkinID.Count; i++) Skins.Add(SkinID[i], SkinName[i]);
        }

        public static void SetSkinList(DeList list)
        {
            list.Items.Clear();

            foreach (var enumVal in Enum.GetValues(typeof(SkinList)))
                list.Items.Add(enumVal + "  |  " + (int) enumVal);
        }

        public static void SearchSkin(DeList list, string text)
        {
            list.Items.Clear();
            text = text.Replace(" ", string.Empty);
            foreach (var enumVal in Enum.GetValues(typeof(SkinList)))
            {
                var item = enumVal + "  |  " + (int) enumVal;

                if (!item.ToLower().Contains(text.ToLower())) continue;

                list.Items.Add(item);
            }
        }
    }
}