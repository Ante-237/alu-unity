using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene(4);
    }

    public void Next()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentIndex > 2)
        {
            SceneManager.LoadScene(4);
            return;
        }

        SceneManager.LoadScene(currentIndex + 1);
    }
}
