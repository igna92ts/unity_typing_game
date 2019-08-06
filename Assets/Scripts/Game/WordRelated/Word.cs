using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WordTypes {
    NORMAL = 0,
    TIME_BOMB = 1
}
public class Word {
    public string word;
    private int typeIndex = 0;
    WordDisplay display;
    public WordTypes wordType;
    public Word(string word, WordDisplay display, float fallSpeed, Transform target, WordTypes wordType = WordTypes.NORMAL) {
        this.wordType = wordType;
        this.word = word;
        this.typeIndex = 0;

        this.display = display;
        this.display.SetWord(word, fallSpeed, target, wordType);
    }
    public void TogglePause() {
        display.TogglePause();
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

    public void SelfDestroy() {
        display.RemoveWord();
    }

    public bool WordTyped() {
        var wordTyped = typeIndex >= word.Length;
        if (wordTyped) {
            return true;
        }
        return false;
    }
}
