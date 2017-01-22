using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


#region delegates
public delegate void DelegateGameState(GameState state);
#endregion

public class GameLogic : Singleton<GameLogic>
{
    #region events
    public event DelegateGameState StateEvent;
    #endregion

    #region game state
    private GameState _state = GameState.NONE;

    public static GameState State
    {
        get
        {
            return Instance._state;
        }

        set
        {
            Instance._state = value;

            // callback
            if (Instance.StateEvent != null)
                Instance.StateEvent(State);

            // set state specific logic here
            switch (Instance._state)
            {
                case GameState.Menu:
                    break;

                case GameState.Intro:
                    SceneManager.LoadScene("Test_CB");
                    break;

                case GameState.Play:
                    break;

                default:
                    break;
            }
        }
    }
    #endregion

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
