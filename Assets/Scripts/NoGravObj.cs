using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGravObj : MonoBehaviour
{
    public bool isActivated = true;
    public float testTorquePower = 100f;

    private Rigidbody rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.AddTorque(Vector3.up * testTorquePower * rb.mass);
    }

    private void Update()
    {
        if (isActivated)
        {
            rb.useGravity = false;
        } else
        {
            rb.useGravity = true;
        }
    }
}
