using LLHandlers;
using LLScreen;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


namespace TextureMod
{
    public class TextureLoader : MonoBehaviour
    {
        private static string resourceFolder = Application.dataPath.Replace("/", @"\") + @"\Managed\TextureModResources\Images\Characters\";
        public List<string> chars = new List<string>();
        public Dictionary<Character, Dictionary<string, Texture2D>> characterTextures = new Dictionary<Character, Dictionary<string, Texture2D>>();
        const string regexFilter = @"^\d+#";

        public bool loadingExternalFiles = true;
        public bool hasCactuar = false;

        private void Start()
        {
            LoadLibrary();
        }

        public void LoadLibrary()
        {
            chars.Clear();
            Resources.UnloadUnusedAssets();
            characterTextures.Clear();

            foreach (string path in Directory.GetDirectories(resourceFolder.Replace("/", @"\")))
            {
                chars.Add(path);
            }

            foreach (string characterFolder in chars)
            {

                Dictionary<string, Texture2D> skins = new Dictionary<string, Texture2D>();
                Character character = StringToChar(Path.GetFileName(characterFolder));
                bool hasDLC = CheckHasDLCForCharacter(character);

                foreach (string file in Directory.GetFiles(characterFolder, "*.png", SearchOption.TopDirectoryOnly))
                {
                    ModelVariant modelVariant = ModelVariantFromFile(file);
                    if (modelVariant == ModelVariant.DLC && hasDLC == false)
                    {
                        continue;
                    }

                    string cleanName = Path.GetFileNameWithoutExtension(file);
                    cleanName = Regex.Replace(cleanName, regexFilter, "");
                    skins.Add(cleanName, TextureHelper.LoadPNG(file));
                }


                foreach (string dir in Directory.GetDirectories(characterFolder))
                {
                    string authorName = Path.GetFileName(dir);
                    authorName = Regex.Replace(authorName, regexFilter, "");

                    foreach (string file in Directory.GetFiles(dir, "*.png", SearchOption.TopDirectoryOnly))
                    {
                        ModelVariant modelVariant = ModelVariantFromFile(file);
                        if (modelVariant == ModelVariant.DLC && hasDLC == false)
                        {
                            continue;
                        }

                        string cleanName = Path.GetFileNameWithoutExtension(file);
                        cleanName = Regex.Replace(cleanName, regexFilter, "");

                        skins.Add(cleanName + " by " + authorName, TextureHelper.LoadPNG(file));
                    }
                }

                characterTextures.Add(character, skins);
            }

            UIScreen.SetLoadingScreen(false);
            loadingExternalFiles = false;
        }

        bool CheckHasDLCForCharacter(Character character)
        {
            foreach (Character DLC in TextureMod.ownedDLCs)
            {
                if (DLC == character)
                {
                    return true;
                }
            }
            return false;
        }

        public enum ModelVariant
        {
            None,
            Default,
            Alternative,
            DLC,
        }

        ModelVariant ModelVariantFromFile(string path)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);

            if (fileName.Contains("_ALT2"))
            {
                return ModelVariant.DLC;
            }
            else if (fileName.Contains("_ALT"))
            {
                return ModelVariant.Alternative;
            }
            else return ModelVariant.Default;
        }

        public Character StringToChar(string charString)
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
                case "DUST&ASHES":
                    ret = Character.BAG;
                    break;
                case "CACTUAR":
                    hasCactuar = true;
                    ret = (Character)50;
                    break;
                default:
                    ret = Character.NONE;
                    break;
            }
            return ret;
        }
    }
}
