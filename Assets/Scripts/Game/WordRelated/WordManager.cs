using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager : MonoBehaviour {
    public List<Word> words;

    bool hasActiveWord;
    Word activeWord;
    public int clearedWords = 0;
    public Shooting shooter;
    WordSpawner wordSpawner;
    WordTimer wordTimer;
    int speedCounter = 0;
    int increaseSpeedStep = 1;
    float wordFallSpeed;
    public GameObject player;
    void Awake() {
        wordSpawner = GetComponent<WordSpawner>();
        wordTimer = GetComponent<WordTimer>();
    }
    public void AddWord() {
        Word word = new Word(WordGenerator.GetRandomWord(), wordSpawner.SpawnWord(), wordFallSpeed, player.transform);
        words.Add(word);
    }
    public void Clear() {
        activeWord = null;
        hasActiveWord = false;
        speedCounter = 0;
        wordFallSpeed = 1f;
        foreach(Word w in words) {
            w.SelfDestroy();
        }
        words.Clear();
        wordTimer.Clear();
        wordTimer.TurnOn();
        clearedWords = 0;
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
            clearedWords++;
            speedCounter++;
        }
        if (speedCounter >= increaseSpeedStep) {
            wordTimer.IncreaseSpeed();
            wordFallSpeed += .01f;
            speedCounter = 0;
        }
    }
}
