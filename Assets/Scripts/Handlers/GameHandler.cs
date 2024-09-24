using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameManager oldGameManager;
    [SerializeField] private WordManager wordManager;

    //TODO: to refactor out of GameHandler
    private Word currentWord; //when is it set?
    private int currentLetterIndex; //when is it set?
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
        }
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
        DOTween.Init().SetCapacity(4000, 4000);

        levelManager.Initialize();
        wordManager.Initialize(levelManager);
    }

    private void OnEnable()
    {
        inputHandler.OnLetterInput += HandleLetterInput;
        levelManager.OnLevelChanged += HandleLevelChanged;
        levelManager.OnGameCompleted += HandleGameCompleted;
    }

    private void OnDisable()
    {
        inputHandler.OnLetterInput -= HandleLetterInput;
        levelManager.OnLevelChanged -= HandleLevelChanged;
        levelManager.OnGameCompleted -= HandleGameCompleted;
    }

    private void HandleLetterInput(char inputChar)
    {
        if (inputChar == currentWord.GetCharAtIndex(currentLetterIndex))
        {
            currentLetterIndex++;
            if (currentLetterIndex >= currentWord.Count())
            {
                levelManager.WordCompleted();
                SetNextWord();
            }
        }
        else
        {
            oldGameManager.GetScoreManager().IncrementFailures();
            oldGameManager.GetScoreManager().IncreaseScore(-1);
            oldGameManager.GetScoreManager().ResetKeystrokeStreak();
        }
    }

    private void HandleLevelChanged(int i)
    {
        Debug.Log($"Advancing to level: {i}");
        //TODO: implement logic here
    }

    private void HandleGameCompleted()
    {
        
    }

    private void SetNextWord()
    {
        currentWord = wordManager.GetAndSetNextWord();
        currentLetterIndex = 0;
        //update UI to display new word
    }

    public void StartGame()
    {
        oldGameManager.StartGame();
    }

    public void StartNewGame() 
    {
        oldGameManager.StartNewGame();
    }
}
