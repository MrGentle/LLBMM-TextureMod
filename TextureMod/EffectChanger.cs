using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameplayEntities;
using LLScreen;
using LLGUI;
using LLHandlers;


namespace TextureMod
{
    public class EffectChanger : MonoBehaviour
    {
        public static string resourceFolder = Application.dataPath.Replace("/", @"\") + @"\Managed\TextureModResources\Images\Effects\";

        ModMenuIntegration MMI;

        #region Parry and clash effects
        Texture2D parryActiveBG;
        Texture2D parryActiveMG;
        Texture2D parryActiveFG;

        Texture2D parryEndBG;
        Texture2D parryEndMG;
        Texture2D parryEndFG;

        Texture2D parrySuccessBG;
        Texture2D parrySuccessMG;
        Texture2D parrySuccessFG;

        Texture2D clashBG;
        Texture2D clashFG;

        byte parryFirstColorR = 0;
        byte parryFirstColorG = 0;
        byte parryFirstColorB = 0;

        byte parrySecondColorR = 0;
        byte parrySecondColorG = 0;
        byte parrySecondColorB = 0;

        byte parryThirdColorR = 0;
        byte parryThirdColorG = 0;
        byte parryThirdColorB = 0;
        #endregion



        private void Start()
        {
            parryActiveBG = TextureHelper.LoadPNG(resourceFolder + @"parry\" + "parryActiveBG.png");
            parryActiveMG = TextureHelper.LoadPNG(resourceFolder + @"parry\" + "parryActiveMG.png");
            parryActiveFG = TextureHelper.LoadPNG(resourceFolder + @"parry\" + "parryActiveFG.png");

            parryEndBG = TextureHelper.LoadPNG(resourceFolder + @"parry\" + "parryEndBG.png");
            parryEndMG = TextureHelper.LoadPNG(resourceFolder + @"parry\" + "parryEndMG.png");
            parryEndFG = TextureHelper.LoadPNG(resourceFolder + @"parry\" + "parryEndFG.png");

            parrySuccessBG = TextureHelper.LoadPNG(resourceFolder + @"parry\" + "parrySuccessBG.png");
            parrySuccessMG = TextureHelper.LoadPNG(resourceFolder + @"parry\" + "parrySuccessMG.png");
            parrySuccessFG = TextureHelper.LoadPNG(resourceFolder + @"parry\" + "parrySuccessFG.png");

            clashBG = TextureHelper.LoadPNG(resourceFolder + @"Clash\" + "clashEffectBG.png");
            clashFG = TextureHelper.LoadPNG(resourceFolder + @"Clash\" + "clashEffectFG.png");
        }

        private void FixedUpdate()
        {
            if (MMI == null) MMI = TextureMod.Instance.MMI;
            if (TextureMod.Instance.tc.InMenu())
            {
                parryFirstColorR = (byte)MMI.GetSliderValue("(slider)parryFirstColorR");
                parryFirstColorG = (byte)MMI.GetSliderValue("(slider)parryFirstColorG");
                parryFirstColorB = (byte)MMI.GetSliderValue("(slider)parryFirstColorB");

                parrySecondColorR = (byte)MMI.GetSliderValue("(slider)parrySecondColorR");
                parrySecondColorG = (byte)MMI.GetSliderValue("(slider)parrySecondColorG");
                parrySecondColorB = (byte)MMI.GetSliderValue("(slider)parrySecondColorB");

                parryThirdColorR = (byte)MMI.GetSliderValue("(slider)parryThirdColorR");
                parryThirdColorG = (byte)MMI.GetSliderValue("(slider)parryThirdColorG");
                parryThirdColorB = (byte)MMI.GetSliderValue("(slider)parryThirdColorB");
            }
        }

        private void Update()
        {
            MeshRenderer[] mrs = FindObjectsOfType<MeshRenderer>();
            foreach (MeshRenderer mr in mrs)
            {
                if (mr.name == "parryVisual")
                {
                    mr.material.mainTexture = parryActiveBG;
                    Material m1 = mr.material;
                    Material m2 = new Material(mr.material.shader);
                    Material m3 = new Material(mr.material.shader);
                    m2.mainTexture = parryActiveMG;
                    m3.mainTexture = parryActiveFG;
                    Material[] mArray = new Material[] { m1, m2, m3 };
                    mr.materials = mArray;
                    mr.materials[0].color = new Color32(parryFirstColorR, parryFirstColorG, parryFirstColorB, 255);
                    mr.materials[1].color = new Color32(parrySecondColorR, parrySecondColorG, parrySecondColorB, 255);
                    mr.materials[2].color = new Color32(parryThirdColorR, parryThirdColorG, parryThirdColorB, 255);
                }
            }

            VisualEntity[] ves = FindObjectsOfType<VisualEntity>();
            foreach(VisualEntity ve in ves)
            {
                if (ve.name == "parryEnd")
                {
                    foreach (Renderer mr in ve.GetComponentsInChildren<Renderer>())
                    {
                        mr.material.mainTexture = parryEndBG;
                        Material m1 = mr.material;
                        Material m2 = new Material(mr.material.shader);
                        Material m3 = new Material(mr.material.shader);
                        m2.mainTexture = parryEndMG;
                        m3.mainTexture = parryEndFG;
                        Material[] mArray = new Material[] { m1, m2, m3 };
                        mr.materials = mArray;
                        mr.materials[0].color = new Color32(parryFirstColorR, parryFirstColorG, parryFirstColorB, 255);
                        mr.materials[1].color = new Color32(parrySecondColorR, parrySecondColorG, parrySecondColorB, 255);
                        mr.materials[2].color = new Color32(parryThirdColorR, parryThirdColorG, parryThirdColorB, 255);
                    }
                }


                if (ve.name == "parrySuccess")
                {
                    Renderer mr = ve.GetComponentsInChildren<Renderer>().First();
                    mr.material.mainTexture = parrySuccessBG;
                    Material m1 = mr.material;
                    Material m2 = new Material(mr.material.shader);
                    Material m3 = new Material(mr.material.shader);
                    m2.CopyPropertiesFromMaterial(m1);
                    m3.CopyPropertiesFromMaterial(m1);
                    m2.mainTexture = parrySuccessMG;
                    m3.mainTexture = parrySuccessFG;
                    Material[] mArray = new Material[] { m1, m2, m3 };
                    mr.materials = mArray;
                    mr.materials[0].color = new Color32(parryFirstColorR, parryFirstColorG, parryFirstColorB, 255);
                    mr.materials[1].color = new Color32(parrySecondColorR, parrySecondColorG, parrySecondColorB, 255);
                    mr.materials[2].color = new Color32(parryThirdColorR, parryThirdColorG, parryThirdColorB, 255);
                }

                if (ve.name == "clashEffect")
                {
                    Renderer mr = ve.GetComponentsInChildren<Renderer>().First();
                    mr.material.mainTexture = clashBG;
                    Material m1 = mr.material;
                    Material m2 = new Material(mr.material.shader);
                    m2.CopyPropertiesFromMaterial(m1);
                    m2.mainTexture = clashFG;
                    Material[] mArray = new Material[] { m1, m2 };
                    mr.materials = mArray;
                    mr.materials[0].color = new Color32(parryFirstColorR, parryFirstColorG, parryFirstColorB, 255);
                    mr.materials[1].color = new Color32(parryThirdColorR, parryThirdColorG, parryThirdColorB, 255);
                }
            }
        }

        public Texture2D Combine(Texture2D background, Texture2D overlay)
        {

            int startX = 0;
            int startY = background.height - overlay.height;

            for (int x = startX; x < background.width; x++)
            {

                for (int y = startY; y < background.height; y++)
                {
                    Color bgColor = background.GetPixel(x, y);
                    Color wmColor = overlay.GetPixel(x - startX, y - startY);

                    Color final_color = Color.Lerp(bgColor, wmColor, wmColor.a / 1.0f);

                    background.SetPixel(x, y, final_color);
                }
            }

            background.Apply();
            return background;
        }
    }
}
