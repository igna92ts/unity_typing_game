using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : State<GameStateManager> {
    private static MainMenuState _instance;
    private MainMenuState() {
        if (_instance != null) {
            return;
        }
        _instance = this;
    }
    public static MainMenuState Instance {
        get {
            if (_instance == null) {
                new MainMenuState();
            }
            return _instance;
        }
    }
    public override void EnterState(GameStateManager owner) {
        owner.uiManager.MainMenu();    
    }
    public override void ExitState(GameStateManager owner) {

    }
    public override void UpdateState(GameStateManager owner) {
        if (owner.GameEvent == GameEvents.NEW_GAME) {
           owner.stateMachine.ChangeState(PlayingState.Instance);
        }
    }
}
