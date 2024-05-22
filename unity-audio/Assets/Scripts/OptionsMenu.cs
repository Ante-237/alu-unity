using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public SettingsSO settings;
    public Toggle RotationInverstion;

    private void Start()
    {
        RotationInverstion.onValueChanged.AddListener(RotationDirection);
    }

    private void RotationDirection(bool state)
    {
        settings.isInverted = state;
        RotationInverstion.isOn = state;
    }

    [System.Obsolete]
    public void Back()
    {
        SceneManager.LoadScene(settings.PreviousScene);
    }
}
