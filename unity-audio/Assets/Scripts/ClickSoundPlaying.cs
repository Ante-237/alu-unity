using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickSoundPlaying : MonoBehaviour
{
    public Animator PlayerAnimator;
    public AudioSource RunningAudioSource;
    public AudioSource LandingAudioSource;

    public AudioClip SandClip;
    public AudioClip GrassClip;

    public AudioClip LandingSandClip;
    public AudioClip LandingGrassClip;

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

    public void Fallen()
    {
        LandingAudioSource.Play();
    }

    public void Update()
    {
        PlaySoundEffect();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.LogWarning("Tghe collision method is running");

        if (other.gameObject.CompareTag("Ground"))
        {
            RunningAudioSource.clip = SandClip;
            LandingAudioSource.clip = LandingSandClip;  
        }

        if (other.gameObject.CompareTag("GroundGrass"))
        {
            RunningAudioSource.clip = GrassClip;
            LandingAudioSource.clip = LandingGrassClip;
        }

    }
}
