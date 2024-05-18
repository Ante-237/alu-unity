using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings")]
public class SettingsSO : ScriptableObject
{

    public int PreviousScene = 4;
    public bool optionsActivated = false;
    public float CurrentTime = 0f;
    public Vector3 PlayerPosition = Vector3.zero;
    public bool isInverted = false;
}
