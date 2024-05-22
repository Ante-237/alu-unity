using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickSoundPlaying : MonoBehaviour
{
    public Animator PlayerAnimator;
    public AudioSource RunningAudioSource;

    public AudioClip SandClip;
    public AudioClip GrassClip;

    private bool firstPlaying = false;

    public void PlaySoundEffect()
    {
        if(PlayerAnimator.GetBool("JumpToRunning") || PlayerAnimator.GetInteger("IdleToRunning") != 0)
        {
            if (!firstPlaying)
            {
                RunningAudioSource.Play();
                firstPlaying = true;
            }
         
        }
        else
        {
            RunningAudioSource.Stop();
            firstPlaying = false;
        }
            
    }

    public void Update()
    {
        PlaySoundEffect();
    }


    /*
    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogWarning("Tghe collision method is running");

        if (collision.gameObject.CompareTag("Ground"))
        {
            RunningAudioSource.clip = SandClip;
        }
        else
        {
            RunningAudioSource.clip = GrassClip;
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        // Debug.LogWarning("Tghe collision method is running");

        if (other.gameObject.CompareTag("Ground"))
        {
            RunningAudioSource.clip = SandClip;
        }

        if (other.gameObject.CompareTag("GroundGrass"))
        {
            RunningAudioSource.clip = GrassClip;
        }

    }
}
