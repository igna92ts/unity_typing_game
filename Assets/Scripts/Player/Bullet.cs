using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Word targetWord;
    public float bulletSpeed = 2f;
    public void SetTarget(Word targetWord) {
        this.targetWord = targetWord;
        transform.up = targetWord.GetPosition().normalized;
    }
    void Update() {
        transform.position = Vector2.MoveTowards(transform.position, targetWord.GetPosition(), Time.deltaTime * bulletSpeed);
        if ((Vector2)transform.position == targetWord.GetPosition()) {
            targetWord.RemoveLetterDisplayed();
            Destroy(this.gameObject);
        }
    }
}
