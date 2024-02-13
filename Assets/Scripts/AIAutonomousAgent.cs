using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AIAutonomousAgent : AIAgent
{
    public AIPerception seekperception = null;
    public AIPerception fleeperception = null;
    public AIPerception flockperception = null;
    public AIPerception obstacleperception = null;

    private void Update()
    {
        //seek
        if(seekperception != null)
        {
            var gameObjects = seekperception.GetGameObjects();
            if(gameObjects.Length > 0 )
            {
                Vector3 force = Seek(gameObjects[0]);
                movement.ApplyForce(force);
            }
        }

        //flee
        if (fleeperception != null)
        {
            var gameObjects = fleeperception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                Vector3 force = Flee(gameObjects[0]);
                movement.ApplyForce(force);
            }
        }

        //flock
        if (flockperception != null)
        {
            var gameObjects = flockperception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                movement.ApplyForce(Cohesion(gameObjects));
                movement.ApplyForce(Seperation(gameObjects, 0.5f));
                movement.ApplyForce(Alignment(gameObjects));
            }
        }

        //obstacle avoid
        if(obstacleperception != null)
        {
            if (((AISphereCastPerception)obstacleperception).CheckDirection(Vector3.forward))
            {
                Vector3 open = Vector3.zero;
                if (((AISphereCastPerception)obstacleperception).GetOpenDirection(ref open))
                {
                    movement.ApplyForce(GetSteeringForce(open) * 5); // *5 = the change of how much they want to avoid
                }
            }
        }

        //cancel y movement
        Vector3 acceleration = movement.acceleration;
        acceleration.y = 0;
        movement.acceleration = acceleration;

        //wrap position in world
        transform.position = Utilities.Wrap(transform.position, new Vector3(-10, -10, -10), new Vector3(10, 10, 10));
    }

    private Vector3 Seek(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);

        return force;
    }

    private Vector3 Flee(GameObject target)
    {
        Vector3 direction = transform.position - target.transform.position;
        Vector3 force = GetSteeringForce(direction);

        return force;
    }

    private Vector3 Cohesion(GameObject[] neighbors)
    {
        Vector3 positions = Vector3.zero;
        foreach(var neighbor in neighbors)
        {
            positions += neighbor.transform.position;
        }

        Vector3 center = positions / neighbors.Length;
        Vector3 direction = center - transform.position;

        Vector3 force = GetSteeringForce(direction);

        return force;
    }

    private Vector3 Seperation(GameObject[] neighbors, float radius)
    {
        Vector3 seperation = Vector3.zero;
        foreach(var neighbor in neighbors)
        {
            Vector3 direction = (transform.position - neighbor.transform.position);
            if(direction.magnitude < radius)
            {
                seperation += direction / direction.sqrMagnitude;
            }
        }
        Vector3 force = GetSteeringForce(seperation);

        return force;
    }

    private Vector3 Alignment(GameObject[] neighbors)
    {
        Vector3 velocities = Vector3.zero;
        foreach(var neighbor in neighbors)
        {
            velocities += neighbor.GetComponent<AIAgent>().movement.Velocity;
        }
        Vector3 averageVelocity = velocities / neighbors.Length;

        Vector3 force = GetSteeringForce(averageVelocity);

        return force;
    }

    private Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.maxSpeed;
        Vector3 steer = desired - movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.maxForce);

        return force;
    }

}
