using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private float BulletSpeed = 10f;
    [SerializeField] private float TimeToDestroy = 2.0f;
    private void Update()
    {
        FlyForward();
    }

    private void Start()
    {
        Destroy(gameObject, TimeToDestroy);
    }

    void FlyForward()
    {
       // transform.position = new Vector3(0,   ( BulletSpeed), 0);
        transform.Translate(Vector3.up * (BulletSpeed * Time.deltaTime), Space.World);
    }
}
