using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameManager oldGameManager;
    [SerializeField] private WordManager wordManager;

    private bool gameFinished = false;

    
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
        SceneManager.sceneLoaded += HandleOnSceneLoaded;
    }

    private void InitializeManagers()
    {
        levelManager = FindObjectOfType<LevelManager>();
        levelManager?.Initialize();
        if (levelManager == null) { Debug.Log("Cannot find LevelManager in scene"); }
        wordManager = FindObjectOfType<WordManager>();
        wordManager?.Initialize(levelManager);
        if (wordManager == null) { Debug.Log("Cannot find WordManager in scene"); }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleOnSceneLoaded;
        levelManager.OnGameCompleted -= HandleGameCompleted;
    }

    private void HandleOnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            InitializeManagers();
            levelManager.OnGameCompleted += HandleGameCompleted;
            levelManager.SetLevel(1);
        }
    }

    private void HandleGameCompleted()
    {
        new LevelLoader().LoadScene(3);
        gameFinished = true;
    }

    public void StartGame()
    {
        new LevelLoader().LoadScene(1);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayBackgroundNoise();
    }

    public void StartNewGame() 
    {
        Debug.Log("Enter: Start New Game");
        gameFinished = false;
        SceneManager.LoadScene(1);
        levelManager.NewGame();
        Debug.Log("Exit: Start New Game");
    }
}
