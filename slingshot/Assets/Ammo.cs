using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public SettingSO settings;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FireBall(Vector3 direction)
    {
        rb.AddForce(settings.FireDirection * settings.FireForce * settings.MagnitudeFactor, ForceMode.Force);
    }
}
