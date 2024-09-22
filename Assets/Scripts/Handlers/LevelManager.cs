using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    //TODO: I'd love to have game data centralized somewhere
    [SerializeField] private int wordsPerLevel = 5;
    
    private int initialLevel = 1;
    private int maxLevel = 3;
    private int currentLevel;
    private int wordsCompletedInCurrentLevel;

    public event Action<int> OnLevelChanged;
    public event Action OnGameCompleted;

    public void Initialize()
    {
        currentLevel = initialLevel;
        wordsCompletedInCurrentLevel = 0;
        OnLevelChanged?.Invoke(currentLevel);
    }

    public void WordCompleted()
    {
        wordsCompletedInCurrentLevel++;
        if (wordsCompletedInCurrentLevel >= wordsPerLevel)
        {
            AdvanceLevel();
        }
    }

    private void AdvanceLevel()
    {
        currentLevel++;
        wordsCompletedInCurrentLevel = 0;

        if (currentLevel > maxLevel)
        {
            OnGameCompleted?.Invoke();
        } 
        else
        {
            OnLevelChanged?.Invoke(currentLevel);
        }
    }

    public int GetCurrentLevel() => currentLevel;
    public int GetWordsPerLevel() => wordsPerLevel;
    public float GetLevelProgress() => (float)wordsCompletedInCurrentLevel / wordsPerLevel;
}
