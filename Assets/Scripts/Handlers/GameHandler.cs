using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;
    [SerializeField] InputHandler inputHandler;
    

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

    private void OnEnable()
    {
        inputHandler.OnLetterInput += HandleLetterInput;
    }

    private void OnDisable()
    {
        inputHandler.OnLetterInput -= HandleLetterInput;
    }

    private void HandleLetterInput(char inputChar)
    {
        Debug.Log("HELLO WORLD?! " + inputChar);
    }
}
