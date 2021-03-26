#define CustomSkin
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace TextureMod
{
#if CustomSkin
    public class CustomSkin
    {
        public CustomSkin(int _index,Character _character, VariantType _variant, string _name, string _author, string _filePath)
        {
            index = _index;
            FileLocation = _filePath;
            SkinTexture = TextureHelper.LoadPNG(FileLocation);
            Name = SkinTexture.name = _name;
            Character = _character;
            Variant = _variant;
            Author = _author;
        }

        public int index { get; private set; }
        public Character Character { get; private set; }
        public VariantType Variant { get; private set; }
        public Texture2D SkinTexture { get; private set; }
        public List<Color> SkinColors { get; private set; }
        public string Author { get; private set; }
        public string Name { get; private set; }

        private string FileLocation;

        public Texture2D ReloadSkin()
        {
            Debug.Log($"Loading Texture at...\n {FileLocation}");
            return SkinTexture = TextureHelper.LoadPNG(FileLocation);
        }

        public bool VariantMatch(CharacterVariant characterVariant)
        {
            switch (Variant)
            {
                case VariantType.Default:
                    return characterVariant < CharacterVariant.STATIC_ALT;
                case VariantType.Alternative:
                    return characterVariant == CharacterVariant.MODEL_ALT || characterVariant == CharacterVariant.MODEL_ALT2;
                case VariantType.DLC:
                    return characterVariant == CharacterVariant.MODEL_ALT3 || characterVariant == CharacterVariant.MODEL_ALT4;
                default:
                    return false;
            }
        }

        public string GetSkinLabel()
        {
            StringBuilder sBuilder = new StringBuilder(Name);
            if (Author != "")
            {
                sBuilder.Append($" by {Author}");
            }
            return sBuilder.ToString();
        }

        public enum VariantType
        {
            None,
            Default,
            Alternative,
            DLC,
        }
    }
#endif
}
