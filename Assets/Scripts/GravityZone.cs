using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{
    public GameObject parent;
    public MeshCollider parentCollider;
    public float gravZonePower = 20f;
    
    private void Start()
    {
        parent = transform.parent.gameObject;
        parentCollider = parent.GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControl _pc = other.GetComponent<PlayerControl>();
            _pc.gravZonePower = this.gravZonePower;
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
