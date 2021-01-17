using LLScreen;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureMod
{
    public class ShowcaseStudio : MonoBehaviour
    {
        KeyCode enterShowcaseStudio = KeyCode.Tab;
        KeyCode showcaseStudioHideHud = KeyCode.F3;
        KeyCode showcaseStudioRotateCharacter = KeyCode.Mouse0;
        KeyCode showcaseStudioMoveLight = KeyCode.Mouse3;
        KeyCode showcaseStudioMoveCamera = KeyCode.Mouse2;

        public bool showUI = true;
        public string skinName = "N/A";
        public int refreshTimer = 0;
        public bool refreshMode = false;
        bool hideGUI = false;
        bool showControls = false;
        ScreenUnlocksSkins SUS;
        List<GameObject> gameObjects = new List<GameObject>();
        GameObject cameraController;
        Camera camControllerCam;
        Camera lightControllerCam;
        GameObject lightController;
        CharacterModel characterModel;
        Shader mainShader;
        Vector3 originalCharacterRendPos;
        bool enableLight = true;
        bool bgColorSelection = false;
        byte bgR = 50;
        byte bgG = 50;
        byte bgB = 50;

        //Animation Selection
        Vector2 animSelectionPos = Vector2.zero;
        public List<string> animList = new List<string>();
        string currentAnimation = "idle";
        float animationSpeed = 60;
        int selectedGridAnim = 0;
        float animationTime;
        float animationPos = 0;
        bool animate = true;

        private void OnGUI()
        {
            GUI.skin.box.alignment = TextAnchor.MiddleCenter;

            if (!showUI && !hideGUI)
            {
                GUI.skin.box.fontSize = 18;
                GUI.Window(777, new Rect(10, 10, Screen.width / 7, Screen.height - 20), new GUI.WindowFunction(AnimationSelectionWindow), "Showcase Studio", CustomStyle.windStyle);
                if (showControls) GUI.Window(778, new Rect(20 + (Screen.width / 7), 10, 10 + ((Screen.width / 7) * 2), Screen.height / 3), new GUI.WindowFunction(ControlsWindow), "Controls", CustomStyle.windStyle);
            }
            else
            {
                if (SUS != null && !hideGUI)
                {
                    GUI.skin.box.fontSize = 22;
                    GUI.Box(new Rect(10, 10, 600, 35), "Press [" + enterShowcaseStudio + "] to enter Showcase Studio");
                }
            }
        }

        bool initCustomStyle = false;

        void Start()
        {

        }

        private void Update()
        {
            if (SUS == null)
            {
                SUS = FindObjectOfType<ScreenUnlocksSkins>();
                if (initCustomStyle == false)
                {
                    CustomStyle.InitStyle();
                    initCustomStyle = true;
                }
            }
            else
            {

                if (mainShader == null) mainShader = Shader.Find("LethalLeague/GameplayOpaque");

                if (Input.GetKeyDown(showcaseStudioHideHud)) hideGUI = !hideGUI;

                Renderer[] rends = FindObjectsOfType<Renderer>();
                foreach (Renderer r in rends)
                {
                    if (r.material.shader != mainShader && r.name.Contains("Effect")) r.material.shader = mainShader;
                }

                if (Input.GetKeyDown(enterShowcaseStudio) || (!showUI && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))))
                {
                    showUI = !showUI;
                    GameObject[] gos = FindObjectsOfType<GameObject>();
                    if (!showUI)
                    {

                        originalCharacterRendPos = SUS.characterRenderer.transform.position;
                        SUS.characterRenderer.transform.position = new Vector3(0, 0, 0);

                        foreach (GameObject go in gos)
                        {
                            if (go.name.Contains("btFirst")) gameObjects.Add(go);
                            if (go.name.Contains("btQuit")) gameObjects.Add(go);
                            if (go.name.Contains("btPose")) gameObjects.Add(go);

                            if (go.name.Contains("characterCamera"))
                            {
                                go.GetComponent<Camera>().enabled = false;
                            }

                            if (go.name.Contains("cameraController"))
                            {
                                cameraController = new GameObject("cameraControllerGentle");
                                cameraController.transform.position = new Vector3(-2.5f, -302f, -4.4f);
                                cameraController.transform.eulerAngles = new Vector3(3f, 132f, 0f);
                                camControllerCam = cameraController.AddComponent<Camera>();
                                cameraController.GetComponent<Camera>().enabled = true;
                                cameraController.AddComponent<SmoothMouseLook>();
                            }

                            if (go.name.Contains("characterLight"))
                            {
                                lightController = new GameObject("lightControllerGentle");
                                foreach (GameObject go2 in gos)
                                {
                                    if (go2.name.Contains("characterCamera"))
                                    {
                                        lightController.transform.position = go2.transform.position;
                                        lightController.transform.rotation = go2.transform.rotation;
                                    }
                                }
                                Light l = lightController.AddComponent<Light>();
                                Light cl = go.GetComponent<Light>();
                                l.type = cl.type;
                                l.color = cl.color;
                                lightControllerCam = lightController.AddComponent<Camera>();
                                lightController.AddComponent<SmoothMouseLook>();
                                go.GetComponent<Light>().enabled = false;
                            }
                        }

                        for (var i = 0; i < gameObjects.Count; i++) gameObjects[i].SetActive(false);

                        if (characterModel == null) characterModel = SUS.previewModel;
                        else
                        {
                            Animation anim = characterModel.gameObject.GetComponentInChildren<Animation>();
                            var idleIndex = 0;
                            foreach (AnimationState state in anim)
                            {
                                animList.Add(state.name);
                                if (state.name == "idle") selectedGridAnim = idleIndex;
                                idleIndex++;
                            }
                        }
                    }
                    else //If showui
                    {
                        SUS.characterRenderer.transform.position = originalCharacterRendPos;
                        for (var i = 0; i < gameObjects.Count; i++) gameObjects[i].SetActive(true);
                        gameObjects.Clear();
                        animList.Clear();

                        if (lightController != null) Destroy(lightController);
                        if (cameraController != null) Destroy(cameraController);

                        foreach (GameObject go in gos)
                        {
                            if (go.name.Contains("characterCamera"))
                            {
                                go.GetComponent<Camera>().enabled = true;
                            }

                            if (go.name.Contains("characterLight"))
                            {
                                go.GetComponent<Light>().enabled = true;
                            }
                        }
                    }
                }

                if (characterModel == null) characterModel = SUS.previewModel;
                else
                {
                    Animation anim = characterModel.gameObject.GetComponentInChildren<Animation>();
                    if (anim.clip.name != currentAnimation && !showUI)
                    {
                        anim.clip = anim[currentAnimation].clip;
                    }

                    if (animate)
                    {
                        if (animationTime >= anim[currentAnimation].length) animationTime = 0f;
                        animationTime += (animationSpeed / 60f) * Time.deltaTime;
                        anim[currentAnimation].normalizedTime = animationTime / anim[currentAnimation].length;
                    }
                    else
                    {
                        anim[currentAnimation].normalizedTime = animationPos;
                    }
                }

                if (lightController != null) lightController.GetComponent<Light>().enabled = enableLight;

                if (camControllerCam != null) camControllerCam.backgroundColor = new Color32(bgR, bgG, bgB, 255);
                if (lightControllerCam != null) lightControllerCam.backgroundColor = new Color(0.5f, 0.5f, 0.5f);
            }
        }

        private void FixedUpdate()
        {
            if (TextureMod.Instance.MMI != null)
            {
                if (TextureMod.Instance.tc.InMenu())
                {
                    var mmi = TextureMod.Instance.MMI;
                    enterShowcaseStudio = mmi.GetKeyCode(mmi.configKeys["(key)enterShowcaseStudio"]);
                    showcaseStudioHideHud = mmi.GetKeyCode(mmi.configKeys["(key)showcaseStudioHideHud"]);
                    showcaseStudioRotateCharacter = mmi.GetKeyCode(mmi.configKeys["(key)showcaseStudioRotateCharacter"]);
                    showcaseStudioMoveCamera = mmi.GetKeyCode(mmi.configKeys["(key)showcaseStudioMoveCamera"]);
                    showcaseStudioMoveLight = mmi.GetKeyCode(mmi.configKeys["(key)showcaseStudioMoveLight"]);
                }
            }

            if (cameraController != null)
            {
                if (Input.GetKey(showcaseStudioMoveCamera)) cameraController.GetComponent<SmoothMouseLook>().isActive = true;
                else cameraController.GetComponent<SmoothMouseLook>().isActive = false;
            }

            if (lightController != null)
            {
                if (Input.GetKey(showcaseStudioMoveLight))
                {
                    lightControllerCam.enabled = true;
                    camControllerCam.enabled = false;
                    lightController.GetComponent<SmoothMouseLook>().isActive = true;
                }
                else
                {
                    lightControllerCam.enabled = false;
                    camControllerCam.enabled = true;
                    lightController.GetComponent<SmoothMouseLook>().isActive = false;
                }
            }

            if (characterModel != null)
            {
                if (Input.GetKey(showcaseStudioRotateCharacter))
                {
                    int speed = 3;
                    if (Input.GetKey(KeyCode.A))
                    {
                        characterModel.transform.Rotate(Vector3.up * speed);
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        characterModel.transform.Rotate(Vector3.right * speed);
                        characterModel.transform.Rotate(Vector3.forward * speed);
                    }
                    if (Input.GetKey(KeyCode.W))
                    {
                        characterModel.transform.Rotate(-Vector3.right * speed);
                        characterModel.transform.Rotate(-Vector3.forward * speed);
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        characterModel.transform.Rotate(-Vector3.up * speed);
                    }
                    if (Input.GetKey(KeyCode.Q))
                    {
                        characterModel.transform.Rotate(Vector3.right * speed);
                        characterModel.transform.Rotate(-Vector3.forward * speed);
                    }
                    if (Input.GetKey(KeyCode.E))
                    {
                        characterModel.transform.Rotate(-Vector3.right * speed);
                        characterModel.transform.Rotate(Vector3.forward * speed);
                    }
                }
            }
        }

        private void AnimationSelectionWindow(int winID)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(30);
            GUILayout.Box("General Options");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Show controls: ");
            if (GUILayout.Button(showControls.ToString()))
            {
                showControls = !showControls;
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Skin Name: ");
            GUILayout.Label(skinName);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Skin Refresh: ");
            if (!refreshMode) GUILayout.Label("Off");
            else GUILayout.Label("On [" + refreshTimer.ToString() + "]");
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.Box("BG and Lighting Options");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Enable Lighting: ");
            if (GUILayout.Button(enableLight.ToString()))
            {
                enableLight = !enableLight;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Show BG Color select: ");
            if (GUILayout.Button(bgColorSelection.ToString()))
            {
                bgColorSelection = !bgColorSelection;
            }
            GUILayout.EndHorizontal();

            if (bgColorSelection)
            {
                GUILayout.Label("RGB: [" + bgR.ToString() + ", " + bgG.ToString() + ", " + bgB.ToString() + "]");
                bgR = (byte)GUILayout.HorizontalSlider(bgR, 0f, 255f);
                bgG = (byte)GUILayout.HorizontalSlider(bgG, 0f, 255f);
                bgB = (byte)GUILayout.HorizontalSlider(bgB, 0f, 255f);
            }


            GUILayout.Space(20);

            GUILayout.Box("Animation Options");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Current Animation:");
            GUILayout.Label(currentAnimation);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (characterModel != null)
            {
                Animation anim = characterModel.gameObject.GetComponentInChildren<Animation>();
                GUILayout.Label("Animation time:");
                GUILayout.Label(Decimal.Round((decimal)animationTime, 2) + "s / " + Decimal.Round((decimal)anim[currentAnimation].length, 2).ToString() + "s");
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Play Animation");
            if (GUILayout.Button(animate.ToString()))
            {
                animate = !animate;
            }
            GUILayout.EndHorizontal();

            if (animate)
            {
                GUILayout.Space(5);
                GUILayout.Label("Animation speed: " + Decimal.Round((decimal)animationSpeed).ToString() + " FPS");
                animationSpeed = GUILayout.HorizontalSlider(animationSpeed, 0f, 60f);
                GUILayout.Space(5);
            }
            else
            {
                GUILayout.Space(5);
                GUILayout.Label("Animation step:");
                animationPos = GUILayout.HorizontalSlider(animationPos, 0f, 1f);
                GUILayout.Space(5);
            }

            animSelectionPos = GUILayout.BeginScrollView(animSelectionPos, false, true);
            if (animList.Count > 0) selectedGridAnim = GUILayout.SelectionGrid(selectedGridAnim, animList.ToArray(), 1);

            currentAnimation = animList[selectedGridAnim];

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void ControlsWindow(int wid)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(30);
                GUILayout.Box("General");
                GUILayout.Label("Hide GUI: [" + showcaseStudioHideHud + "]");
                GUILayout.Label("Exit ShowcaseStudio: [" + enterShowcaseStudio + ", ESC or Right Click]");
                GUILayout.Label("Reload current skin [" + TextureMod.Instance.tc.reloadCustomSkin + "]");
                GUILayout.Label("Reimport skin library [" + TextureMod.Instance.tc.reloadEntireSkinLibrary + "]");

                GUILayout.Space(20);

                GUILayout.Box("Camera Movement");
                GUILayout.Label("Hold [" + showcaseStudioMoveCamera + "] and move the mouse to tilt the camera (Bindable in mod settings)");
                GUILayout.Label("While holding [" + showcaseStudioMoveCamera + "] you can press WASD, left shift and space to move the camera around");

                GUILayout.Space(20);

                GUILayout.Box("Light Movement");
                GUILayout.Label("Hold [" + showcaseStudioMoveLight + "] and move the mouse to tilt the light (Bindable in mod settings)");
                GUILayout.Label("While holding [" + showcaseStudioMoveLight + "] you can press WASD, left shift and space to move the light around");

                GUILayout.Space(20);

                GUILayout.Box("Character Rotation");
                GUILayout.Label("Hold [" + showcaseStudioRotateCharacter + "] (Bindable in mod settings) and press WASD, left shift and space to rotate the model");

            }
            GUILayout.EndVertical();
        }
    }
}
