using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] FadeIn fadeIn;
    private bool increaseTime = false;

    private void Start()
    {
        Score.Instance.SetElapsedTime(0f);
        fadeIn.EventOnFadeInComplete += HandleFadeInComplete;
    }

    private void OnDisable()
    {
        fadeIn.EventOnFadeInComplete -= HandleFadeInComplete;
    }

    private void HandleFadeInComplete()
    {
        increaseTime = true;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && increaseTime)
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
