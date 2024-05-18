using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public SettingsSO settings;

    public void Back()
    {
        SceneManager.LoadScene(settings.PreviousScene);
    }
}
