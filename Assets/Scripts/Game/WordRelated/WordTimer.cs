using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTimer : MonoBehaviour {
    public WordManager wordManager;
    float wordDelay = 5f;
    private float nextWordTime = 0f;
    private bool shouldRun = false;

    void Start() {
        wordManager = GetComponent<WordManager>();
    }

    public void IncreaseSpeed() {
        wordDelay -= .05f;
    }

    public void Clear() {
        nextWordTime = 0f;
        wordDelay = 3f;
        shouldRun = false;
    }

    int bombPercentChance = 10;
    void Update() {
        if (shouldRun) {
            if (Time.time >= nextWordTime) {
                var wordType = Random.Range(0, 100) < bombPercentChance ? WordTypes.TIME_BOMB : WordTypes.NORMAL;
                wordManager.AddWord(wordType);
                nextWordTime = Time.time + wordDelay;
            }
        }
    }

    public void TurnOn() {
        shouldRun = true;
    }
    public void TurnOff() {
        shouldRun = false;
    }
}
