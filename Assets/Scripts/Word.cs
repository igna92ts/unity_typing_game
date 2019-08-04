using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Word {
    public string word;
    private int typeIndex = 0;
    WordDisplay display;
    public Word(string word, WordDisplay display) {
        this.word = word;
        this.typeIndex = 0;

        this.display = display;
        this.display.SetWord(word);
    }

    public char GetNextLetter() {
        return word[typeIndex];
    }

    public void TypeLetter() {
        typeIndex++;
    }
    public void RemoveLetterDisplayed() {
        display.RemoveLetter();
        if (display.AllLettersRemoved()) {
            display.RemoveWord();
        } 
    }

    public Vector2 GetPosition() {
        return display.gameObject.transform.position;
    }

    public bool WordTyped() {
        var wordTyped = typeIndex >= word.Length;
        if (wordTyped) {
            return true;
        }
        return false;
    }
}
