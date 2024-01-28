using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    [SerializeField] private float JumpForce = 300f;
    
    [SerializeField] private float MovementSpeed = 20f;
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
            rb.AddForce(Vector3.up * JumpForce);
        }
       
    }


    void Movements()
    {
        directionX = Input.GetAxisRaw("Horizontal");
        directionZ = Input.GetAxisRaw("Vertical");

        Vector3 movements = new Vector3(directionX, 0, directionZ);//* (Time.deltaTime * MovementSpeed);
        //transform.position += movements;
        rb.AddForce( movements * (Time.deltaTime * MovementSpeed));

    }

}
