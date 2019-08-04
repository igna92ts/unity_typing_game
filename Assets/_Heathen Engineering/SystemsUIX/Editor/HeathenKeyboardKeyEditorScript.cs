using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeathenEngineering.UIX.KeyboardKey))]
public class HeathenKeyboardKeyEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        HeathenEngineering.UIX.KeyboardKey key = target as HeathenEngineering.UIX.KeyboardKey;
        key.keyType = (HeathenEngineering.UIX.KeyboardKeyType)EditorGUILayout.EnumPopup("Type", key.keyType);

        if (key.keyType == HeathenEngineering.UIX.KeyboardKeyType.WhiteSpace)
        {
            if (!key.EditorParseKeyCode)
            {
                key.EditorParseKeyCode = true;
                key.keyType = key.keyGlyph.DefaultFromCode(key.keyGlyph.code);
                EditorUtility.SetDirty(key);
            }
        }

        key.EditorParseKeyCode = EditorGUILayout.ToggleLeft("Parse return values from key code?", key.EditorParseKeyCode);
        KeyCode previousCode = (KeyCode)EditorGUILayout.EnumPopup("Code", key.keyGlyph.code);

        if(previousCode == KeyCode.Space || previousCode == KeyCode.Tab)
        {
            key.keyGlyph.code = previousCode;
            key.keyType = key.keyGlyph.DefaultFromCode(key.keyGlyph.code);

            EditorGUILayout.HelpBox("White space key codes automatically parse the return value based on the provided code.", MessageType.Info);

            if (key.keyGlyph.normalDisplay != null)
                EditorUtility.SetDirty(key.keyGlyph.normalDisplay);

            if (key.keyGlyph.shiftedDisplay != null)
                EditorUtility.SetDirty(key.keyGlyph.shiftedDisplay);

            if (key.keyGlyph.altGrDisplay != null)
                EditorUtility.SetDirty(key.keyGlyph.altGrDisplay);

            if (key.keyGlyph.shiftedAltGrDisplay != null)
                EditorUtility.SetDirty(key.keyGlyph.shiftedAltGrDisplay);
        }
        else if(key.keyGlyph.code != previousCode && key.EditorParseKeyCode)
        {
            key.keyGlyph.code = previousCode;
            key.keyType = key.keyGlyph.DefaultFromCode(key.keyGlyph.code);

            if(key.keyGlyph.normalDisplay != null)
                EditorUtility.SetDirty(key.keyGlyph.normalDisplay);

            if (key.keyGlyph.shiftedDisplay != null)
                EditorUtility.SetDirty(key.keyGlyph.shiftedDisplay);

            if (key.keyGlyph.altGrDisplay != null)
                EditorUtility.SetDirty(key.keyGlyph.altGrDisplay);

            if (key.keyGlyph.shiftedAltGrDisplay != null)
                EditorUtility.SetDirty(key.keyGlyph.shiftedAltGrDisplay);
        }
        else
            key.keyGlyph.code = previousCode;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("State Glyphs", EditorStyles.boldLabel);
        key.keyGlyph.normal = EditorGUILayout.ObjectField("Normal", key.keyGlyph.normal, typeof(RectTransform), true) as RectTransform;
        if (key.keyGlyph.normal != null)
        {
            key.keyGlyph.normalDisplay = EditorGUILayout.ObjectField("Text", key.keyGlyph.normalDisplay, typeof(UnityEngine.UI.Text), true) as UnityEngine.UI.Text;
            if (!key.EditorParseKeyCode)
            {
                EditorGUILayout.LabelField("Return Value:");
                key.keyGlyph.normalString = EditorGUILayout.TextArea(key.keyGlyph.normalString);
            }
        }
        EditorGUILayout.Space(); 
        key.keyGlyph.shifted = EditorGUILayout.ObjectField("On Shift", key.keyGlyph.shifted, typeof(RectTransform), true) as RectTransform;
        if (key.keyGlyph.shifted != null)
        {
            key.keyGlyph.shiftedDisplay = EditorGUILayout.ObjectField("Text", key.keyGlyph.shiftedDisplay, typeof(UnityEngine.UI.Text), true) as UnityEngine.UI.Text;
            if (!key.EditorParseKeyCode)
            {
                EditorGUILayout.LabelField("Return Value:");
                key.keyGlyph.shiftedString = EditorGUILayout.TextArea(key.keyGlyph.shiftedString);
            }
        }
        EditorGUILayout.Space();
        key.keyGlyph.altGr = EditorGUILayout.ObjectField("On AltGr", key.keyGlyph.altGr, typeof(RectTransform), true) as RectTransform;
        if (key.keyGlyph.altGr != null)
        {
            key.keyGlyph.altGrDisplay = EditorGUILayout.ObjectField("Text", key.keyGlyph.altGrDisplay, typeof(UnityEngine.UI.Text), true) as UnityEngine.UI.Text;
            if (!key.EditorParseKeyCode)
            {
                EditorGUILayout.LabelField("Return Value:");
                key.keyGlyph.altGrString = EditorGUILayout.TextArea(key.keyGlyph.altGrString);
            }
        }
        EditorGUILayout.Space();
        key.keyGlyph.shiftedAltGr = EditorGUILayout.ObjectField("On Shift + AltGr", key.keyGlyph.shiftedAltGr, typeof(RectTransform), true) as RectTransform;
        if (key.keyGlyph.shiftedAltGr != null)
        {
            key.keyGlyph.shiftedAltGrDisplay = EditorGUILayout.ObjectField("Text", key.keyGlyph.shiftedAltGrDisplay, typeof(UnityEngine.UI.Text), true) as UnityEngine.UI.Text;
            if (!key.EditorParseKeyCode)
            {
                EditorGUILayout.LabelField("Return Value:");
                key.keyGlyph.shiftedAltGrString = EditorGUILayout.TextArea(key.keyGlyph.shiftedAltGrString);
            }
        }

        EditorUtility.SetDirty(key);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Events");
        var p = serializedObject.FindProperty("pressed");
        EditorGUILayout.PropertyField(p);
        p = serializedObject.FindProperty("isDown");
        EditorGUILayout.PropertyField(p);
        p = serializedObject.FindProperty("isUp");
        EditorGUILayout.PropertyField(p);
        p = serializedObject.FindProperty("isClicked");
        EditorGUILayout.PropertyField(p);

        serializedObject.ApplyModifiedProperties();
    }
}
