using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject keyBoard;
    public GameObject lifeCounter;
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
        this.lifeCounter.SetActive(false);
    }
    public void MainMenu() {
        Clear();
        this.mainMenuObjects.SetActive(true);
    }
    public void InGameUI() {
        Clear();
        #if UNITY_IOS || UNITY_ANDROID
            this.keyBoard.SetActive(true);
        #endif
        lifeCounter.SetActive(true);
    }
    public void ShowLifeCount(int lifeCount) {
        lifeCounter.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = lifeCount.ToString();
    }
}
