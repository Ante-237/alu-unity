using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        
    }

    public void PlayAnimation()
    {
        animator.SetTrigger("On");

    }

    public void StopAnimation()
    {
        animator.SetTrigger("Off");
    }

    public void runUp()
    {
        Application.OpenURL("https://github.com/Ante-237");
    }
}
