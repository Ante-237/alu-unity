using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ball : MonoBehaviour
{
    public int forceOfBall = 600;
    public Transform firePoint;
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.forward * forceOfBall, ForceMode.Impulse);
    }

    public void FireBall(Vector3 Direction)
    {
        rb.AddForce(Direction * forceOfBall, ForceMode.Impulse);
    }
}
