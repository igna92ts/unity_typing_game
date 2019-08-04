using UnityEngine;
using System.Collections.Generic;
using System;

namespace HeathenEngineering.UIX
{
    [CreateAssetMenu(menuName = "Keyboard/Key Layout")]
    public class KeyLayout : ScriptableObject
    {
        [Multiline()]
        public string Notes;
        public List<KeyRecord> Keys;

        public void ApplyTo(Keyboard keyboard, KeyboardKey keyTemplate)
        {
            if (keyboard == null || keyTemplate == null)
                return;

            while(keyboard.keys.Count > 0)
            {
                var target = keyboard.keys[0];
                keyboard.keys.Remove(target);

                try
                {
#if UNITY_EDITOR
                    if (Application.IsPlaying(target.gameObject))
                        Destroy(target.gameObject);
                    else
                        DestroyImmediate(target.gameObject);
#else
                Destroy(target.gameObject);
#endif
                }
                catch { }
            }

            keyboard.keys.Clear();

            var transform = keyboard.GetComponent<Transform>();

            int index = 0;
            foreach(var key in Keys)
            {
                var GO = Instantiate(keyTemplate.gameObject, transform);
                GO.name = "["+index.ToString()+"] '" + key.keyGlyph.shifted + "' Key";
                var keyCom = GO.GetComponent<KeyboardKey>();
                keyCom.EditorParseKeyCode = key.parseFromKeyCode;
                keyCom.keyGlyph.Set(key.keyGlyph);
                keyCom.keyType = key.keyType;
                keyCom.selfRectTransform = keyCom.gameObject.GetComponent<RectTransform>();
                keyCom.selfRectTransform.sizeDelta = key.Size;
                keyCom.selfRectTransform.localPosition = key.Position;
                keyCom.selfRectTransform.localEulerAngles = key.Rotation;
                keyCom.selfRectTransform.localScale = key.Scale;
                GO.SetActive(true);
                keyboard.RegisterKey(keyCom);
                index++;
            }
        }

        public static KeyLayout CreateLayout(Keyboard keyboard)
        {
            if (keyboard == null)
                return null;

            UpdateKeys(keyboard);
            var keyLayout = CreateInstance<KeyLayout>();
            keyLayout.Notes = "Generated from [" + keyboard.name + "] on " + DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss");
            keyLayout.Keys = new List<KeyRecord>();
            var transform = keyboard.GetComponent<Transform>();
            foreach (var key in keyboard.keys)
            {
                keyLayout.Keys.Add(new KeyRecord(transform, key));
            }

            return keyLayout;
        }

        #if UNITY_EDITOR
        public static void SaveLayout(KeyLayout layout, string name, string assetPath)
        {
            if (layout == null)
                return;

            if (assetPath.StartsWith("../"))
                assetPath = assetPath.Replace("../", "");

            if (assetPath.StartsWith("./"))
                assetPath = assetPath.Replace("./", "");

            if (!assetPath.StartsWith("Assets"))
                assetPath = "Assets/" + assetPath;

            if (!assetPath.EndsWith("/"))
                assetPath = assetPath + "/";

            if (!name.EndsWith(".asset"))
                name = name + ".asset";

            UnityEditor.AssetDatabase.CreateAsset(layout, assetPath + name);
            Debug.Log("New layout saved to " + assetPath + name);
            UnityEditor.EditorGUIUtility.PingObject(layout);
        }

        public static void SaveLayout(Keyboard keyboard, string name, string assetPath)
        {
            if (keyboard == null)
                return;

            SaveLayout(CreateLayout(keyboard), name, assetPath);
        }
        #endif

        public static int UpdateKeys(Keyboard keyboard)
        {
            if (keyboard == null)
                return 0;

            List<KeyboardKey> currentKeys = keyboard.keys;

            if (keyboard.keys != null)
                keyboard.keys.Clear();
            else
                keyboard.keys = new List<KeyboardKey>();

            var keys = keyboard.gameObject.GetComponentsInChildren<KeyboardKey>();
            int newKeysFound = 0;

            foreach (var k in keys)
            {
                if (k.gameObject.activeInHierarchy)
                {
                    if (!currentKeys.Contains(k))
                        newKeysFound++;

                    keyboard.RegisterKey(k);
                }
            }

            Debug.Log(newKeysFound.ToString() + " new keys added to the [" + keyboard.name + "] keyboard");

            return newKeysFound;
        }
    }
}
