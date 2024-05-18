using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
   public Text TimerText;
   public SettingsSO settings;

   public bool isStarted = false;
   public float seconds = 0f;

   private void Update()
   {
      if (isStarted)
      {
         UpdateTimer();
      }
   }

   private void UpdateTimer()
   {
      seconds += Time.deltaTime;
      int hours = (int)seconds / 60;
      TimerText.text =  hours + ":" + seconds.ToString("00.00");
   }

   public void ChangeToGreen()
   {
      TimerText.color = Color.green;
   }

   private void OnTriggerEnter(Collider other)
   {
        if (settings.optionsActivated)
        {
            isStarted = false;
           
            int hours = (int)settings.CurrentTime / 60;
            TimerText.text = hours + ":" + settings.CurrentTime.ToString("00.00");
        }
        else
        {
            isStarted = true;
        }
    
   }
}
