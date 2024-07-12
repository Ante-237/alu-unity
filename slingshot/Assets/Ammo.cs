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

    private bool runOnce = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("obstacle"))
        {
            if (!runOnce)
            {
                settings.Score += 1;
                runOnce = true;
                Destroy(collision.gameObject);
                gameObject.SetActive(false);
            }          
        }
    }
}
