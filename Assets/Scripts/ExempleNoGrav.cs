using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExempleNoGrav : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody>().AddTorque(Vector3.up * 40);
    }
}
