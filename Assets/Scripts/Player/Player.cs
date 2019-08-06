using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int lives = 5;
    public int maxLives = 5;
    Vector2 startPosition;
    public GameObject keyBoard;
    public GameStateManager gameStateManager;
    void Start() {
        #if !UNITY_IOS && !UNITY_ANDROID
            var screenBottomCenter = new Vector3(Screen.width / 2, Screen.height, 0);
            var inWorld = Camera.main.ScreenToWorldPoint(screenBottomCenter);
            startPosition = new Vector2(0, -inWorld.y + 8);
        #else
            startPosition = new Vector2(0, keyBoard.transform.position.y + 8f);
        #endif
        transform.position = startPosition;
    }
    void OnCollisionEnter2D(Collision2D collision) {
        PlayerLostLife();
    }
    void PlayerLostLife() {
        lives--;
        if (lives > 0) {
            gameStateManager.GameEvent = GameEvents.PLAYER_LOST_LIFE;
        } else {
            gameStateManager.GameEvent = GameEvents.PLAYER_DIED;
        }
    }
    public void Clear() {
        transform.position = startPosition;
        lives = maxLives;
    }
}
