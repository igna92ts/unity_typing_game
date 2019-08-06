using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager : MonoBehaviour {
    public List<Word> words = new List<Word>();

    bool hasActiveWord;
    Word activeWord;
    public int clearedWords = 0;
    public int clearedLetters = 0;
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
    public void AddWord(WordTypes wordType = WordTypes.NORMAL) {
        Word word = new Word(WordGenerator.GetRandomWord(), wordSpawner.SpawnWord(), wordFallSpeed, player.transform, wordType);
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
        clearedLetters = 0;
    }

    public void PlayerHitClear() {
        activeWord = null;
        hasActiveWord = false;
        foreach(Word w in words) {
            w.SelfDestroy();
        }
        words.Clear();
    }

    public void TypeLetter(char letter) {
        if (hasActiveWord) {
            if (activeWord.GetNextLetter() == letter) {
                clearedLetters++;
                activeWord.TypeLetter();
                shooter.Shoot(activeWord);
            }
        } else {
            foreach(Word word in words) {
                if (word.GetNextLetter() == letter) {
                    clearedLetters++;
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
            if (activeWord.wordType == WordTypes.TIME_BOMB) {
                StartCoroutine(FreezeTimer());
            }
            clearedWords++;
            speedCounter++;
        }
        if (speedCounter >= increaseSpeedStep) {
            wordTimer.IncreaseSpeed();
            wordFallSpeed += .1f;
            speedCounter = 0;
        }
    }
    float freezeTime = 3f;
    IEnumerator FreezeTimer() {
        wordTimer.TurnOff();
        foreach(Word word in words) {
            word.TogglePause();
        }
        yield return new WaitForSeconds(freezeTime);
        foreach(Word word in words) {
            word.TogglePause();
        }
        wordTimer.TurnOn();
    }
}
