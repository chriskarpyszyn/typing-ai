using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    //TODO: I'd love to have game data centralized somewhere
    private int wordsPerLevel = 20;
    
    private int maxLevel = 3;
    private int currentLevel;
    private int wordsCompletedInCurrentLevel;

    public event Action<int> OnLevelChanged;
    public event Action OnNextWord;
    public event Action OnGameCompleted;
    public event Action OnNewGame;

    public void Initialize()
    {
        //wordsCompletedInCurrentLevel = 1;
        //OnLevelChanged?.Invoke(currentLevel);
    }

    public void WordCompleted()
    {
        if (wordsCompletedInCurrentLevel >= wordsPerLevel)
        {
            AdvanceLevel();
        } else
        {
            NextWord();
        }
    }

    private void NextWord()
    {
        wordsCompletedInCurrentLevel++;
        OnNextWord?.Invoke();
    }

    private void AdvanceLevel()
    {
        currentLevel++;
        wordsCompletedInCurrentLevel = 1;
        InvokeNextLevelOrEndGame();
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
        wordsCompletedInCurrentLevel = 1;
        InvokeNextLevelOrEndGame();
    }

   
    private void InvokeNextLevelOrEndGame()
    {
        if (currentLevel > maxLevel)
        {
            OnGameCompleted?.Invoke();
        }
        else
        {
            OnLevelChanged?.Invoke(currentLevel);
        }
    }

    public void NewGame()
    {
        SetLevel(1);
        OnNewGame?.Invoke();
    }

    public int GetCurrentLevel() => currentLevel;
    public int GetWordsPerLevel() => wordsPerLevel;
    public float GetLevelProgress() => (float)wordsCompletedInCurrentLevel / wordsPerLevel;
    public int GetWordsCompletedInCurrentLevel() => wordsCompletedInCurrentLevel;
}
