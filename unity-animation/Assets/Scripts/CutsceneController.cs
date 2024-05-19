using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public GameObject TimerObject;
    public GameObject PlayerObject;
    public GameObject CutSceneCamera;
    public float TimeToShow = 4f;

    private FirstPersonController FPC;

    private void Start()
    {
        TimerObject.SetActive(false);
        FPC = PlayerObject.GetComponentInChildren<FirstPersonController>();
        FPC.enabled = false;
        CutSceneCamera.SetActive(true);
        StartCoroutine(FollowUp());

    }
    

    IEnumerator FollowUp()
    {
        yield return new WaitForSeconds(TimeToShow);
        TimerObject.SetActive(true);
        CutSceneCamera.SetActive(false);
        FPC.enabled = true;
    }
}
