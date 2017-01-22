using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResults : UIMain
{
    public static int pointsPerIcon = 10;
    public static int maxScore = 50;

    // Use this for initialization
    void Start()
    {
        foreach (PlayerStats p in GameLogic.Players)
        {
            string dbgtxt = "Chara " + p.Index + "(original score: " + p.Score + ")";
            int newscore = p.Score;
            foreach (KeyValuePair<TrackingEvent, int> pair in p.Events)
            {
                dbgtxt += "\n" + pair.Key.ToString() + ": " + pair.Value;
                newscore += GameSettings.ScoreMatrix[pair.Key] * pair.Value;
            }

            foreach (Text t in FindObjectsOfType<Text>())
            {
                if (t.name == "Results_" + p.Index)
                {
                    t.text = "" + newscore;
                    Debug.Log(dbgtxt + "\n" + t.name + " " + t.text);

                    for (int i = pointsPerIcon; i < maxScore; i += pointsPerIcon)
                    {
                        GameObject obj = t.transform.FindChild("Score" + i / pointsPerIcon).gameObject;
                        obj.GetComponent<CanvasGroup>().alpha = (newscore >= i ? 1 : 0);

                        if (p.Score < i && newscore >= i)
                        {
                            obj.GetComponent<Animation>().Play();
                        }
                    }
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
