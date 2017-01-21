using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour {

    // Use this for initialization
    void Start () {
        // define delegates
        GameLogic.Instance.StateEvent += OnGameState;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region UI calls
    public void StartGame()
    {
        // set 1st state
        GameLogic.State = GameState.Intro;
    }
    #endregion

    #region callbacks
    void OnDestroy()
    {
        // delegates
        if (GameLogic.Instance)
        {
            GameLogic.Instance.StateEvent += OnGameState;
        }
    }

    public void OnGameState(GameState state)
    {
        // set state specific UI logic here
        switch (state)
        {
            case GameState.Menu:
                break;

            case GameState.Intro:
                break;

            default:
                break;
        }
    }
    #endregion
}
