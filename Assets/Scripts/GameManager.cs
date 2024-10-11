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
    private const char NULL_CHAR = '\0';
    private List<GameObject> asteroids;

    #region Serialized Fields
    [Header("References")]
    //[SerializeField] private GameObject letterParent;
    [SerializeField] private GameObject letterPrefab;
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private GameObject level3GameCanvas;
    [SerializeField] public ScoreManager scoreManager;
   

    [Header("Game Configuration")]
    [SerializeField] private int numWordsPerRound = 5;    
    [SerializeField] private float textShrinkAnimationDuration = 0.2f;
    [SerializeField] private Color successColor = new Color(0.439f, 0.812f, 0.498f, 1f);
    [SerializeField] private float letterOffset = 2.5f;
    #endregion

    #region Public Properties
    public UnityEvent<string, string> submitScoreEvent;
    #endregion

    #region Private Properties
    private char[] wordCharArray;
    private int wordCharArraySize;
    private bool wordCompleted = false;
    private bool canType = true;

    //minigameOneLevel 1 is referencing a specific minigameOneLevel within the first minigame.
    private int minigameOneLevel = 1; //todo-ck i can prob find a better way to do this...
    
    
    private int numberOfWordsCompletedThisLevel = 0;
    private bool gameFinished = false;
    private List<string> wordList;
    private List<GameObject> currentWordList;
    private int currentLetterPosition = 0;
    public LetterSounds letterSounds;
    private int randomWordPosition;
    private string nextHardCodedWord;
    private ScaleTextAnimation scaleTextAnimation;
    private bool isFadeInComplete = false;
    #endregion

    private void Start()
    {
        scaleTextAnimation = new ScaleTextAnimation();
        scoreManager = ScoreManager.Instance;
 

        //asteroids = new List<GameObject>();
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            /*letterSounds = letterParent.GetComponent<LetterSounds>();*/ //todo-ck refactor sound off letter parent
            //having to do this kind of sucks to simply subscribe to an event
            //todo-ck should null check each line here
            SubscribeToFadeIn();
            
        } else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            //Debug.Log("OnStart");
            //SubscribeToFadeIn();
            //currentWordList = new List<GameObject>();
            //AssignWordList(threeLetterWords);
            //ChangeAsteroidWord();
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

    private void CheckLetter()
    {

        //3 words on screen
        //fox, kid, bot
        //start typing F - then I'm typing against Fox
        //what if I type B next.. is that a wrong input or does it work with the word Bot
        //then if I type O, do both Fox and Bot register a char

        //if i have two words Foo, Fox.
        //then typing F would register against both.
        //then typing o would register against both.

        //another thought, asteroids that require multiple words to destroy... 
        GameObject currentLetter = null; //delete me, not useful.
        //if (IsCharTyped() && (currentLetterPosition < wordCharArraySize))
        {
            //char inputChar = ExtractCharFromInput(Input.inputString.ToCharArray());
            //if (IsSuccessfullLetter(inputChar))
            //{
                //set color of word to success!!! 
                //not added to new code yet.
                //GameObject currentLetter = currentWordList[currentLetterPosition];
                //TypedCorrectLetter(currentLetter); //change color here
                //DecreaseLetterScale(currentLetter); //animate destruction

                currentLetterPosition++;
                if (currentLetterPosition < wordCharArraySize)
                {
                    GameObject nextLetter = currentWordList[currentLetterPosition];
                    //IncreaseLetterScale(nextLetter);
                }
            //}
            //else
            //{
            //    //TypedWrongLetter();
            //}

        }
    }

    private void TypedCorrectLetter(GameObject letter)
    {
        //letter.GetComponent<TextMeshPro>().color = successColor; - implemented in letterCanvas
        //letterSounds?.playPositiveSound();
        scoreManager.IncreaseScore(2);
        scoreManager.IncrementKeystrokeStreak();
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
