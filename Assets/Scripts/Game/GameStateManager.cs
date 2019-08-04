using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEvents {
    NOOP = 0,
    NEW_GAME = 1 
}
public class GameStateManager : MonoBehaviour {
    public StateMachine<GameStateManager> stateMachine;
    public UIManager uiManager;
    GameEvents gameEvent = GameEvents.NOOP;
    public GameEvents GameEvent { get; set; }
    //// GAME ELEMENTS
    public GameObject player;
    public GameObject wordManager;
    void Start() {
        uiManager = GetComponent<UIManager>();

        stateMachine = new StateMachine<GameStateManager>(this);
        stateMachine.ChangeState(MainMenuState.Instance);
    }
    // Update is called once per frame
    void Update() {
       stateMachine.Update();
    }
}
