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
    }
    public override void ExitState(GameStateManager owner) {

    }
    public override void UpdateState(GameStateManager owner) {
    }
}
