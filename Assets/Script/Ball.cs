using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts.Internal;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void Update()
    {
        if (!Manager.Instance.triggerOnBarrier)
            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(transform.position.x, transform.position.y + 40, transform.position.z),
                Time.deltaTime * (100 + Manager.Instance.GetRecord() / 50f));
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
