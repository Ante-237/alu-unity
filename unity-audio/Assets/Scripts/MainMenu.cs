using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SettingsSO settings;

    public void LevelSelect(int level)
    {
        settings.PreviousScene = 4;
        SceneManager.LoadScene(level);
        
    }

    public void Options()
    {
        settings.PreviousScene = 4;
        SceneManager.LoadScene(3);

    }

    public void ExitApplication()
    {
        Application.Quit();
        Debug.Log("Application Exit");
    }
}
