using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingSO")]
public class SettingSO : ScriptableObject
{

    public int FireForce = 100;
    public float MagnitudeFactor = 1.0f;
    public int Score = 0;
    public int ammo = 5;
    public bool gameStarted = false;

    public Vector3 FireDirection = Vector3.forward;
}
