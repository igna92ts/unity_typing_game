using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTimer : MonoBehaviour {
    public WordManager wordManager;
    public float wordDelay = 2f;
    private float nextWordTime = 0f;

    void Start() {
        wordManager = GetComponent<WordManager>();
    }

    void Update() {
        if (Time.time >= nextWordTime) {
            wordManager.AddWord();
            nextWordTime = Time.time + wordDelay;
        }
    }
}
