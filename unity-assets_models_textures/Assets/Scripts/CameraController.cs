using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private float AngelRotation = 45f;
    
    private Vector3 DistanceApart = Vector3.zero;


    private void Awake()
    {
        DistanceApart =  transform.position - Player.transform.position;
    }


    private void Update()
    {
        FollowPlayer();
        RotateAroundPlayer();
    }

    void RotateAroundPlayer()
    {
        float RotationYDirection = Input.GetAxis("Mouse X");
        transform.RotateAround(Player.transform.position, Vector3.up, RotationYDirection * RotationYDirection);
    }

    void FollowPlayer()
    {
        transform.position = Player.transform.position + DistanceApart;
    }
}
