using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject keyBoard;
    public GameObject lifeCounter;
    public GameObject mainMenuObjects;
    public GameObject backToTitle;
    // SCORE ELEMENTS
    public GameObject scoreElements;
    public GameObject scoreResults;
    public GameObject nameInput;
    public GameObject highscoreBoard;
    public GameObject scoreHeaders;
    GameStateManager gameStateManager;
    void Start() {
        gameStateManager = GetComponent<GameStateManager>();
    }

    public void NewGameButton() {
        gameStateManager.GameEvent = GameEvents.NEW_GAME;
    }
    public void SaveScoreButton() {
        gameStateManager.GameEvent = GameEvents.UPLOADED_SCORE;
    }
    public void SkipScoreUploadButton() {
        gameStateManager.GameEvent = GameEvents.SKIPPED_UPLOAD;
    }
    public void HighscoreButton() {
        gameStateManager.GameEvent = GameEvents.HIGH_SCORES;
    }
        
    public void Clear() {
        this.keyBoard.SetActive(false);
        this.mainMenuObjects.SetActive(false);
        this.lifeCounter.SetActive(false);
        this.backToTitle.SetActive(false);

        // SCORE ELEMENTS
        this.scoreResults.SetActive(false);
        this.scoreElements.SetActive(false);
        this.scoreHeaders.SetActive(false);
        this.highscoreBoard.SetActive(false);
        this.nameInput.GetComponent<TMPro.TextMeshProUGUI>().text = "";
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
    public void ScoreUpload() {
        Clear();
        scoreElements.SetActive(true);
        scoreResults.SetActive(true);
        backToTitle.SetActive(true);
    }
    public void ScoreBoard() {
        Clear();
        scoreElements.SetActive(true);
        scoreHeaders.SetActive(true);
        highscoreBoard.SetActive(true);
        backToTitle.SetActive(true);
    }
    public void ShowLifeCount(int lifeCount) {
        lifeCounter.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = lifeCount.ToString();
    }
    public void ShowScoreResults(int score, int wordsCleared) {
        var scoreValueText = scoreResults.transform.Find("ScoreValue").GetComponent<TMPro.TextMeshProUGUI>();
        var wordsClearedText = scoreResults.transform.Find("WordsClearedValue").GetComponent<TMPro.TextMeshProUGUI>();
        scoreValueText.text = score.ToString();
        wordsClearedText.text = wordsCleared.ToString();
    }
    public void BackToTitleButton() {
        gameStateManager.GameEvent = GameEvents.BACK_TO_TITLE;
    }
}
