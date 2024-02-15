using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// controls various menu settings for main menu
/// </summary>
public class MainMenu : MonoBehaviour
{

    public Material trapMat;
    public Material goalMat;
    public Toggle colorblindMode;

    /// <summary>
    ///  quits application
    /// </summary>
    public void QuitMaze()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
    
    /// <summary>
    /// loads maze scene
    /// </summary>
    public void PlayMaze()
    {
        if (colorblindMode.isOn)
        {
            trapMat.color = new Color32(255, 112, 0, 1);
            goalMat.color = Color.blue;
        }
        SceneManager.LoadScene("maze");
    }
    
    
    
    
}
