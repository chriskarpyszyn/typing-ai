using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

    #region Serialized Fields
    [Header("References")]
    //[SerializeField] private GameObject letterParent;
    [SerializeField] private GameObject level3GameCanvas;
    [SerializeField] public ScoreManager scoreManager;
   
    #endregion

    #region Public Properties
    public UnityEvent<string, string> submitScoreEvent;
    #endregion

    #region Private Properties
    private bool gameFinished = false;
    private bool isFadeInComplete = false;
    #endregion

    private void Start()
    {
        scoreManager = ScoreManager.Instance;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SubscribeToFadeIn();
        } 
    }
   

    /// <summary>
    /// Find the FadeIn canvas and subscribe to the animation complete event
    /// TODO: We should move this code elsewhere
    /// </summary>
    private void SubscribeToFadeIn()
    {
        GameObject fadeInCanvas = GameObject.Find("FadeInCanvas");
        Transform fadeInImage = fadeInCanvas.transform.Find("FadeInImage");
        FadeIn fadeInScript = fadeInImage.GetComponent<FadeIn>();
        fadeInScript.EventOnFadeInComplete += HandleFadeInComplete;
    }
    /// <summary>
    /// Handle the event by flipping a local bool.
    /// </summary>
    private void HandleFadeInComplete()
    {
        isFadeInComplete = true;
    }

    private void Update()
    {
        //is there a way to avoid this check in the update method?
        if (isFadeInComplete)
        {
            //CheckAllLettersCompleted();
            //CheckLetter();

            //TODO: move to score manager, need to get the timer out of here
            CheckAndIncreaseTime();
        }
    }

    public void CheckAndIncreaseTime()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && !gameFinished)
        {
            ScoreManager.Instance.IncreaseElapsedTime(Time.deltaTime);
        }
    }

    //StartGame is called when clicking the Start Game button on the title screen.
    public void StartGame()
    {
        new LevelLoader().LoadLevel(1);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayBackgroundNoise();
    }
    //StartNewGame is called when clicking the Start New button on the end screen.
    public void StartNewGame()
    {
        scoreManager.ResetStats();
        gameFinished = false;
        SceneManager.LoadScene(1);
    }


    public ScoreManager GetScoreManager()
    {
        return ScoreManager.Instance;
    }



    private void ChangeAsteroidWord()
    {
        //Debug.Log("ChangeAsteroidWord");
        //GameObject newAsteroid = Instantiate(asteroidPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        //newAsteroid.transform.SetParent(level3GameCanvas.transform);
        //asteroids.Add(newAsteroid);
        ////position it randomly x 10, y 7
        //float randomX = Random.Range(-10f, 10f);
        //float randomY = Random.Range(-7f, 7f);
        //newAsteroid.transform.position = new Vector3(randomX, randomY, -1f);

        //Transform asteroidLetterParentTransform = newAsteroid.transform.Find("LetterParent");
        //GameObject asteroidLetterParent = asteroidLetterParentTransform.gameObject;

        //////add word to asteroid
        ////put the characters into an array so that we can do our input checks (repeated code)
        //wordCharArray = GetAndRemoveNextWord().ToLower().ToCharArray();
        //wordCharArraySize = wordCharArray.Length;
        //CreateGameObjectWordList(wordCharArray, asteroidLetterParent);
    }
 
    public void EndGame()
    {
        //SceneManager.LoadScene(2); //todo-ck i hate having hard coded constants, breaks if I add  another scene
        new LevelLoader().LoadLevel(3);
        //DestroyGameObjectWordList();
        gameFinished = true;
        scoreManager.DisplayEndGameStats();
    }

    public void SubmitScore()
    {
        //TODO-CK-17 Refactor this out.
        GameObject inputHighScoreNameObject = GameObject.Find("LeaderboardInputField");
        if (inputHighScoreNameObject != null)
        {
            TMP_InputField inputHighScoreName = inputHighScoreNameObject.GetComponent<TMP_InputField>();
            submitScoreEvent.Invoke(inputHighScoreName.text, scoreManager.GetElapsedTimeString()); //testing the string value of score
        }
    }
}
