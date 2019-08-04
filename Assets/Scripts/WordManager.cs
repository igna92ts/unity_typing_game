using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager : MonoBehaviour {
    public List<Word> words;

    bool hasActiveWord;
    Word activeWord;
    public Shooting shooter;
    WordSpawner wordSpawner;
    void Start() {
        wordSpawner = GetComponent<WordSpawner>();
    }
    public void AddWord() {
        Word word = new Word(WordGenerator.GetRandomWord(), wordSpawner.SpawnWord());
        words.Add(word);
    }

    public void TypeLetter(char letter) {
        if (hasActiveWord) {
            if (activeWord.GetNextLetter() == letter) {
                activeWord.TypeLetter();
                shooter.Shoot(activeWord);
            }
        } else {
            foreach(Word word in words) {
                if (word.GetNextLetter() == letter) {
                    activeWord = word;
                    hasActiveWord = true;
                    word.TypeLetter();
                    shooter.Shoot(word);
                    break;
                }
            }
        }

        if (hasActiveWord && activeWord.WordTyped()) {
            hasActiveWord = false;
            words.Remove(activeWord);
        }
    }
}
