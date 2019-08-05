using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEvents {
    NOOP = 0,
    NEW_GAME = 1,
    PLAYER_LOST_LIFE = 2,
    PLAYER_DIED = 3 
}
public class GameStateManager : MonoBehaviour {
    public StateMachine<GameStateManager> stateMachine;
    public UIManager uiManager;
    GameEvents gameEvent = GameEvents.NOOP;
    public GameEvents GameEvent {
        get {
            var e = gameEvent;
            gameEvent = GameEvents.NOOP;
            return e;
        }
        set { gameEvent = value; }
    }
    //// GAME ELEMENTS
    public Player player;
    public WordManager wordManager;
    public int score = 0;
    void Start() {
        uiManager = GetComponent<UIManager>();

        stateMachine = new StateMachine<GameStateManager>(this);
        stateMachine.ChangeState(MainMenuState.Instance);
    }
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ScreenShake.Shake(.1f);
        }
        stateMachine.Update();
    }
}
