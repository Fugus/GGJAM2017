using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGame : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // define delegates
        GameLogic.Instance.StateEvent += OnGameState;
        GameLogic.State = GameState.Intro;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region callbacks
    void OnDestroy()
    {
        // delegates
        if (GameLogic.Instance)
        {
            GameLogic.Instance.StateEvent -= OnGameState;
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

            case GameState.Outro:
                GetComponent<CanvasGroup>().alpha = 1;
                break;

            default:
                break;
        }
    }
    #endregion
}
