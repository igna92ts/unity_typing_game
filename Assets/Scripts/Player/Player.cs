using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int lives = 5;
    public GameStateManager gameStateManager;
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
        lives = 5;
    }
}
