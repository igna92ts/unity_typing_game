using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    int lives = 5;
    public GameStateManager gameStateManager;
    void OnCollisionEnter2D(Collision2D collision) {
        PlayerLostLife();
    }
    void PlayerLostLife() {
        lives--;
        gameStateManager.GameEvent = GameEvents.PLAYER_LOST_LIFE;
    }
}
