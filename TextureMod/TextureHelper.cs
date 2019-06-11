using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace TextureMod
{
    class TextureHelper
    {
        public static Texture2D LoadPNG(string _path, string _fileName) //Loads a png from a file and returns it (Loads the asset into memory, do only load it once)
        {
            if (!File.Exists(_path + _fileName))
            {
                Debug.Log("Could not find " + _path + _fileName);
                return null;
            }
            Texture2D tex = null;
            byte[] fileData;

            fileData = File.ReadAllBytes(_path + _fileName);
            tex = new Texture2D(2, 2);
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
            Texture2D tex = null;
            byte[] fileData;

            fileData = File.ReadAllBytes(_path);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //This resizes the texture width and height

            return tex;
        }
    }
}
