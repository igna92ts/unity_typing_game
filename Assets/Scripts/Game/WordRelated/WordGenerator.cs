using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGenerator : MonoBehaviour {
    private static string[] wordList;
    public static string GetRandomWord() {
        if (wordList == null) {
            wordList = JsonLoader.LoadDictionary();
        }
        int randomIndex = Random.Range(0, wordList.Length);
        return wordList[randomIndex];
    }
}
