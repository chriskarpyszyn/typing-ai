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
        ResetProperties();
        numberOfWordsCompletedThisLevel = 0;
        gameFinished = false;
        SceneManager.LoadScene(1);
        minigameOneLevel = 1; //todo-ck need a minigameOneLevel manager.
        //AssignWordList(threeLetterWords); //todo-ck refactor repeated code, youll know.
        ChangeWord();
    }


    public ScoreManager GetScoreManager()
    {
        return ScoreManager.Instance;
    }

    private void ResetProperties()
    {
        currentLetterPosition = 0;
        wordCompleted = false;
    }





    /// <summary>
    /// Method for handling the asteroid sprite and word generation.
    /// </summary>
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


    /// <summary>
    /// Change Word method for handling all of the Scene 1 logic for changing each word
    /// </summary>
    private void ChangeWord()
    {
        if (numberOfWordsCompletedThisLevel >= numWordsPerRound) //now in level manager
        {
            if (minigameOneLevel == 1) 
            {
                //minigameOneLevel++;
                //numberOfWordsCompletedThisLevel = 0;
                //AssignWordList(fourLetterWords);
                //ChangeWord(); 
                //randomWordPosition = Random.Range(1, numWordsPerRound); //randomPos for hardcoded word
                //nextHardCodedWord = specialWords.words[minigameOneLevel-1];

            } else if (minigameOneLevel == 2)
            {
                //minigameOneLevel++;
                //numberOfWordsCompletedThisLevel = 0;
                //AssignWordList(fiveLetterWords);
                //ChangeWord(); 
                //randomWordPosition = Random.Range(1, numWordsPerRound);
                //nextHardCodedWord = specialWords.words[minigameOneLevel-1];

            } else if (minigameOneLevel >= 3) //fyi greater or equal, dont forget
            {
                //EndGame();
            }
        } else
        {
            ////string nextWord = "";
            //if (numberOfWordsCompletedThisLevel == randomWordPosition)
            //{
            //    //insert one of the words not from the list
            //    nextWord = nextHardCodedWord;
            //} else
            //{
            //    //get the next word and remove it.
            //    nextWord = GetAndRemoveNextWord();
            //}

            //keep track of the number of words completed in this minigameOneLevel
            //todo-ck move to a minigameOneLevel manager script
            //numberOfWordsCompletedThisLevel++;

            //put the characters into an array so that we can do our input checks
            //wordCharArray = nextWord.ToLower().ToCharArray();
            //wordCharArraySize = wordCharArray.Length;

            //destroy old list and draw the next word on the scene
            //DestroyGameObjectWordList();
            CreateGameObjectWordList(wordCharArray);

            ResetProperties();

            //IncreaseLetterScale(currentWordList[0]);
        }
    }


    //Create a list of game objects that spell a word, and draw them to screen.
    private void CreateGameObjectWordList(char[] wordCharArray)
    {
        //CreateGameObjectWordList(wordCharArray, this.letterParent); //not moved over yet
    }

    private void CreateGameObjectWordList(char[] wordCharArray, GameObject letterParent)
    {
        currentLetterPosition = 0;
        float animationDuration = 0.4f;
        float overlapDelay = 0.04f;
        float cumulativeDelay = overlapDelay;

        float firstLetterPositionX = -6;
        foreach (char c in wordCharArray)
        {
            GameObject newLetter = Instantiate(letterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TextMeshPro tmpLetter = newLetter.GetComponent<TextMeshPro>();
            tmpLetter.color = new Color(tmpLetter.color.r, tmpLetter.color.g, tmpLetter.color.b, 0);
            newLetter.transform.SetParent(letterParent.transform, false);
            newLetter.name = "offset" + firstLetterPositionX;
            tmpLetter.text = c.ToString().ToUpper();
            //AddNewLetter(newLetter);


            //todo: letter fade in
            if (currentWordList.Count == 1)
            {
                scaleTextAnimation.FadeTMPAnimation(tmpLetter, 1, animationDuration);
            }
            else
            {
                scaleTextAnimation.FadeTMPAnimation(tmpLetter, 1, animationDuration).SetDelay(cumulativeDelay);
                cumulativeDelay = cumulativeDelay + overlapDelay;
            }
        }
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
