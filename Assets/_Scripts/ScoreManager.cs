using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    //Singleton
    private static ScoreManager _instance;
    public static ScoreManager Instance

    {
        get
        {
            if (_instance == null)
                Debug.Log("Error instance GameManger is null.");

            return _instance;
        }
    }

    private Text _scoreTxt;
    private Text _tipsTxt;
    public int score = 3;
    public int tipsInt = 5;


    void Awake()
    {
        _instance = this;

        _scoreTxt = GameObject.Find("ScoreTxt").GetComponent<Text>();
        _tipsTxt = GameObject.Find("TipsText").GetComponent<Text>();


    }

    public void ScoreSub()
    {
        if (score > 1)
            score--;
        else
        {
            score = 0;
        }

        _scoreTxt.text = score.ToString();
    }

    public void TipSub()
    {
        if (tipsInt > 1)
            tipsInt--;
        else
        {
            tipsInt = 0;

        }

        _tipsTxt.text = tipsInt.ToString();
    }
    public void StartGame()
    {
        score = 3;
        _scoreTxt.text = score.ToString();

        tipsInt=5;
        _tipsTxt.text = tipsInt.ToString();

    }

}
