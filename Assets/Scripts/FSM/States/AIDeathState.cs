using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    float timer = 0;

    public AIDeathState(AIStateAgent agent) : base(agent)
    {

    }

    public override void OnEnter()
    {
        agent.movement.Stop();
        agent.movement.Velocity = Vector3.zero;
        agent.animator?.SetTrigger("Death");
        agent.timer.value = 2;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if(Time.time > timer)
        {
            GameObject.Destroy(agent.gameObject);
        }
    }
}
