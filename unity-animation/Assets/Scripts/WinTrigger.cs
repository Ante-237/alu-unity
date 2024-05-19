using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
    public Timer timer;
    public GameObject WinCanvas;
    public GameObject DefaultTime;
    public Text MenuTime;


    private void Start()
    {
        WinCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        timer.isStarted = false;
        DefaultTime.SetActive(false);
        MenuTime.text = timer.seconds.ToString("00:00.00");
        WinCanvas.SetActive(true);
        timer.ChangeToGreen();
    }
}
