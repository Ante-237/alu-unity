using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    private float JumpForce = 500f;
    private float MovementSpeed = 20f;
    private Vector3 vertical;
    private Vector3 horizontal;
    private float directionX;
    private float directionZ;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Movements();
        Jumping();
    }
    

    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(transform.position.x, transform.position.y * JumpForce * Time.deltaTime, transform.position.z);
            
        }
       
    }


    void Movements()
    {
        directionX = Input.GetAxisRaw("Horizontal");
        directionZ = Input.GetAxisRaw("Vertical");

        Vector3 movements = new Vector3(directionX , 0, directionZ ) * (Time.deltaTime * MovementSpeed);
        transform.position += movements;

    }

}
