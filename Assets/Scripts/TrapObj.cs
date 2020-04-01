using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObj : MonoBehaviour
{
    float damages = 1;
    private PlayerControl pc;

    private void OnCollisionStay(Collision collision)
    {
        if (pc == null)
        {
            pc = collision.gameObject.GetComponent<PlayerControl>();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            pc.playerLife -= damages;
        }
    }
}
