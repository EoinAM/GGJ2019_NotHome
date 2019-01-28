using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public Rigidbody rb;
    public bool throwing = false;
    public bool thrown;
    public bool falling;

    public Vector3 movement;

    // Use this for initialization

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (throwing & !thrown)
            {
                rb.AddForce(movement);
                thrown = true;
            }

            if (falling)
            {
                if (!rb.useGravity)
                {
                    rb.AddForce(movement);
                    rb.useGravity = true;
                }

            }

        }
    }
}