using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObj : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           PlayerControl _pc = collision.gameObject.GetComponent<PlayerControl>();
            _pc.playerLife -= _pc.playerLife;
        }
    }
}
