using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject Player;
    
    
    void Update()
    {
        
    }


    void FollowPlayer()
    {
        transform.position = Player.transform.position + transform.position;
    }
}
