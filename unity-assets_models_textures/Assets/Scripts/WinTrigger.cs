using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public Timer timer;

    private void OnTriggerEnter(Collider other)
    {
        timer.isStarted = false;
        timer.ChangeToGreen();
    }
}
