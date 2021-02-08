using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TextureMod
{
#if DEBUG
    public class CustomSkin
    {
        public CustomSkin() { }
        public CustomSkin(ModelVariant _variant, string _name, string _author, string _filePath)
        {
            SkinTexture = TextureHelper.LoadPNG(_filePath);
            Name = SkinTexture.name = _name;
            Variant = _variant;
            Author = _author;
        }

        public void Reset()
        {
            SkinTexture = null;
            Variant = ModelVariant.None;
            Author = "";
            Name = "";
        }

        public Texture2D SkinTexture { get; private set; }
        public ModelVariant Variant { get; private set; }
        public string Author { get; private set; }
        public string Name { get; private set; }

        public bool VariantMatch(CharacterVariant characterVariant)
        {
            switch (Variant)
            {
                case ModelVariant.Default:
                    return characterVariant < CharacterVariant.STATIC_ALT;
                case ModelVariant.Alternative:
                    return characterVariant == CharacterVariant.MODEL_ALT || characterVariant == CharacterVariant.MODEL_ALT2;
                case ModelVariant.DLC:
                    return characterVariant == CharacterVariant.MODEL_ALT3 || characterVariant == CharacterVariant.MODEL_ALT4;
                default:
                    return false;
            }
        }

        public enum ModelVariant
        {
            None,
            Default,
            Alternative,
            DLC,
        }
    } 
#endif
}
