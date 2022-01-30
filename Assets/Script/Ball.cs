using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool collisionOnBarrier = false;
    private void Update()
    {
        if(!collisionOnBarrier)
            transform.position += Vector3.up;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Barrier"))
        {
            collisionOnBarrier = true;
            Debug.Log("Barrier");
            //Manager.Instance.GameOver();
        }
        else if (other.CompareTag("Target"))
        {
            Debug.Log("Target");
            //Manager.Instance.RecordUp(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
    }
}
