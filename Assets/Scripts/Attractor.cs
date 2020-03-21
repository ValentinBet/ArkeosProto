using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public float Mass;

    public void Attract(Attractor objToAttract)
    {
        Vector3 direction = transform.position - objToAttract.transform.position;
        float distance = direction.magnitude;

      //  float forceMagnitude = (Mass * )
    } 
}
