using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Timer : MonoBehaviour
{
   public TextMeshProUGUI TimerText;

   public bool isStarted = false;

   private void Update()
   {
      if (isStarted)
      {
         UpdateTimer();
      }
   }

   private void UpdateTimer()
   {
      float seconds = Time.time;
      int hours = (int)seconds / 60;
      TimerText.text = hours + ":" + seconds.ToString("00.00");
   }

   public void ChangeToGreen()
   {
      TimerText.color = Color.green;
   }

   private void OnTriggerEnter(Collider other)
   {
      isStarted = true;
   }
}
