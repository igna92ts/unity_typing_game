using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGenerator : MonoBehaviour {
    private static string[] wordList = {
        "hello",
        "what",
        "name",
        "nationality"
    };
    public static string GetRandomWord() {
        int randomIndex = Random.Range(0, wordList.Length);
        return wordList[randomIndex];
    }
}
