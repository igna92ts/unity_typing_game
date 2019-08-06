using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordDisplay : MonoBehaviour {
    public TMPro.TextMeshProUGUI text;
    float fallSpeed;
    Transform target;
    Color initialColor;
    bool shouldRun = true;
    Transform enemySpriteTransform;

    void OnEnable() {
        if (enemySpriteTransform == null)
            enemySpriteTransform = transform.Find("EnemySprite");
        enemySpriteTransform.up = Vector2.down;
    }
    public void SetWord(string word, float fallSpeed, Transform target, WordTypes wordType) {
        this.fallSpeed = fallSpeed;
        if (text == null) {
            text = GetComponent<TMPro.TextMeshProUGUI>();
        }
        text.text = word;
        initialColor = text.color;
        text.color = GetWordColor(wordType);
        this.target = target;
    }
    Color GetWordColor(WordTypes wordType) {
        switch(wordType) {
            case WordTypes.TIME_BOMB: return Color.green;
            default: return Color.white;
        }
    }

    public void RemoveLetter() {
        text.text = text.text.Remove(0, 1);
        text.color = Color.red;
    }
    public bool AllLettersRemoved() {
        return text.text.Length > 0 ? false : true;
    }

    public void RemoveWord() {
        text.color = initialColor;
        this.gameObject.SetActive(false);
    }
    float moveTowardsMinDistance = 15f;
    void Update() {
        if (shouldRun) {
            if (transform.position.y - target.position.y <= moveTowardsMinDistance) {
                transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * fallSpeed);

                var direction = (Vector2)(target.position - enemySpriteTransform.position).normalized;
                enemySpriteTransform.up = direction;
            } else {
                transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f);
            }
        }
    }
    public void TogglePause() {
        shouldRun = !shouldRun;
    }
}