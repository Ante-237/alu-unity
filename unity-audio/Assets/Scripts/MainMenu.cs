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
        settings.PreviousScene = 0;
        SceneManager.LoadScene(level);
        
    }

    public void Options()
    {
        settings.PreviousScene = 0;
        SceneManager.LoadScene(1);

    }

    public void ExitApplication()
    {
        Application.Quit();
        Debug.Log("Application Exit");
    }
}
