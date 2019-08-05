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

        owner.player.SetActive(true);
        #if !UNITY_IOS && !UNITY_ANDROID
            var playerPosition = owner.player.transform.position;
            owner.player.transform.position = new Vector2(0, -15);
        #endif

        owner.wordManager.gameObject.SetActive(true);
        owner.wordManager.Clear();
    }
    public override void ExitState(GameStateManager owner) {

    }
    public override void UpdateState(GameStateManager owner) {
        owner.score = owner.wordManager.clearedWords * 100;
        if (owner.GameEvent == GameEvents.PLAYER_LOST_LIFE) {
            ScreenShake.Shake(.2f);
            owner.wordManager.Clear();
        }
    }
}
