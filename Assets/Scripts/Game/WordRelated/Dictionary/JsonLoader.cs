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
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if(File.Exists(filePath)) {
            string dataAsJson = File.ReadAllText(filePath); 
            Words loadedData = JsonUtility.FromJson<Words>(dataAsJson);
            return loadedData.values;
        }
        else {
            Debug.LogError("Cannot load word data!");
            return null;
        }
    }
}
