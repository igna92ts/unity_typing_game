using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject keyBoard;
    public GameObject mainMenuObjects;
    GameStateManager gameStateManager;
    void Start() {
        gameStateManager = GetComponent<GameStateManager>();
    }

    public void NewGameButton() {
        gameStateManager.GameEvent = GameEvents.NEW_GAME;
    }
        
    public void Clear() {
        this.keyBoard.SetActive(false);
        this.mainMenuObjects.SetActive(false);
    }
    public void MainMenu() {
        Clear();
        this.mainMenuObjects.SetActive(true);
    }
    public void InGameUI() {
        Clear();
        this.keyBoard.SetActive(true);
    }
}
