using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TextureMod
{
    public class ModDebugging : MonoBehaviour
    {
        public Dictionary<string, Dictionary<string, string>> windows = new Dictionary<string, Dictionary<string, string>>();
        public Dictionary<string, Rect> windowRects = new Dictionary<string, Rect>();

        private void OnGUI()
        {
            if (TextureMod.Instance.tc.showDebugInfo)
            {
                GUIStyle ss = new GUIStyle("Box");
                GUI.skin.window = ss;
                GUI.skin.window.fontSize = 20;
                GUI.skin.window.padding.top = 35;
                GUI.skin.window.contentOffset = new Vector2(0, -25f);
                GUI.skin.window.alignment = TextAnchor.UpperCenter;
                int i = 909090;
                foreach (KeyValuePair<string, Dictionary<string, string>> window in windows)
                {
                    int size = -1;
                    string str = "";
                    foreach (KeyValuePair<string, string> values in window.Value)
                    {
                        var s = values.Key + " = " + values.Value;
                        if (s.Length > size)
                        {
                            size = s.Length;
                            str = s;
                        }
                    }
                    GUIContent guiContent = new GUIContent(str);

                    if (!windowRects.ContainsKey(window.Key)) windowRects.Add(window.Key, new Rect(0, 0, 100, 20));

                    windowRects[window.Key] = GUILayout.Window(i, new Rect(windowRects[window.Key].x, windowRects[window.Key].y, GUI.skin.window.CalcSize(guiContent).x + 100, windowRects[window.Key].height), new GUI.WindowFunction(DebugWindow), window.Key);

                    i++;
                }
            }
        }

        private void DebugWindow(int _windowId)
        {
            Dictionary<string, string> values = windows.ElementAt(_windowId - 909090).Value;
            GUI.skin.label.fontSize = 16;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            {
                foreach (KeyValuePair<string, string> pair in values)
                {
                    GUILayout.BeginHorizontal();
                    {
                        var str = pair.Key + " = " + pair.Value;
                        GUILayout.Label(str);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        public void AddToWindow(string windowName, string varName, string varValue)
        {
            if (windows.ContainsKey(windowName))
            {
                if (windows[windowName].ContainsKey(varName)) windows[windowName][varName] = varValue;
                else windows[windowName].Add(varName, varValue);
            } else
            {
                windows.Add(windowName, new Dictionary<string, string>());
                windows[windowName].Add(varName, varValue);
            }
        }

        private Texture2D ColorToTexture2D(Color color)
        {
            Texture2D tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, color);
            tex.Apply();
            return tex;
        }

    }
}
