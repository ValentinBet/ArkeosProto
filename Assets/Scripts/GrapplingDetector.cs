using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingDetector : MonoBehaviour
{
    [SerializeField] GrapplingHook grapplingHook;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Immaterial") && grapplingHook.fired)
        {
            grapplingHook.hooked = true;
        }

    }
}
