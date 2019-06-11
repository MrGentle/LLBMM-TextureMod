using LLScreen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace TextureMod
{
    public class TextureLoader : MonoBehaviour
    {
        private static string resourceFolder = Application.dataPath.Replace("/", @"\") + @"\Managed\TextureModResources\Images\Characters\";
        private List<string> chars = new List<string>();
        public Dictionary<Character, Dictionary<string, Texture2D>> characterTextures = new Dictionary<Character, Dictionary<string, Texture2D>>();

        public bool loadingExternalFiles = true;

        private void Start()
        {
            foreach (string path in Directory.GetDirectories(resourceFolder.Replace("/", @"\")))
            {
                chars.Add(path);
            }

            foreach (string character in chars)
            {
                Dictionary<string, Texture2D> skins = new Dictionary<string, Texture2D>();
                foreach (string file in Directory.GetFiles(character))
                {
                    skins.Add(Path.GetFileName(file), TextureHelper.LoadPNG(file));
                }
                characterTextures.Add(StringToChar(Path.GetFileName(character)), skins);
            }
            UIScreen.SetLoadingScreen(false);
            loadingExternalFiles = false;
        }

        private Character StringToChar(string charString)
        {
            Character ret = Character.NONE;
            switch (charString)
            {
                case "CANDYMAN":
                    ret = Character.CANDY;
                    break;
                case "DICE":
                    ret = Character.PONG;
                    break;
                case "DOOMBOX":
                    ret = Character.BOSS;
                    break;
                case "GRID":
                    ret = Character.ELECTRO;
                    break;
                case "JET":
                    ret = Character.SKATE;
                    break;
                case "LATCH":
                    ret = Character.CROC;
                    break;
                case "NITRO":
                    ret = Character.COP;
                    break;
                case "RAPTOR":
                    ret = Character.KID;
                    break;
                case "SONATA":
                    ret = Character.BOOM;
                    break;
                case "SWITCH":
                    ret = Character.ROBOT;
                    break;
                case "TOXIC":
                    ret = Character.GRAF;
                    break;
                default:
                    ret = Character.NONE;
                    break;
            }
            return ret;
        }
    }
}
