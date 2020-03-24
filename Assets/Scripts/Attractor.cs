using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public float mass;
    public PlayerControl player;
    public float distanceToPlayer;
    public float gravityToPlayer;
    public float size;

    public float fakeGravityForce = 5f;

    private void FixedUpdate()
    {
        //AttractPlayer(player);
    }

    public void AttractPlayer(PlayerControl player)
    {
        Vector3 direction = transform.position - player.transform.position;
        distanceToPlayer = direction.magnitude;

        gravityToPlayer = (size * mass) / Mathf.Pow(distanceToPlayer, 2);

        float forceMagnitude = fakeGravityForce * (gravityToPlayer * (mass * player.rb.mass) / Mathf.Pow(distanceToPlayer, 2));
        Vector3 force = direction.normalized * forceMagnitude;

        //player.GetAttract(force, this);
    } 
}
