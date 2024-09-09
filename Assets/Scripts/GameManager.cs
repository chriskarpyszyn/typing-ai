using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using UnityEditor;
using UnityEngine.XR;
using Unity.VisualScripting.Antlr3.Runtime;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private const string WORD_LIST_3CHAR = "word-list-3char";
    private const string WORD_LIST_4CHAR = "word-list-4char";
    private const string WORD_LIST_5CHAR = "word-list-5char";
    private const string WORD_1 = "God";
    private const string WORD_2 = "Help";
    private const string WORD_3 = "Truth";
    private const char NULL_CHAR = '\0';

    #region Serialized Fields
    [Header("References")]
    [SerializeField] private GameObject letterParent;
    [SerializeField] private GameObject letterPrefab;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TextMeshProUGUI wrongCharXTMP;

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
    private List<string> wordList;
    private int level = 1; //todo-ck i can prob find a better way to do this...
    private int numberOfWordsCompletedThisLevel = 0;
    private bool gameFinished = false;
    private List<GameObject> letterList;
    private int currentLetterPosition = 0;
    private LetterSounds letterSounds;
    private int randomWordPosition;
    private string nextHardCodedWord;
    private ScaleTextAnimation scaleTextAnimation;
    #endregion

    private void Start()
    {
        Application.targetFrameRate = 60;
        DOTween.Init().SetCapacity(4000,4000);
        scaleTextAnimation = new ScaleTextAnimation();
        scoreManager = ScoreManager.Instance;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            letterList = new List<GameObject>();
            AssignWordList(WORD_LIST_3CHAR);
            nextHardCodedWord = WORD_1;
            randomWordPosition = Random.Range(1, numWordsPerRound); //todo-ck SPAGHAT
            ChangeWord();
            letterSounds = letterParent.GetComponent<LetterSounds>();
        }
    }

    private void Update()
    {
        CheckExitGameShortcut();
        CheckAllLettersCompleted();
        CheckLetter();
        CheckAndIncreaseTime();
    }

    private void CheckExitGameShortcut()
    {
        //sneaky exit
        if (Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    private void CheckAllLettersCompleted()
    {
        //check if we're successfully compelted all letters!
        if (SceneManager.GetActiveScene().buildIndex == 1 && currentLetterPosition == wordCharArraySize && !wordCompleted)
        {
            scoreManager.IncreaseScore(3);
            wordCompleted = true;
            //letterSounds.playPositiveLongSound(); //todo-ck replace? redo?
            letterSounds.ResetPositiveSoundPitch();
            ChangeWordWithAnimation();
        }
    }

    private bool IsCharTyped()
    {
        return Input.anyKeyDown && !IsMouseButtonClick() && canType && !gameFinished;
    }

    private bool IsSuccessfullLetter(char inputChar)
    {
        return inputChar != NULL_CHAR && inputChar == wordCharArray[currentLetterPosition];
    }

    private char ExtractCharFromInput(char[] inputCharArray)
    {
        if (inputCharArray.Length>0)
        {
            return inputCharArray[0];
        }
        return NULL_CHAR;
    }
    private void CheckLetter()
    {
        if (IsCharTyped() && (currentLetterPosition < wordCharArraySize))
        {
            char inputChar = ExtractCharFromInput(Input.inputString.ToCharArray());
            if (IsSuccessfullLetter(inputChar))
            {
                //set color of word to success!!!
                GameObject currentLetter = letterList[currentLetterPosition];
                TypedCorrectLetter(currentLetter);
                DecreaseLetterScale(currentLetter);

                currentLetterPosition++;
                if (currentLetterPosition < wordCharArraySize)
                {
                    GameObject nextLetter = letterList[currentLetterPosition];
                    IncreaseLetterScale(nextLetter);
                }

            }
            else
            {
                TypedWrongLetter();
            }

        }
    }

    private void TypedCorrectLetter(GameObject letter)
    {
        letter.GetComponent<TextMeshPro>().color = successColor;
        letterSounds.playPositiveSound();
        scoreManager.IncreaseScore(2);
        scoreManager.IncrementKeystrokeStreak();
    }

    private void CheckAndIncreaseTime()
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
        level = 1; //todo-ck need a level manager.
        AssignWordList("word-list-3char"); //todo-ck refactor repeated code, youll know.
        ChangeWord();
    }

    public ScoreManager GetScoreManager()
    {
        return ScoreManager.Instance;
    }

    private void TypedWrongLetter()
    {
        scoreManager.IncrementFailures();
        scoreManager.IncreaseScore(-1);
        scoreManager.ResetKeystrokeStreak();
        letterSounds.playErrorSound();
        StartCoroutine(ShowTextTemporarily());
    }

    private void ResetProperties()
    {
        currentLetterPosition = 0;
        wordCompleted = false;
    }

    private void LetterScaleAnimation(float toSize, GameObject letter)
    {
        scaleTextAnimation.ScaleAnimation(
            letter,
            new Vector3(toSize, toSize, toSize),
            0.1f);
    }

    private void IncreaseLetterScale(GameObject letter)
    {
        LetterScaleAnimation(1.2f, letter);
    }

    private void DecreaseLetterScale(GameObject letter)
    {
        LetterScaleAnimation(1f, letter);
    }

    private void ChangeWordWithAnimation()
    {
        scaleTextAnimation.ScaleAnimation(letterParent,
            Vector3.zero,
            textShrinkAnimationDuration).OnComplete(ChangeWord);
    }

    private IEnumerator ShowTextTemporarily()
    {
        canType = false;
        wrongCharXTMP.enabled = true;
        yield return new WaitForSeconds(0.25f);
        wrongCharXTMP.enabled = false;
        canType = true;
    }

    private bool IsMouseButtonClick()
    {
        return (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2));
    }

    private void ChangeWord()
    {
        letterParent.transform.localScale = Vector3.one;
        if (numberOfWordsCompletedThisLevel >= numWordsPerRound)
        {
            if (level == 1) //todo-ck we need to refactor this out.
            {
                level++;
                numberOfWordsCompletedThisLevel = 0;
                AssignWordList(WORD_LIST_4CHAR);
                ChangeWord(); //todo-ck this is also not great, need to refactor out
                randomWordPosition = Random.Range(1, numWordsPerRound);
                nextHardCodedWord = WORD_2;

            } else if (level == 2)
            {
                level++;
                numberOfWordsCompletedThisLevel = 0;
                AssignWordList(WORD_LIST_5CHAR);
                ChangeWord(); //todo-ck this needs to be refactored out
                randomWordPosition = Random.Range(1, numWordsPerRound);
                nextHardCodedWord = WORD_3;

            } else if (level >= 3) //fyi greater or equal, dont forget
            {
                EndGame();
            }
        } else
        {
            string nextWord = "";
            if (numberOfWordsCompletedThisLevel == randomWordPosition)
            {
                //insert one of the words not from the list
                nextWord = nextHardCodedWord;
            } else
            {
                //get the next word and remove it.
                int randomInt = new System.Random().Next(0, wordList.Count);
                nextWord = wordList[randomInt];
                wordList.RemoveAt(randomInt);
            }

            //keep track of the number of words completed in this level
            //todo-ck move to a level manager script
            numberOfWordsCompletedThisLevel++;

            //put the characters into an array so that we can do our input checks
            wordCharArray = nextWord.ToLower().ToCharArray();
            wordCharArraySize = wordCharArray.Length;

            //destroy old list and draw the next word on the scene
            DestroyGameObjectWordList();
            CreateGameObjectWordList(wordCharArray);

            ResetProperties();

            IncreaseLetterScale(letterList[0]);
        }
    }

    //Create a list of game objects that spell a word, and draw them to screen.
    private void CreateGameObjectWordList(char[] wordCharArray)
    {
        currentLetterPosition = 0;

        float firstLetterPositionX = -6;
        foreach (char c in wordCharArray)
        {
            GameObject newLetter = Instantiate(letterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newLetter.transform.SetParent(letterParent.transform, false);
            newLetter.name = "offset" + firstLetterPositionX;
            newLetter.GetComponent<TextMeshPro>().text = c.ToString().ToUpper();
            letterList.Add(newLetter);
        }
    }

    //Destroys all the game objects in the word list.
    private void DestroyGameObjectWordList()
    {
        foreach (GameObject go in letterList)
        {
            Destroy(go);
        }

        letterList.Clear();

    }

    private void AssignWordList(string fileName)
    {
        wordList = new List<string>();
        currentLetterPosition = 0; //todo-ck spaghetti
        foreach (string line in LoadLinesFromFile(fileName))
        {
            wordList.Add(line.Trim().ToUpper());
        }
    }

    private string[] LoadLinesFromFile(string fileName)
    {
        TextAsset textWordList = Resources.Load<TextAsset>(fileName);
        if (textWordList != null)
        {
            string[] lines = textWordList.text.Split('\n');
            return lines;
        }
        return null;
    }

    private void EndGame()
    {
        //SceneManager.LoadScene(2); //todo-ck i hate having hard coded constants, breaks if I add  another scene
        new LevelLoader().LoadLevel(2);
        DestroyGameObjectWordList();
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
