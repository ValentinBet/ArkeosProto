using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public GameObject hook;
    public GameObject hookHolder;
    public LineRenderer lineRenderer;

    public float hookTravelSpeed;
    public float playerTravelSpeed;
    public float maxDistance;
    public bool fired;
    public bool hooked;

    private float currentDistance;
    private Vector3 direction;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !fired)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPositions(new Vector3[0]);
            fired = true;
            direction = Camera.main.transform.forward;
        }


        if (hooked)
        {
            fired = false;
            transform.position = Vector3.Lerp(transform.position, hook.transform.position, Time.deltaTime * playerTravelSpeed);

            if (Vector3.Distance(transform.position, hook.transform.position) < 2)
            {
                ReturnHook();
            }
        }

        if (fired)
        {

            hook.transform.Translate(direction * Time.deltaTime * hookTravelSpeed);
            currentDistance = Vector3.Distance(transform.position, hook.transform.position);

            if (currentDistance > maxDistance)
            {
                ReturnHook();
            }
        }

        if (!fired && !hooked)
        {
            hook.transform.position = hookHolder.transform.position;
        } else
        {
            lineRenderer.SetPositions(new Vector3[2] { this.transform.position, hook.transform.position });
        }
    }


    private void ReturnHook()
    {
        lineRenderer.enabled = false;
        fired = false;
        hooked = false;
    }

}
