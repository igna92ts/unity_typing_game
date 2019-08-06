using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour {
    public GameObject wordPrefab;
    public Transform wordCanvas;
    float spawnSpan;
    float wordPoolAmount = 30;
    List<GameObject> wordDisplayPool;
    void Start() {
		Camera cam = Camera.main;
		float camHeight = 2f * cam.orthographicSize;
		float camWidth = camHeight * cam.aspect;
        spawnSpan = (camWidth * .6f) / 2; // 70% of the screen

        wordDisplayPool = new List<GameObject>();
        for (int i = 0; i < wordPoolAmount; i++) {
            GameObject wordObj = Instantiate(wordPrefab, new Vector2(Random.Range(-spawnSpan, spawnSpan), 20), Quaternion.identity, wordCanvas);
            wordObj.SetActive(false);
            wordDisplayPool.Add(wordObj);
        }
    }
    public WordDisplay SpawnWord() { 
        // GameObject wordObj = Instantiate(wordPrefab, new Vector2(Random.Range(-spawnSpan, spawnSpan), 20), Quaternion.identity, wordCanvas);
        // return wordObj.GetComponent<WordDisplay>();

        foreach(GameObject word in wordDisplayPool) {
            if (!word.activeInHierarchy) {
                word.transform.position = new Vector2(Random.Range(-spawnSpan, spawnSpan), 20);
                word.transform.rotation = Quaternion.identity;
                word.SetActive(true);
                return word.GetComponent<WordDisplay>();
            }
        }
        return null;
    }
}
