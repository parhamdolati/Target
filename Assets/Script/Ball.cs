using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void Update()
    {
        //transform.position += Vector3.up;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Barrier")
        {
            Manager.Instance.GameOver();
        }
        else if (other.tag == "Target")
        {
            Manager.Instance.RecordUp(gameObject);
        }
    }
}
