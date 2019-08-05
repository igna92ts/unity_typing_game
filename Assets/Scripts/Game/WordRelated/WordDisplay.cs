using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordDisplay : MonoBehaviour {
    public Text text;
    float fallSpeed;
    Transform target;
    public void SetWord(string word, float fallSpeed, Transform target) {
        this.fallSpeed = fallSpeed;
        if (text == null) {
            text = GetComponent<Text>();
        }
        text.text = word;
        this.target = target;
    }

    public void RemoveLetter() {
        text.text = text.text.Remove(0, 1);
        text.color = Color.red;
    }
    public bool AllLettersRemoved() {
        return text.text.Length > 0 ? false : true;
    }

    public void RemoveWord() {
        Destroy(gameObject);
    }
    float moveTowardsMinDistance = 10f;
    void Update() {
        if (Vector2.Distance(transform.position, target.position) <= moveTowardsMinDistance) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * fallSpeed);
        } else {
            transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f);
        }
    }
}