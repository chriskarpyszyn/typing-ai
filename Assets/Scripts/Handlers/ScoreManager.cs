using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    private void Start()
    {
        Score.Instance.SetElapsedTime(0f);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            IncreaseElapsedTime(Time.deltaTime);
        }
    }

    public void IncreaseElapsedTime(float t)
    {
        Score.Instance.IncreaseElapsedTime(t);

        //TODO-CK-17 Refactor this out.
        GameObject textTimerObject = GameObject.Find("TextTimer");
        if (textTimerObject != null)
        {
            TextMeshProUGUI textTimer = textTimerObject.GetComponent<TextMeshProUGUI>();
            textTimer.text = Score.Instance.GetElapsedTime().ToString("F1");
        }
    }
}
