using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUploadState : State<GameStateManager> {
    private static ScoreUploadState _instance;
    private ScoreUploadState() {
        if (_instance != null) {
            return;
        }
        _instance = this;
    }
    public static ScoreUploadState Instance {
        get {
            if (_instance == null) {
                new ScoreUploadState();
            }
            return _instance;
        }
    }
    public override void EnterState(GameStateManager owner) {
        owner.uiManager.ScoreUpload();

        owner.uiManager.ShowScoreResults(owner.score, owner.wordManager.clearedWords);

        owner.wordManager.Clear();
        owner.wordManager.gameObject.SetActive(false);
    }
    public override void ExitState(GameStateManager owner) {

    }
    public override void UpdateState(GameStateManager owner) {
        var gameEvent = owner.GameEvent;
        if (gameEvent == GameEvents.BACK_TO_TITLE) {
            owner.stateMachine.ChangeState(MainMenuState.Instance);
        }
        if (gameEvent == GameEvents.SKIPPED_UPLOAD) {
            owner.stateMachine.ChangeState(ScoreBoardState.Instance);
        }
        if (gameEvent == GameEvents.UPLOADED_SCORE) {
            var name = owner.uiManager.nameInput.GetComponent<TMPro.TextMeshProUGUI>().text;
            owner.highscoreBoard.SaveHighscore(owner.score, name);
            owner.stateMachine.ChangeState(ScoreBoardState.Instance);
        }
    }
}
