using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using LLModMenu;
using System.IO;

namespace TextureMod
{
    public class ModMenuIntegration : MonoBehaviour
    {
        private ModMenu mm;
        private bool mmAdded = false;

        public Dictionary<string, string> configKeys = new Dictionary<string, string>();
        public Dictionary<string, string> configBools = new Dictionary<string, string>();

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
            configKeys.Add("holdToEnableCustomSkinChangerKeyOrButton1", "LeftShift");
            configKeys.Add("holdToEnableCustomSkinChangerKeyOrButton2", "RightShift");
            configKeys.Add("setCustomSkinKeyOrButton", "Mouse0");
            configKeys.Add("cancelOpponentCustomSkin", "A");
            configBools.Add("disableHoldButtonAndOnlyUseSetButton", "false");
            configBools.Add("neverApplyOpponentsSkin", "false");
            ModMenu.Instance.WriteIni(gameObject.name, configKeys, configBools);
        }

        public void ReadIni()
        {
            string[] lines = File.ReadAllLines(Directory.GetParent(Application.dataPath).FullName + @"\" + gameObject.name + ".ini");
            configBools.Clear();
            configKeys.Clear();
            foreach (string line in lines)
            {
                if ((line.EndsWith("true")) || (line.EndsWith("false")))
                {
                    string[] split = line.Split('=');
                    configBools.Add(split[0], split[1]);
                }
                else
                {
                    if (!line.StartsWith("["))
                    {
                        string[] split = line.Split('=');
                        configKeys.Add(split[0], split[1]);
                    }
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
            if (boolName == "true")
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
