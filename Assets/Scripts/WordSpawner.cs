using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour {
    public GameObject wordPrefab;
    public Transform wordCanvas;
    public WordDisplay SpawnWord() { 
        GameObject wordObj = Instantiate(wordPrefab, new Vector2(Random.Range(-5, 5), 20), Quaternion.identity, wordCanvas);
        return wordObj.GetComponent<WordDisplay>();
    }
}
