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
        owner.wordManager.SetActive(true);
    }
    public override void ExitState(GameStateManager owner) {

    }
    public override void UpdateState(GameStateManager owner) {
    }
}
