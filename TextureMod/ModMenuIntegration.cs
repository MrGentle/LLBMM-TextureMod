// Script used to connect to ModMenu
using System.Collections.Generic;
using UnityEngine;
using LLModMenu;
using System.IO;
using System;

namespace TextureMod
{
    public class ModMenuIntegration : MonoBehaviour
    {
        private ModMenu mm;
        private bool mmAdded = false;

        public Dictionary<string, string> configKeys = new Dictionary<string, string>();
        public Dictionary<string, string> configBools = new Dictionary<string, string>();
        public Dictionary<string, string> configInts = new Dictionary<string, string>();
        public Dictionary<string, string> configSliders = new Dictionary<string, string>();
        public Dictionary<string, string> configText = new Dictionary<string, string>();

        private void Start()
        {
            InitConfig();
            ReadIni();
        }

        private void Update()
        {
            mm = FindObjectOfType<ModMenu>();
            if (mm != null)
            {
                if (mmAdded == false)
                {
                    mm.mods.Add(gameObject.name);
                    mmAdded = true;
                }
            }
        }

        private void InitConfig()
        {
            configKeys.Add("(key)holdToEnableSkinChanger", "LeftShift");
            configKeys.Add("(key)holdToEnableSkinChanger2", "RightShift");
            configKeys.Add("(key)setCustomSkin", "Mouse0");
            configKeys.Add("(key)cancelOpponentCustomSkin", "A");
            configBools.Add("(bool)noHoldMode", "false");
            configBools.Add("(bool)neverApplyOpponentsSkin", "false");

            configText.Add("(text)text1", "This mod was written by MrGentle");
            configText.Add("(text)text2", " ");
            configText.Add("(text)text3", "Wondering how to assign skins and in what part of the game you can do so?");
            configText.Add("(text)text4", "If you don't have 'No Hold Mode' set to true, simply hold one of the 'Hold To Enable Skin Changer' buttons and press the assigned 'Set Custom Skin button'");
            configText.Add("(text)text5", "Skins can be assigned in Ranked Lobbies, 1v1 Lobbies, FFA Lobbies(Only for player 1 and 2) and in the skin unlock screen for a character");
            ModMenu.Instance.WriteIni(gameObject.name, configKeys, configBools, configInts, configSliders, configText);
        }

        public void ReadIni()
        {
            string[] lines = File.ReadAllLines(Directory.GetParent(Application.dataPath).FullName + @"\ModSettings\" + gameObject.name + ".ini");
            configBools.Clear();
            configKeys.Clear();
            configInts.Clear();
            configSliders.Clear();
            configText.Clear();
            foreach (string line in lines)
            {
                if (line.StartsWith("(key)"))
                {
                    string[] split = line.Split('=');
                    configKeys.Add(split[0], split[1]);
                }
                else if (line.StartsWith("(bool)"))
                {
                    string[] split = line.Split('=');
                    configBools.Add(split[0], split[1]);
                }
                else if (line.StartsWith("(int)"))
                {
                    string[] split = line.Split('=');
                    configInts.Add(split[0], split[1]);
                }
                else if (line.StartsWith("(slider)"))
                {
                    string[] split = line.Split('=');
                    configSliders.Add(split[0], split[1]);
                }
                else if (line.StartsWith("(text)"))
                {
                    string[] split = line.Split('=');
                    configText.Add(split[0], split[1]);
                }
            }
        }

        public KeyCode GetKeyCode(string keyCode)
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (vKey.ToString() == keyCode)
                {
                    return vKey;
                }
            }
            return KeyCode.A;
        }

        public bool GetTrueFalse(string boolName)
        {
            if (boolName == "true") return true;
            else return false;
        }

        public int GetSliderValue(string sliderName)
        {
            string[] vals = configSliders[sliderName].Split('|');
            return Convert.ToInt32(vals[0]);
        }

        public int GetInt(string intName)
        {
            return Convert.ToInt32(configInts[intName]);
        }
    }
}
