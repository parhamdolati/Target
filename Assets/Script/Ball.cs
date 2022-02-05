using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private int baseSpeed = 1;
    private void Update()
    {
        if (!Manager.Instance.triggerOnBarrier)
            transform.position += Vector3.up *  (baseSpeed + ((100 + Manager.Instance.GetRecord()) / 50f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Barrier")
        {
            Manager.Instance.triggerOnBarrier = true;
            GetComponent<Animator>().enabled = true;
            Manager.Instance.GameOver();
        }
        else if (other.gameObject.tag == "Target")
        {
            Manager.Instance.RecordUp(gameObject);
        }
    }
}
