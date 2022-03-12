using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistence : MonoBehaviour
{
    public static DataPersistence Instance;

    // Save the playerName between sessions.
    // Set in MainMenu is accessed in MainScene
    [HideInInspector] public string PlayerName;
    [HideInInspector] public int HighScore;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayerName = String.Empty;
        HighScore = 0;
    }
}
