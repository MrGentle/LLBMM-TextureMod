using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameplayEntities;
using LLScreen;
using LLGUI;

namespace TextureMod
{
    public class TextureChanger : MonoBehaviour
    {
        #region General Fields
        public static string resourceFolder = Application.dataPath.Replace("/", @"\") + @"\Managed\TextureModResources\Images\";
        private JOFJHDJHJGI gameState;
        private GameMode currentGameMode;
        public string[] debug = new string[20];
        public bool doSkinPost = false;
        public int postTimer = 0;
        private int postTimerLimit = 30;
        public bool doSkinGet = false;
        public int getTimer = 0;
        private int getTimerLimit = 20;
        private bool calculateMirror = true;
        private bool setAntiMirrior = false;
        private bool showDebugVariables = false;
        public Character packetSkinCharacter = Character.NONE;
        public CharacterVariant packetSkinCharacterVariant = CharacterVariant.CORPSE;
        public bool sendCancelRequestToServer = true;

        public Color32 originalGrafColor = new Color32();

        #endregion
        #region Config Fields
        private KeyCode holdKey1 = KeyCode.LeftShift;
        private KeyCode holdKey2 = KeyCode.RightShift;
        private KeyCode setSkinKey = KeyCode.Mouse0;
        private KeyCode cancelKey = KeyCode.A;
        private bool useOnlySetKey = false;
        private bool neverApplyOpponentsSkin = false;
        #endregion
        #region LocalPlayer Fields
        private ALDOKEMAOMB localLobbyPlayer = null;
        private CharacterModel localLobbyPlayerModel = null;
        private PlayerEntity localGamePlayerEntity = null;
        private CharacterModel localWinnerModel = null;
        private string localPlayerName = "";
        private Character localPlayerChar = Character.NONE;
        private CharacterVariant localPlayerCharVar = CharacterVariant.CORPSE;
        public Texture2D localTex = null;
        private bool initLocalPlayer = false;
        #endregion
        #region Remote Player Fields
        private ALDOKEMAOMB opponentPlayer = null;
        private CharacterModel opponentLobbyCharacterModel = null;
        private PlayerEntity opponentPlayerEntity = null;
        private CharacterModel opponentWinnerModel = null;
        private string opponentPlayerName = "";
        public Character opponentCustomSkinCharacter = Character.NONE;
        public CharacterVariant opponentCustomSkinCharacterVariant = CharacterVariant.CORPSE;
        public Texture2D opponentCustomTexture = null;
        private bool initOpponentPlayer = false;
        public bool newSkinToApply = false;
        private bool cancelOpponentSkin = false;
        #endregion

        private void OnGUI()
        {
            if (showDebugVariables)
            {
                GUI.Label(new Rect(5f, 5f, 1920f, 50f), debug[0]);
                GUI.Label(new Rect(5f, 55f, 1920f, 50f), debug[1]);
                GUI.Label(new Rect(5f, 105f, 1920f, 50f), debug[2]);
                GUI.Label(new Rect(5f, 155f, 1920f, 50f), debug[3]);
                GUI.Label(new Rect(5f, 205f, 1920f, 50f), debug[4]);
                GUI.Label(new Rect(5f, 255f, 1920f, 50f), debug[5]);
                GUI.Label(new Rect(5f, 305f, 1920f, 50f), debug[6]);
                GUI.Label(new Rect(5f, 355f, 1920f, 50f), debug[7]);
            }
        }

        private void FixedUpdate()
        {
            if (!neverApplyOpponentsSkin)
            {
                if (doSkinPost) { postTimer++; }
                if (localLobbyPlayer != null && localTex != null)
                {
                    if (postTimer >= postTimerLimit)
                    {
                        doSkinPost = false;
                        postTimer = 0;
                        try
                        {
                            StartCoroutine(TextureMod.Instance.ec.PostSkin(localLobbyPlayer.KLEEADMGHNE.peerId, localPlayerChar, localPlayerCharVar, localTex));
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.ToString());
                        }
                    }
                }

                if (doSkinGet) { getTimer++; }
                if (opponentPlayer != null)
                {
                    if (getTimer >= getTimerLimit)
                    {
                        doSkinGet = false;
                        getTimer = 0;
                        try
                        {
                            StartCoroutine(TextureMod.Instance.ec.GetSkin(opponentPlayer.KLEEADMGHNE.peerId));
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.ToString());
                        }
                    }
                }
            }
        } //POST and GET requests

        private void Update()
        {
            #region Set MMI Config Vars
            if (TextureMod.Instance.MMI != null)
            {
                var mmi = TextureMod.Instance.MMI;
                holdKey1 = mmi.GetKeyCode(mmi.configKeys["holdToEnableCustomSkinChangerKeyOrButton1"]);
                holdKey2 = mmi.GetKeyCode(mmi.configKeys["holdToEnableCustomSkinChangerKeyOrButton2"]);
                setSkinKey = mmi.GetKeyCode(mmi.configKeys["setCustomSkinKeyOrButton"]);
                cancelKey = mmi.GetKeyCode(mmi.configKeys["cancelOpponentCustomSkin"]);
                useOnlySetKey = mmi.GetTrueFalse(mmi.configBools["disableHoldButtonAndOnlyUseSetButton"]);
                neverApplyOpponentsSkin = mmi.GetTrueFalse(mmi.configBools["neverApplyOpponentsSkin"]);
            }
            #endregion
            #region Set Static Vars
            gameState = DNPFJHMAIBP.HHMOGKIMBNM();
            currentGameMode = JOMBNFKIHIC.GIGAKBJGFDI.PNJOKAICMNN;
            #endregion
            #region Set Debug Vars
            if (showDebugVariables)
            {
                debug[0] = "General: gameState: " + gameState.ToString() + ", currentGameMode: " + currentGameMode.ToString() + ", doSkinPost: " + doSkinPost.ToString() + ", postTimer: " + postTimer.ToString() + ", doSkinGet:" + doSkinGet.ToString() + ", getTimer: " + getTimer.ToString() + ", setAntiMirror: " + setAntiMirrior.ToString();
                debug[1] = "LocalPlayer: localLobbyPlayer: " + localLobbyPlayer + "| localLobbyPlayerModel: " + localLobbyPlayerModel + "| localGamePlayerEntity: " + localGamePlayerEntity + "| localWinnerModel: " + localWinnerModel + "| localPlayerName: " + localPlayerName + "| localPlayerChar: " + localPlayerChar.ToString() + "| localPlayerCharVar: " + localPlayerCharVar.ToString() + "| localTex: " + localTex + "| initLocalPlayer: " + initLocalPlayer.ToString();
                debug[2] = "RemotePlayer: opponentPlayer: " + opponentPlayer + "| lobbyModel: " + opponentLobbyCharacterModel + "| playerEntity: " + opponentPlayerEntity + "| winnerModel: " + opponentWinnerModel + "| name: " + opponentPlayerName + "| skinCharacter: " + opponentCustomSkinCharacter.ToString() + "| skinCharVar: " + opponentCustomSkinCharacterVariant.ToString() + "| customTex: " + opponentCustomTexture + "| init: " + initOpponentPlayer.ToString() + "| newSkinToApply: " + newSkinToApply.ToString() + "| cancelOpponentSkin: " + cancelOpponentSkin.ToString();
            }
            #endregion

            if (opponentPlayer != null && opponentLobbyCharacterModel != null)
            {
                if ((opponentLobbyCharacterModel.character != opponentCustomSkinCharacter || opponentLobbyCharacterModel.characterVariant != opponentCustomSkinCharacterVariant) && (opponentPlayer.ABOGODDKDEE() != Character.NONE || opponentPlayer.AIINAIDBHJI != CharacterVariant.CORPSE))
                {
                    if (packetSkinCharacter != Character.NONE && opponentPlayer.ABOGODDKDEE() != (Character)32 && packetSkinCharacterVariant != CharacterVariant.CORPSE)
                    {
                        opponentCustomSkinCharacter = packetSkinCharacter;
                        opponentCustomSkinCharacterVariant = packetSkinCharacterVariant;
                        opponentLobbyCharacterModel.SetCharacterLobby(opponentPlayer.CJFLMDNNMIE, packetSkinCharacter, packetSkinCharacterVariant, false);
                    }
                    else
                    {
                        if (opponentPlayer.ABOGODDKDEE() != Character.NONE && opponentPlayer.ABOGODDKDEE() != (Character)32 && opponentPlayer.AIINAIDBHJI != CharacterVariant.CORPSE)
                        {
                            opponentCustomSkinCharacter = opponentPlayer.ABOGODDKDEE();
                            opponentCustomSkinCharacterVariant = opponentPlayer.AIINAIDBHJI;
                            opponentLobbyCharacterModel.SetCharacterLobby(opponentPlayer.CJFLMDNNMIE, opponentPlayer.ABOGODDKDEE(), opponentPlayer.AIINAIDBHJI, false);
                            initOpponentPlayer = true;
                        } else
                        {
                            initOpponentPlayer = true;
                        }
                    }
                }

                if (newSkinToApply )
                {
                    if (!cancelOpponentSkin)
                    {
                        if (opponentCustomSkinCharacter == packetSkinCharacter || opponentCustomSkinCharacterVariant == packetSkinCharacterVariant)
                        {
                            opponentCustomTexture = TextureHelper.LoadPNG(Application.dataPath.Replace("/", @"\") + @"\Managed\TextureModResources\Images\opponent.png");
                            packetSkinCharacter = Character.NONE;
                            packetSkinCharacterVariant = CharacterVariant.CORPSE;
                        }
                    }
                    calculateMirror = true;
                    newSkinToApply = false;
                }

                if (opponentPlayer.ABOGODDKDEE() != opponentCustomSkinCharacter && opponentPlayer.ABOGODDKDEE() != (Character)32 && opponentPlayer.ABOGODDKDEE() != Character.NONE)
                {
                    opponentCustomSkinCharacter = opponentPlayer.ABOGODDKDEE();
                    opponentCustomSkinCharacterVariant = opponentPlayer.AIINAIDBHJI;
                    opponentLobbyCharacterModel.SetCharacterLobby(opponentPlayer.CJFLMDNNMIE, opponentPlayer.ABOGODDKDEE(), opponentPlayer.AIINAIDBHJI, false);
                }
            }

            if (localLobbyPlayer != null)
            {
                if ((localPlayerChar != localLobbyPlayer.ABOGODDKDEE() || localPlayerCharVar != localLobbyPlayer.AIINAIDBHJI))
                {
                    localPlayerChar = localLobbyPlayer.ABOGODDKDEE();
                    localPlayerCharVar = localLobbyPlayer.AIINAIDBHJI;
                    initLocalPlayer = true;
                }

                LLButton[] buttons = FindObjectsOfType<LLButton>();
                HDLIJDBFGKN gameStatesOnlineLobby = FindObjectOfType<HDLIJDBFGKN>();
                if (useOnlySetKey == false)
                {
                    if ((Input.GetKey(holdKey1) || Input.GetKey(holdKey2)) && buttons.Length > 0)
                    {
                        foreach (LLButton b in buttons)
                        {
                            b.SetActive(false);
                        }
                        if (Input.GetKeyDown(setSkinKey))
                        {
                            localTex = GetLoadedTexture(localLobbyPlayer.DOFCCEDJODB, localTex);
                            if (localLobbyPlayer != null && gameStatesOnlineLobby != null)
                            {
                                gameStatesOnlineLobby.JPNNBHNHHJC();
                            }
                            doSkinPost = true;
                            postTimer = 0;
                            calculateMirror = true;
                        }
                    }
                    else
                    {
                        if (buttons.Length > 0)
                        {
                            foreach (LLButton b in buttons)
                            {
                                b.SetActive(true);
                            }
                        }
                    }
                } else
                {
                    if (Input.GetKeyDown(setSkinKey))
                    {
                        localTex = GetLoadedTexture(localLobbyPlayer.DOFCCEDJODB, localTex);
                        if (localLobbyPlayer != null && gameStatesOnlineLobby != null)
                        {
                            gameStatesOnlineLobby.JPNNBHNHHJC();
                        }
                        doSkinPost = true;
                        postTimer = 0;
                        calculateMirror = true;
                    }
                }
            } // Determine and assign skin to local player

            if (opponentCustomTexture != null && localTex != null && InLobby(GameType.Any) && calculateMirror && localPlayerChar == opponentCustomSkinCharacter)
            {
                Color[] ot = opponentCustomTexture.GetPixels();
                var otCount = 0;
                foreach (Color col in ot)
                {
                    if (col.r > 0.5f && col.g > 0.5f && col.b > 0.5f && col.a > 0.5f) { if (col.r != 1f && col.g != 1f && col.b != 1f) { otCount++; } }
                    else if (col.r < 0.5f && col.g < 0.5f && col.b < 0.5f) { if (col.r == 0f && col.g == 0f && col.b == 0f) { otCount++; } }
                }
                Color[] pt = localTex.GetPixels();
                var ptCount = 0;
                foreach (Color col in pt)
                {
                    if (col.r > 0.5f && col.g > 0.5f && col.b > 0.5f && col.a > 0.5f) { if (col.r != 1f && col.g != 1f && col.b != 1f) { ptCount++; } }
                    else if (col.r < 0.5f && col.g < 0.5f && col.b < 0.5f) { if (col.r == 0f && col.g == 0f && col.b == 0f) { ptCount++; } }
                }
                PlayerEntity[] pes = FindObjectsOfType<PlayerEntity>();
                if (otCount == ptCount)
                {
                    setAntiMirrior = true;
                    MakeTextureGrayscale(opponentCustomTexture);
                }
                else
                {
                    setAntiMirrior = false;
                }
                calculateMirror = false;
            }

            if (localTex == null || opponentCustomTexture == null) { setAntiMirrior = false; }

            switch (currentGameMode)
            {
                case GameMode.TRAINING:
                case GameMode.TUTORIAL:
                    if (initLocalPlayer) { InitLocalPlayer(); }
                    if (InLobby(GameType.Offline))
                    {
                        if (localLobbyPlayer == null) { localLobbyPlayer = GetLocalPlayerInLobby(GameType.Offline); }
                        else
                        {
                            if (localLobbyPlayerModel == null) { localLobbyPlayerModel = GetLobbyCharacterModel(localLobbyPlayer.CJFLMDNNMIE); }
                            else
                            {
                                localLobbyPlayerModel.SetSilhouette(false);
                                if (localTex != null)
                                {
                                    AssignTextureToCharacterModelRenderers(localLobbyPlayerModel, localTex);
                                }
                            }
                        }
                    }
                    if (InGame(GameType.Offline))
                    {
                        if (localGamePlayerEntity == null) { localGamePlayerEntity = GetLocalPlayerInGame(); }
                        else
                        {
                            if (localTex != null)
                            {
                                AssignTextureToIngameCharacter(localGamePlayerEntity, localTex);
                                localPlayerName = AssignTextureToHud(localGamePlayerEntity, localTex);
                                if (localGamePlayerEntity.character == Character.GRAF)
                                {
                                    AssignToxicEffectColors(localGamePlayerEntity.player.CJFLMDNNMIE, localTex, localGamePlayerEntity.variant);
                                } else if (localGamePlayerEntity.character == Character.BOSS && (localGamePlayerEntity.variant == CharacterVariant.MODEL_ALT3 || localGamePlayerEntity.variant == CharacterVariant.MODEL_ALT4))
                                {
                                    AssignDoomBoxVisualizerColorIngame(localGamePlayerEntity.player.CJFLMDNNMIE, localTex);
                                }
                            }
                        }
                    }
                    if (gameState == (JOFJHDJHJGI)21) { initLocalPlayer = true; }
                    break;
                case GameMode._1v1:
                case GameMode.FREE_FOR_ALL:
                    if (Input.GetKeyDown(cancelKey))
                    {
                        cancelOpponentSkin = !cancelOpponentSkin;
                        if (opponentLobbyCharacterModel != null)
                        {
                            opponentCustomTexture = null;
                            opponentCustomSkinCharacter = opponentPlayer.ABOGODDKDEE();
                            opponentCustomSkinCharacterVariant = opponentPlayer.AIINAIDBHJI;
                            opponentLobbyCharacterModel.SetCharacterLobby(opponentPlayer.CJFLMDNNMIE, opponentCustomSkinCharacter, CharacterVariant.CORPSE, false);
                            initOpponentPlayer = true;
                        }
                    }
                    if (InLobby(GameType.Any))
                    {
                        if (initLocalPlayer) { InitLocalPlayer(); }
                        if (initOpponentPlayer) { InitOpponentPlayer(); }
                        if (localLobbyPlayer == null)
                        {
                            if (InLobby(GameType.Online))
                            {
                                localLobbyPlayer = GetLocalPlayerInLobby(GameType.Online);
                            } else
                            {
                                localLobbyPlayer = GetLocalPlayerInLobby(GameType.Offline);
                            }
                        }
                        else
                        {
                            try
                            {
                                if (sendCancelRequestToServer == true)
                                {
                                    StartCoroutine(TextureMod.Instance.ec.PostCancel(localLobbyPlayer.KLEEADMGHNE.peerId));
                                    sendCancelRequestToServer = false;
                                }
                            } catch { }
                            if (localLobbyPlayer.CJFLMDNNMIE == 3)
                            {
                                initLocalPlayer = true;
                            }

                            if (localLobbyPlayerModel == null) { localLobbyPlayerModel = GetLobbyCharacterModel(localLobbyPlayer.CJFLMDNNMIE); }
                            else
                            {
                                localLobbyPlayerModel.SetSilhouette(false);
                                if (localTex != null)
                                {
                                    AssignTextureToCharacterModelRenderers(localLobbyPlayerModel, localTex);
                                }
                            }
                            if (InLobby(GameType.Online))
                            {
                                if (opponentPlayer == null) { opponentPlayer = GetOpponentPlayerInLobby(); }
                                else
                                {
                                    doSkinGet = true;
                                    if (opponentLobbyCharacterModel == null) { opponentLobbyCharacterModel = GetLobbyCharacterModel(opponentPlayer.CJFLMDNNMIE); }
                                    else
                                    {
                                        opponentLobbyCharacterModel.SetSilhouette(false);
                                        if (opponentCustomTexture != null)
                                        {
                                            AssignTextureToCharacterModelRenderers(opponentLobbyCharacterModel, opponentCustomTexture);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (InGame(GameType.Any))
                    {
                        if (localGamePlayerEntity == null) { localGamePlayerEntity = GetLocalPlayerInGame(); }
                        else
                        {
                            try
                            {
                                if (sendCancelRequestToServer == true)
                                {
                                    StartCoroutine(TextureMod.Instance.ec.PostCancel(localLobbyPlayer.KLEEADMGHNE.peerId));
                                    sendCancelRequestToServer = false;
                                }
                            } catch { }
                            if (localTex != null)
                            {
                                AssignTextureToIngameCharacter(localGamePlayerEntity, localTex);
                                localPlayerName = AssignTextureToHud(localGamePlayerEntity, localTex);
                                if (localGamePlayerEntity.character == Character.GRAF)
                                {
                                    AssignToxicEffectColors(localGamePlayerEntity.player.CJFLMDNNMIE, localTex, localGamePlayerEntity.variant);
                                }
                                else if (localGamePlayerEntity.character == Character.BOSS && (localGamePlayerEntity.variant == CharacterVariant.MODEL_ALT3 || localGamePlayerEntity.variant == CharacterVariant.MODEL_ALT4))
                                {
                                    AssignDoomBoxVisualizerColorIngame(localGamePlayerEntity.player.CJFLMDNNMIE, localTex);
                                }
                            }
                        }
                        if (InGame(GameType.Online))
                        {
                            if (opponentCustomTexture != null)
                            {
                                if (opponentPlayerEntity == null) { opponentPlayerEntity = GetOpponentPlayerInGame(); }
                                else
                                {
                                    AssignTextureToIngameCharacter(opponentPlayerEntity, opponentCustomTexture);
                                    opponentPlayerName = AssignTextureToHud(opponentPlayerEntity, opponentCustomTexture);
                                    if (opponentPlayerEntity.character == Character.GRAF)
                                    {
                                        AssignToxicEffectColors(opponentPlayerEntity.player.CJFLMDNNMIE, opponentCustomTexture, opponentPlayerEntity.variant);
                                    }
                                    else if (opponentPlayerEntity.character == Character.BOSS && (opponentPlayerEntity.variant == CharacterVariant.MODEL_ALT3 || opponentPlayerEntity.variant == CharacterVariant.MODEL_ALT4))
                                    {
                                        AssignDoomBoxVisualizerColorIngame(opponentPlayerEntity.player.CJFLMDNNMIE, opponentCustomTexture);
                                    }
                                }
                            }
                        }
                    }
                    else if (InPostGame())
                    {
                        if (localWinnerModel == null) { localWinnerModel = GetWinnerModel(localPlayerChar, localPlayerCharVar); }
                        else { if (localTex != null) { AssignTextureToCharacterModelRenderers(localWinnerModel, localTex); } }
                        if (localTex != null) { AssignTextureToPostGameHud(localPlayerName, localTex); }
                        if (InGame(GameType.Online))
                        {
                            if (opponentCustomTexture != null)
                            {
                                if (opponentWinnerModel == null) { opponentWinnerModel = GetWinnerModel(opponentCustomSkinCharacter, opponentCustomSkinCharacterVariant); }
                                else { AssignTextureToCharacterModelRenderers(opponentWinnerModel, opponentCustomTexture); }
                                AssignTextureToPostGameHud(opponentPlayerName, opponentCustomTexture);
                            }
                        }
                    }
                    else
                    {
                        initLocalPlayer = true;
                        if (InGame(GameType.Online)) { initOpponentPlayer = true; }
                    }
                    break;
            }
            if (InMenu())
            {
                sendCancelRequestToServer = true;
                InitLocalPlayer();
                InitOpponentPlayer();
            }

            if (UIScreen.currentScreens[1].screenType == ScreenType.UNLOCKS_SKINS)
            {
                if ((Input.GetKey(holdKey1) || Input.GetKey(holdKey2)) && Input.GetKeyDown(setSkinKey))
                {
                    localTex = GetLoadedTextureForUnlocksModel(localTex);
                    SetUnlocksCharacterModel(localTex);
                }
            }
            else if (UIScreen.currentScreens[1].screenType == ScreenType.UNLOCKS_CHARACTERS)
            {
                localTex = null;
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                opponentPlayer = ALDOKEMAOMB.BJDPHEHJJJK(1);
                newSkinToApply = true;
                opponentCustomSkinCharacter = Character.CANDY;
                opponentCustomSkinCharacterVariant = CharacterVariant.ALT0;
            }
        }

        private bool InMenu()
        {
            if (gameState == (JOFJHDJHJGI)2)
            { return true; }
            else { return false; }
        }
        private bool InLobby(GameType gt)
        {
            switch(gt)
            {
                case GameType.Online:
                    if (gameState == (JOFJHDJHJGI)5)
                    { return true; }
                    break;
                case GameType.Offline:
                    if (gameState == (JOFJHDJHJGI)11 || gameState == (JOFJHDJHJGI)12 || gameState == (JOFJHDJHJGI)4 || gameState == (JOFJHDJHJGI)6 || gameState == (JOFJHDJHJGI)23)
                    { return true; }
                    break;
                case GameType.Any:
                    if (gameState == (JOFJHDJHJGI)11 || gameState == (JOFJHDJHJGI)12 || gameState == (JOFJHDJHJGI)5 || gameState == (JOFJHDJHJGI)6 || gameState == (JOFJHDJHJGI)4 || gameState == (JOFJHDJHJGI)23)
                    { return true; }
                    break;
            }
            return false;
        }
        private bool InGame(GameType gt)
        {
            switch (gt)
            { 
                case GameType.Online:
                    if (JOMBNFKIHIC.GDNFJCCCKDM && gameState == (JOFJHDJHJGI)19)
                    { return true; }
                    break;
                case GameType.Offline:
                    if ((!JOMBNFKIHIC.GDNFJCCCKDM) && gameState == (JOFJHDJHJGI)19)
                    { return true; }
                    break;
                case GameType.Any:
                    if ((!JOMBNFKIHIC.GDNFJCCCKDM || JOMBNFKIHIC.GDNFJCCCKDM) && gameState == (JOFJHDJHJGI)19)
                    { return true; }
                    break;
            }
            return false;
        }
        private bool InPostGame()
        {
            if ((currentGameMode == GameMode._1v1 || currentGameMode == GameMode.FREE_FOR_ALL) && gameState == (JOFJHDJHJGI)21)
            { return true; }
            else { return false; }
        }

        private ALDOKEMAOMB GetLocalPlayerInLobby(GameType gt)
        {
            ALDOKEMAOMB player = null;
            switch (gt)
            {
                case GameType.Online:
                    HDLIJDBFGKN gameStatesOnlineLobby = FindObjectOfType<HDLIJDBFGKN>();
                    if (gameStatesOnlineLobby != false)
                    {
                        return ALDOKEMAOMB.BJDPHEHJJJK(gameStatesOnlineLobby.HCCGAFLIABM);
                    }
                    break;
                case GameType.Offline:
                    return ALDOKEMAOMB.BJDPHEHJJJK(0);
            }
            return player;
        }
        private ALDOKEMAOMB GetOpponentPlayerInLobby()
        {
            ALDOKEMAOMB player = null;
            HDLIJDBFGKN gameStatesOnlineLobby = FindObjectOfType<HDLIJDBFGKN>();
            if (gameStatesOnlineLobby != false)
            {
                if (gameStatesOnlineLobby.HCCGAFLIABM == 0) { player = ALDOKEMAOMB.BJDPHEHJJJK(1); }
                else { player = ALDOKEMAOMB.BJDPHEHJJJK(0); }
            }
            return player;
        }

        public CharacterModel GetLobbyCharacterModel(int playerNr)
        {
            CharacterModel retmodel = null;
            ScreenPlayers screenPlayers = FindObjectOfType<ScreenPlayers>();
            foreach(PlayersSelection playersSelection in screenPlayers.playerSelections)
            {
                if (playersSelection.playerNr == playerNr)
                {
                    retmodel = playersSelection.characterModel;
                }
            }
            return retmodel;
        }

        public void AssignTextureToCharacterModelRenderers(CharacterModel model, Texture2D tex)
        {
            Renderer[] rs = model.curModel.transform.GetComponentsInChildren<Renderer>();
            if (rs.Length > 0)
            {
                foreach (Renderer r in rs)
                {
                    if (model.character == Character.BOSS && (model.characterVariant == CharacterVariant.MODEL_ALT3 || model.characterVariant == CharacterVariant.MODEL_ALT4))
                    {
                        AssignDoomBoxVisualizerColorToRenderer(r, tex);
                    }
                    r.material.SetTexture("_MainTex", tex);
                }
            }
        }

        private PlayerEntity GetLocalPlayerInGame()
        {
            return localLobbyPlayer.JCCIAMJEODH;
        }
        private PlayerEntity GetOpponentPlayerInGame()
        {
            return opponentPlayer.JCCIAMJEODH;
        }

        private void AssignTextureToIngameCharacter(PlayerEntity playerEntity, Texture tex)
        {
            VisualEntity ve = playerEntity.gameObject.GetComponent<VisualEntity>();
            if (ve != null)
            {
                if (ve.skinRenderers.Count > 0)
                {
                    foreach (Renderer r in ve.skinRenderers)
                    {
                        if (!r.gameObject.name.EndsWith("Outline"))
                        {
                            r.material.SetTexture("_MainTex", tex);
                        }
                    }
                }
            }
        }

        private string AssignTextureToHud(PlayerEntity playerEntity, Texture tex)
        {
            string ret = "";
            GameHudPlayerInfo[] ghpis = FindObjectsOfType<GameHudPlayerInfo>();
            if (ghpis.Length > 0)
            {
                foreach (GameHudPlayerInfo ghpi in ghpis)
                {
                    if (ghpi.shownPlayer == playerEntity.player)
                    {
                        Renderer[] rs = ghpi.gameObject.transform.GetComponentsInChildren<Renderer>();
                        if (rs.Length > 0)
                        {
                            foreach (Renderer r in rs)
                            {
                                r.material.SetTexture("_MainTex", tex);
                            }
                        }
                        ret = ghpi.lbName.text;
                    }
                }
            }
            return ret;
        }

        private CharacterModel GetWinnerModel(Character c, CharacterVariant cv)
        {
            CharacterModel winner = null;
            PostScreen ps = FindObjectOfType<PostScreen>();
            if (ps != null)
            {
                if (ps.winnerCharacter == c && ps.winnerCharacterVariant == cv)
                {
                    winner = ps.winnerModel;
                }
            }
            return winner;
        }

        private void AssignTextureToPostGameHud(string playerName, Texture tex)
        {
            PostScreen ps = FindObjectOfType<PostScreen>();
            PostSceenPlayerBar[] pspbs = ps.playerBarsByPlayer;
            foreach(PostSceenPlayerBar pspb in pspbs)
            {
                if (pspb.btPlayerName.GetText().Contains(playerName))
                {
                    Renderer[] rs = pspb.gameObject.transform.GetComponentsInChildren<Renderer>();
                    if (rs.Length > 0)
                    {
                        foreach (Renderer r in rs)
                        {
                            r.material.SetTexture("_MainTex", tex);
                        }
                    }
                }
            }
        }

        private void SetUnlocksCharacterModel(Texture tex)
        {
            CharacterModel[] cms = FindObjectsOfType<CharacterModel>();
            {
                if (cms.Length > 0)
                {
                    foreach(CharacterModel cm in cms)
                    {
                        cm.SetSilhouette(false);
                        Renderer[] rs = cm.curModel.transform.GetComponentsInChildren<Renderer>();
                        if (rs.Length > 0)
                        {
                            foreach (Renderer r in rs) { r.material.SetTexture("_MainTex", tex); }
                        }
                    }
                }
            }
        }

        private void InitLocalPlayer()
        {
            localLobbyPlayer = null;
            localLobbyPlayerModel = null;
            localGamePlayerEntity = null;
            localWinnerModel = null;
            localPlayerName = "";
            localTex = null;
            doSkinPost = false;
            postTimer = 0;
            initLocalPlayer = false;
        }
        private void InitOpponentPlayer()
        {
            opponentPlayer = null;
            opponentLobbyCharacterModel = null;
            opponentPlayerEntity = null;
            opponentWinnerModel = null;
            opponentPlayerName = "";
            opponentCustomTexture = null;
            doSkinGet = false;
            getTimer = 0;
            initOpponentPlayer = false;
        }

        private Texture2D GetLoadedTexture(Character c, Texture2D currentTexture)
        {
            Texture2D ret = null;
            if (currentTexture == null && TextureMod.Instance.tl.characterTextures[c].ElementAt(0).Value != null)
            {
                localLobbyPlayer.AIINAIDBHJI = GetVariantFromName(c, TextureMod.Instance.tl.characterTextures[c].ElementAt(0).Key);
                localLobbyPlayerModel.SetCharacterLobby(localLobbyPlayer.CJFLMDNNMIE, c, GetVariantFromName(c, TextureMod.Instance.tl.characterTextures[c].ElementAt(0).Key), false);
                localPlayerCharVar = GetVariantFromName(c, TextureMod.Instance.tl.characterTextures[c].ElementAt(0).Key);
                ret = TextureMod.Instance.tl.characterTextures[c].ElementAt(0).Value;
            } else
            {
                bool retnext = false;
                foreach(KeyValuePair<string, Texture2D> pair in TextureMod.Instance.tl.characterTextures[c])
                {
                    if (retnext == true)
                    {
                        localLobbyPlayer.AIINAIDBHJI = GetVariantFromName(c, pair.Key);
                        localLobbyPlayerModel.SetCharacterLobby(localLobbyPlayer.CJFLMDNNMIE, c, GetVariantFromName(c, pair.Key), false);
                        localPlayerCharVar = GetVariantFromName(c, pair.Key);
                        ret = pair.Value;
                        break;
                    } else if (retnext == false && currentTexture == pair.Value)
                    {
                        retnext = true;
                        if (currentTexture == TextureMod.Instance.tl.characterTextures[c].Last().Value)
                        {
                            localLobbyPlayer.AIINAIDBHJI = GetVariantFromName(c, TextureMod.Instance.tl.characterTextures[c].ElementAt(0).Key);
                            localLobbyPlayerModel.SetCharacterLobby(localLobbyPlayer.CJFLMDNNMIE, c, GetVariantFromName(c, TextureMod.Instance.tl.characterTextures[c].ElementAt(0).Key), false);
                            localPlayerCharVar = GetVariantFromName(c, TextureMod.Instance.tl.characterTextures[c].ElementAt(0).Key);
                            ret = TextureMod.Instance.tl.characterTextures[c].ElementAt(0).Value;
                        }
                    }
                }
            }
            localPlayerChar = c;
            return ret;
        }
        private Texture2D GetLoadedTextureForUnlocksModel(Texture2D currentTexture)
        {
            Texture2D ret = null;
            ScreenUnlocksSkins sus = FindObjectOfType<ScreenUnlocksSkins>();
            if (sus != null)
            {
                if (currentTexture == null && TextureMod.Instance.tl.characterTextures[sus.character].ElementAt(0).Value != null)
                {
                    sus.ShowCharacter(sus.character, GetVariantFromName(sus.character, TextureMod.Instance.tl.characterTextures[sus.character].ElementAt(0).Key), false);
                    ret = TextureMod.Instance.tl.characterTextures[sus.character].ElementAt(0).Value;
                }
                else
                {
                    bool retnext = false;
                    foreach (KeyValuePair<string, Texture2D> pair in TextureMod.Instance.tl.characterTextures[sus.character])
                    {
                        if (retnext == true)
                        {
                            sus.ShowCharacter(sus.character, GetVariantFromName(sus.character, pair.Key), false);
                            ret = pair.Value;
                            break;
                        }
                        else if (retnext == false && currentTexture == pair.Value)
                        {
                            retnext = true;
                            if (currentTexture == TextureMod.Instance.tl.characterTextures[sus.character].Last().Value)
                            {
                                sus.ShowCharacter(sus.character, GetVariantFromName(sus.character, TextureMod.Instance.tl.characterTextures[sus.character].ElementAt(0).Key), false);
                                ret = TextureMod.Instance.tl.characterTextures[sus.character].ElementAt(0).Value;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        private CharacterVariant GetVariantFromName(Character localPlayerC, string name)
        {
            Character c = Character.NONE;
            CharacterVariant cv = CharacterVariant.DEFAULT;
            if (opponentPlayer != null)
            {
                c = opponentPlayer.DOFCCEDJODB;
                cv = opponentPlayer.AIINAIDBHJI;
            }
            if (name.Contains("_ALT2"))
            {
                if (localPlayerC != c)
                {
                    return CharacterVariant.MODEL_ALT3;
                } else
                {
                    if (cv == CharacterVariant.MODEL_ALT3)
                    {
                        return CharacterVariant.MODEL_ALT4;
                    } else
                    {
                        return CharacterVariant.MODEL_ALT3;
                    }
                }
            }
            else if (name.Contains("_ALT"))
            {
                if (localPlayerC != c)
                {
                    return CharacterVariant.MODEL_ALT;
                }
                else
                {
                    if (cv == CharacterVariant.MODEL_ALT)
                    {
                        return CharacterVariant.MODEL_ALT2;
                    }
                    else
                    {
                        return CharacterVariant.MODEL_ALT;
                    }
                }
            }
            else
            {
                if (localPlayerC != c)
                {
                    return CharacterVariant.DEFAULT;
                }
                else
                {
                    if (cv == CharacterVariant.DEFAULT)
                    {
                        return CharacterVariant.ALT0;
                    }
                    else
                    {
                        return CharacterVariant.DEFAULT;
                    }
                }
            }
        }

        private enum GameType
        {
            Online = 0,
            Offline = 1,
            Any = 2
        }

        #region Texture Manipulation [Toxic effects, Doombox visualiser, Texture Grayscale]
        public void MakeTextureGrayscale(Texture2D tex)
        {
            var texColors = tex.GetPixels();
            for (var i = 0; i < texColors.Length; i++)
            {
                var grayValue = texColors[i].grayscale;
                texColors[i] = new Color(grayValue, grayValue, grayValue, texColors[i].a);
            }
            tex.SetPixels(texColors);
            tex.Apply();
        }
        public void AssignToxicEffectColors(int playerId, Texture2D tex, CharacterVariant cv)
        {
            GrafPlayer[] toxics = FindObjectsOfType<GrafPlayer>();
            foreach (GrafPlayer toxic in toxics)
            {
                if (toxic.player.CJFLMDNNMIE == playerId)
                {
                    Color32 c = tex.GetPixel(258, 345);
                    c.a = byte.MaxValue;
                    toxic.GetVisual("paintBlobVisual").mainRenderer.material.color = c;
                    switch (cv)
                    {
                        case CharacterVariant.DEFAULT:
                            originalGrafColor = toxic.outfitEffectColors[0];
                            toxic.outfitEffectColors[0] = c;
                            break;
                        case CharacterVariant.ALT0:
                            originalGrafColor = toxic.outfitEffectColors[1];
                            toxic.outfitEffectColors[1] = c;
                            break;
                        case CharacterVariant.MODEL_ALT:
                            originalGrafColor = toxic.outfitEffectColors[8];
                            toxic.outfitEffectColors[8] = c;
                            break;
                        case CharacterVariant.MODEL_ALT2:
                            originalGrafColor = toxic.outfitEffectColors[9];
                            toxic.outfitEffectColors[9] = c;
                            break;
                    }
                }
            }
        }
        public void AssignDoomBoxVisualizerColorIngame(int playerId, Texture2D tex)
        {
            BossPlayerModel[] dbs = FindObjectsOfType<BossPlayerModel>();
            foreach (BossPlayerModel db in dbs)
            {
                Color c1 = tex.GetPixel(493, 510);
                Color c2 = tex.GetPixel(508, 510);
                if (db.player.CJFLMDNNMIE == playerId)
                {
                    FNDGCLEDHAD visualizer = db.GetVisual("main").mainRenderer.gameObject.GetComponent<FNDGCLEDHAD>();
                    Material vismat = visualizer.FHAMOPAJHNJ;
                    vismat.SetColor("_AmpColor0", c1);
                    vismat.SetColor("_AmpColor1", c2);
                }
            }
        }
        public void AssignDoomBoxVisualizerColorToRenderer(Renderer r, Texture2D tex)
        {
            if (r.gameObject.GetComponent<FNDGCLEDHAD>() != null)
            {
                Color c1 = tex.GetPixel(493, 510);
                Color c2 = tex.GetPixel(508, 510);
                FNDGCLEDHAD visualizer = r.gameObject.GetComponent<FNDGCLEDHAD>();
                Material vismat = visualizer.FHAMOPAJHNJ;
                vismat.SetColor("_AmpColor0", c1);
                vismat.SetColor("_AmpColor1", c2);
            }
        }
        #endregion
    }
}

