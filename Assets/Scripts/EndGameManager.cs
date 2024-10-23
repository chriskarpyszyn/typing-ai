using System;
using TMPro;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{

    public event Action<string, string> OnSubmitScore;

    public void SubmitScore()
    {
        Debug.Log("Enter: Submit Score");
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        Debug.Log($"ScoreManager: {scoreManager}");
        GameObject inputHighScoreNameObject = GameObject.Find("LeaderboardInputField");
        if (inputHighScoreNameObject != null)
        {
            Debug.Log($"On Submit Score: {OnSubmitScore}");

            TMP_InputField inputHighScoreName = inputHighScoreNameObject.GetComponent<TMP_InputField>();
            OnSubmitScore?.Invoke(inputHighScoreName.text, Score.Instance.GetElapsedTimeString());
        }
    }

    public void StartNewGame()
    {
        FindObjectOfType<GameHandler>().StartNewGame();
    }
}
