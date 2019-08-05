using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
struct Words {
    public string[] values;
}
public class JsonLoader : MonoBehaviour {
    private static string gameDataFileName = "dictionary.json";
    [SerializeField]

    public static string[] LoadDictionary() {
        var textFile = Resources.Load<TextAsset>("dictionary");
        return JsonUtility.FromJson<Words>(textFile.ToString()).values;
    }
}
