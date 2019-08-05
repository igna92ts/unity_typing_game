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
        wordDelay -= .01f;
    }

    public void Clear() {
        nextWordTime = 0f;
        wordDelay = 3f;
        shouldRun = false;
    }

    void Update() {
        if (shouldRun) {
            if (Time.time >= nextWordTime) {
                wordManager.AddWord();
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
