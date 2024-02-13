using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKinematicMovement : AiMovement
{
    public override void ApplyForce(Vector3 force)
    {
        acceleration += force;
    }

    public override void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        ApplyForce(direction * maxForce);
    }

    public override void Stop()
    {
        Velocity = Vector3.zero;
    }

    public override void Resume()
    {
        //
    }

    private void Awake()
    {
        //
    }

    void LateUpdate()
    {
        Velocity += acceleration * Time.deltaTime;
        Velocity = Vector3.ClampMagnitude(Velocity, maxSpeed);
        transform.position += Velocity * Time.deltaTime;

        if (Velocity.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(Velocity);
        }

        acceleration = Vector3.zero;
    }
}
