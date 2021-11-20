using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Main _main;
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, 
            new Vector3(transform.position.x, 1500, transform.position.z), Time.deltaTime * 5);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Barrier")
        {
            _main.GameOver();
            Destroy(gameObject);
        }

        if (other.collider.tag == "Target")
        {
            _main.RecordUp();
            Destroy(gameObject);
        }
    }
}
