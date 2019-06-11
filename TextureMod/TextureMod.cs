using System;
using UnityEngine;
using LLScreen;
using LLHandlers;
using LLModMenu;
using Multiplayer;

namespace TextureMod
{
    public class TextureMod : MonoBehaviour
    {
        private static TextureMod instance = null;
        public static TextureMod Instance { get { return instance; } }
        public static void Initialize() { GameObject gameObject = new GameObject("TextureMod"); TextureMod modscript = gameObject.AddComponent<TextureMod>(); DontDestroyOnLoad(gameObject); instance = modscript; }

        public string debug = "";

        public TextureChanger tc = null;
        public ModMenuIntegration MMI = null;
        public TextureLoader tl = null;
        public ExchangeClient ec = null;
        public string retSkin = "";

        private void Start()
        {
            UIScreen.SetLoadingScreen(true, false, false, Stage.NONE);
        }

        private void Update()
        {
            if (MMI == null) { MMI = gameObject.AddComponent<ModMenuIntegration>(); }
            if (tl == null) { tl = gameObject.AddComponent<TextureLoader>(); }
            if (tc == null) { tc = gameObject.AddComponent<TextureChanger>(); }
            if (ec == null) { ec = gameObject.AddComponent<ExchangeClient>(); }
        }


        private void OnGUI()
        {
            GUI.contentColor = Color.white;
            GUI.skin.label.fontSize = 50;
            if ((tl == null) || UIScreen.loadingScreenActive && tl.loadingExternalFiles == true)
            {
                var sX = Screen.width / 2;
                var sY = UIScreen.GetResolutionFromConfig().height / 3;
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                GUI.Label(new Rect(0, sY+50, 1920, sY), "TextureMod is loading External Textures");
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            }
            GUI.skin.label.fontSize = 14;
            GUI.Label(new Rect(5f, 5f, 1920f, 25f), debug);
        }
    }
}
