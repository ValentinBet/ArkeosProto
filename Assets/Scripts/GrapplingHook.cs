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
    public float distanceToStopHook = 1f;
    public bool fired;
    public bool hooked;

    private float currentDistance;
    private Vector3 direction;
    private PlayerControl pc;

    private void Awake()
    {
        pc = GetComponent<PlayerControl>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !fired && !hooked)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPositions(new Vector3[0]);
            fired = true;
            direction = Camera.main.transform.forward;
        } else if (Input.GetMouseButtonDown(0))
        {
            ReturnHook();
        }


        if (hooked)
        {
            fired = false;
            transform.position = Vector3.Lerp(transform.position, hook.transform.position, Time.deltaTime * playerTravelSpeed);

            if (Vector3.Distance(transform.position, hook.transform.position) < distanceToStopHook)
            {
                ReturnHook();
                pc.rb.velocity = Vector3.zero;
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
