using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#region delegates
public delegate void DelegateGameState(GameState state);
#endregion

/// <summary>
/// Overall settings. Can be changed or loaded to make another game mode
/// </summary>
public static class GameSettings
{
    public static int MaxPlayers = 8;
    public static int MinPlayerToEnd = 1;
    public static Dictionary<TrackingEvent, int> ScoreMatrix = new Dictionary<TrackingEvent, int>()
    {
        { TrackingEvent.LastManStanding, 10 },
        };
}

/// <summary>
/// Game logic singleton
/// </summary>
public class GameLogic : Singleton<GameLogic>
{
    #region events
    public event DelegateGameState StateEvent;
    #endregion

    #region references
    private List<Player> _players = new List<Player>();
    public static List<Player> Players
    {
        get { return Instance._players; }
        set { Instance._players = value; }
    }
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
                    // remove players
                    Players.Clear();

                    // load
                    SceneManager.LoadScene("Test_CB");

                    break;

                case GameState.Play:
                    break;

                case GameState.Outro:
                    Debug.Log("End game !");
                    break;

                default:
                    break;
            }
        }
    }
    #endregion

    #region player stats mgt
    public void OnDeath()
    {
        Debug.Log(Players.FindAll(x => x.Stats.Alive).Count + " players remaining...");
        // check other players alive, set outro
        if (Players.FindAll(x => x.Stats.Alive).Count <= GameSettings.MinPlayerToEnd)
        {
            State = GameState.Outro;
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
