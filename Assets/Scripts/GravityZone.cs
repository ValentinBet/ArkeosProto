﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{
    public GameObject parent;
    
    private void Start()
    {
        parent = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PlayerControl _pc = other.GetComponent<PlayerControl>();
            _pc.EnterGravityZone(this);
            this.parent.layer = 8;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControl _pc = other.GetComponent<PlayerControl>();
            _pc.ExitGravityZone(this);
            this.parent.layer = 0;
        }
    }
}
