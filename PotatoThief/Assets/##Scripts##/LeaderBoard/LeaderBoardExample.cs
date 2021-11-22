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
    
    public static void AddLeaderBoardData(long score)
    {
        // 리더보드 값 추가
        Social.ReportScore(score, LeaderBoardData.StageTime,
            (x) =>
            {
                Debug.Log($"Leader Board Callback => ({x.ToString()})");
                Debug.Log($"Add Score : {score.ToString()}");
                ShowLeaderBoardData();
            });
    }

    public static void ShowLeaderBoardData()
    {
        // 리더보드 보여주기
        Social.ShowLeaderboardUI();
    }

    // 쓸모없음
    public void SetLeaderBoardData()
    {
        Debug.Log($"input field text : {_inputField.text}");
        _score = long.Parse(_inputField.text);
    }
}
