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
    public float GrapplerSlingVelocityMultiplier = 1000f;
    public bool fired;
    public bool hooked;

    private float currentDistance;
    private float hookTime = 0f;
    private Vector3 direction;
    private PlayerControl pc;

    private void Awake()
    {
        hook = GameObject.FindGameObjectWithTag("Hook");
        pc = GetComponent<PlayerControl>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!fired && !hooked)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPositions(new Vector3[0]);
                fired = true;
                direction = Camera.main.transform.forward;
            }
            else if (hooked)
            {
                GivePlayerVelocity();
                ReturnHook();
            }
            else
            {
                ReturnHook();
            }
        }

        if (hooked)
        {
            hookTime += Time.deltaTime;
            fired = false;
            transform.position = Vector3.Lerp(transform.position, hook.transform.position, Time.deltaTime * playerTravelSpeed);

            if (Vector3.Distance(transform.position, hook.transform.position) < distanceToStopHook)
            {
                GivePlayerVelocity();
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
        }
        else
        {
            lineRenderer.SetPositions(new Vector3[2] { this.transform.position, hook.transform.position });
        }
    }

    private void GivePlayerVelocity()
    {
        pc.rb.velocity = Vector3.zero;
        pc.rb.AddForce(direction * (hookTime * 2) * playerTravelSpeed * GrapplerSlingVelocityMultiplier);
    }

    private void ReturnHook()
    {
        lineRenderer.enabled = false;
        fired = false;
        hooked = false;
        hookTime = 0;
    }

}
