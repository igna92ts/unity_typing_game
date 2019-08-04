using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordInput : MonoBehaviour {
    WordManager wordManager;
    string inputString = "";
    public string CustomInputString { get { return inputString; } set { inputString = value; } }


    void Start() {
        wordManager = GetComponent<WordManager>();
    }
    void Update() {
        foreach(char letter in Input.inputString) {
            wordManager.TypeLetter(letter);
        } 
        if (inputString.Length > 0) {
            foreach(char letter in inputString) {
                wordManager.TypeLetter(letter);
            }
            inputString = "";
        }
    }
}
