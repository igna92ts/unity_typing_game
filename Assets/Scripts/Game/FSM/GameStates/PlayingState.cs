using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : State<GameStateManager> {
    private static PlayingState _instance;
    private PlayingState() {
        if (_instance != null) {
            return;
        }
        _instance = this;
    }
    public static PlayingState Instance {
        get {
            if (_instance == null) {
                new PlayingState();
            }
            return _instance;
        }
    }
    public override void EnterState(GameStateManager owner) {
        owner.uiManager.InGameUI();

        owner.player.gameObject.SetActive(true);
        owner.player.Clear();

        owner.wordManager.gameObject.SetActive(true);
        owner.wordManager.Clear();
    }
    public override void ExitState(GameStateManager owner) {
        owner.player.Clear();
        owner.player.gameObject.SetActive(false);
    }
    int wordScoreValue = 100;
    int letterScoreValue = 1;
    public override void UpdateState(GameStateManager owner) {
        var gameEvent = owner.GameEvent;

        owner.score = owner.wordManager.clearedWords * wordScoreValue + owner.wordManager.clearedLetters * letterScoreValue;
        owner.uiManager.ShowLifeCount(owner.player.lives);
        if (gameEvent == GameEvents.PLAYER_LOST_LIFE) {
            ScreenShake.Shake(.2f);
            owner.wordManager.PlayerHitClear();
        }
        if (gameEvent == GameEvents.PLAYER_DIED) {
            owner.stateMachine.ChangeState(ScoreUploadState.Instance);
        }
    }
}
