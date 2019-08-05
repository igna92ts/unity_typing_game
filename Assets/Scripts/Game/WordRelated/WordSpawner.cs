using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour {
    public GameObject wordPrefab;
    public Transform wordCanvas;
    float spawnSpan;
    void Start() {
		Camera cam = Camera.main;
		float camHeight = 2f * cam.orthographicSize;
		float camWidth = camHeight * cam.aspect;
        spawnSpan = (camWidth * .7f) / 2; // 70% of the screen
    }
    public WordDisplay SpawnWord() { 
        GameObject wordObj = Instantiate(wordPrefab, new Vector2(Random.Range(-spawnSpan, spawnSpan), 20), Quaternion.identity, wordCanvas);
        return wordObj.GetComponent<WordDisplay>();
    }
}
