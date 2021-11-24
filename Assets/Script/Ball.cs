using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Main _main;
    private bool collisionOnBarrier = false;
    void Update()
    {
        if(!collisionOnBarrier)
            transform.position = Vector3.Lerp(transform.position, 
            new Vector3(transform.position.x, 2000, transform.position.z), Time.deltaTime * 4);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Barrier")
        {
            collisionOnBarrier = true;
            _main.lastBall.Add(gameObject);
            _main.GameOver();
        }

        if (other.collider.tag == "Target")
        {
            _main.RecordUp();
            Destroy(gameObject);
        }
    }
}
