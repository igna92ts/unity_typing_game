using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordDisplay : MonoBehaviour {
    public Text text;
    Stack<char> removedCharacters = new Stack<char>();
    float fallSpeed = 2;
    public void SetWord(string word) {
        if (text == null) {
            text = GetComponent<Text>();
        }
        text.text = word;
    }

    public void RemoveLetter() {
        removedCharacters.Push(text.text[0]);
        text.text = text.text.Remove(0, 1);
        text.color = Color.red;
    }
    public bool AllLettersRemoved() {
        return text.text.Length > 0 ? false : true;
    }

    public void RemoveWord() {
        Destroy(gameObject);
    }
    void Update() {
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f);
    }
}