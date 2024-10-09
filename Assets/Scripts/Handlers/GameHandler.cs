using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
        levelManager.OnLevelChanged += HandleLevelChanged;
        levelManager.OnGameCompleted += HandleGameCompleted;
        SceneManager.sceneLoaded += HandleOnSceneLoaded;
    }

    private void OnDisable()
    {
        levelManager.OnLevelChanged -= HandleLevelChanged;
        levelManager.OnGameCompleted -= HandleGameCompleted;
        SceneManager.sceneLoaded -= HandleOnSceneLoaded;
    }

    private void HandleOnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            levelManager.SetLevel(1);
        }
    }

    private void HandleLevelChanged(int i)
    {
        Debug.Log($"Advancing to level: {i}");
        //TODO: implement logic here
    }

    private void HandleGameCompleted()
    {
        oldGameManager.EndGame();
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
