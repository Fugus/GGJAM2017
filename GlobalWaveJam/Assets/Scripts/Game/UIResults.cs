using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResults : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        foreach (PlayerStats p in GameLogic.Players)
        {
            string dbgtxt = "Chara " + p.Index;
            int newscore = p.Score;
            foreach (KeyValuePair<TrackingEvent, int> pair in p.Events)
            {
                newscore += GameSettings.ScoreMatrix[pair.Key] * pair.Value;
            }

            foreach (Text t in FindObjectsOfType<Text>())
            {
                if (t.name == "Results_" + p.Index)
                {
                    t.text = "" + newscore;
                    Debug.Log(t.name + " " + t.text);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
