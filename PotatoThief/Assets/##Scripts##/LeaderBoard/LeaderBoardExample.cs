using System.Collections;
using System.Collections.Generic;
using Login;
using UnityEngine;
using UnityEngine.UI;

public static class LeaderBoardData
{
    public const string StageTime = "CgkIxdiFztoEEAIQAw";
}
public class LeaderBoardExample : MonoBehaviour
{
    [SerializeField] private long _score = 1000;
    [SerializeField] private InputField _inputField;
    public void AddLeaderBoardData()
    {
        Social.ReportScore(_score, LeaderBoardData.StageTime,
            (x) =>
            {
                Debug.Log($"Leader Board Callback => ({x.ToString()})");
                Debug.Log($"Add Score : {_score.ToString()}");
            });
    }

    public void ShowLeaderBoardData()
    {
        Social.ShowLeaderboardUI();
    }

    public void SetLeaderBoardData()
    {
        Debug.Log($"input field text : {_inputField.text}");
        _score = long.Parse(_inputField.text);
    }
}
