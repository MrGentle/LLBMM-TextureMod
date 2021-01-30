using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TextureMod
{
    static class TextureHelper
    {
        public static Texture2D LoadPNG(string _path, string _fileName) //Loads a png from a file and returns it (Loads the asset into memory, do only load it once)
        {
            if (!File.Exists(_path + _fileName))
            {
                Debug.Log("Could not find " + _path + _fileName);
                return null;
            }
            Texture2D tex;
            byte[] fileData;

            fileData = File.ReadAllBytes(_path + _fileName);
            tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.LoadImage(fileData); //This resizes the texture width and height

            return tex;
        }

        public static Texture2D LoadPNG(string _path) //Loads a png from a file and returns it (Loads the asset into memory, do only load it once)
        {
            if (!File.Exists(_path))
            {
                Debug.Log("Could not find " + _path);
                return null;
            }
            Texture2D tex;
            byte[] fileData;

            fileData = File.ReadAllBytes(_path);
            tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.LoadImage(fileData); //This resizes the texture width and height

            return tex;
        }


        public static Texture2D ReloadSkin(Character _character, Texture2D _texture)
        {
            TextureLoader TL = TextureMod.Instance.tl;
            Texture2D newTex = null;

            string characterDirectory = "";
            string skinName = "";

            foreach (string path in TL.chars) if (TL.StringToChar(Path.GetFileName(path)) == _character) characterDirectory = path;
            Debug.Log("characterDirectory:" + characterDirectory);

            foreach (var skin in TL.characterTextures[_character]) if (skin.Value == _texture) skinName = skin.Key;

            string oldSkinName = skinName;

            if (skinName.Contains(" by "))
            {
                int i = skinName.IndexOf(" by ");
                if (i >= 0) skinName = skinName.Remove(i);
            }

            foreach (string dir in Directory.GetDirectories(characterDirectory))
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    if (Path.GetFileName(file) == skinName)
                    {
                        newTex = LoadPNG(file);
                        TL.characterTextures[_character][oldSkinName] = newTex;
                    }
                }
            }

            foreach (string file in Directory.GetFiles(characterDirectory))
            {
                if (Path.GetFileName(file) == skinName)
                {
                    newTex = LoadPNG(file);
                    TL.characterTextures[_character][skinName] = newTex;
                }
            }

            Resources.UnloadUnusedAssets();
            return newTex;
        }
    }
}
