using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardState : State<GameStateManager> {
    private static ScoreBoardState _instance;
    private ScoreBoardState() {
        if (_instance != null) {
            return;
        }
        _instance = this;
    }
    public static ScoreBoardState Instance {
        get {
            if (_instance == null) {
                new ScoreBoardState();
            }
            return _instance;
        }
    }
    public override void EnterState(GameStateManager owner) {
        owner.uiManager.ScoreBoard();
        owner.highscoreBoard.DrawScoreTable();
    }
    public override void ExitState(GameStateManager owner) {
    }
    public override void UpdateState(GameStateManager owner) {
        var gameEvent = owner.GameEvent;
        if (gameEvent == GameEvents.BACK_TO_TITLE) {
            owner.stateMachine.ChangeState(MainMenuState.Instance);
        }
    }
}
