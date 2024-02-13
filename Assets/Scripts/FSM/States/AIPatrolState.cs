using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIPatrolState : AIState
{
    Vector3 destination;
    public AIPatrolState(AIStateAgent agent) : base(agent)
    {
        AIStateTransition transition = new AIStateTransition(nameof(AIIdleState));
        transition.AddCondition(new FloatCondition(agent.destinationDistance, Condition.Predicate.LESS, 1));
        transitions.Add(transition);

        transition = new AIStateTransition(nameof(AIChaseState));
        transition.AddCondition(new BoolCondition(agent.enemySeen));
        transitions.Add(transition);
    }

    public override void OnEnter()
    {
        agent.movement.Resume();

        var navNode = AINavNode.GetRandomAINavNode();
        destination = navNode.transform.position;
    }
    
    public override void OnUpdate()
    {
        agent.movement.MoveTowards(destination);
        Debug.Log(destination);
        Debug.DrawLine(agent.transform.position, destination, Color.magenta);
        //var enemies = agent.enemyPerception.GetGameObjects();
        //if(enemies.Length > 0)
        //{
        //    agent.stateMachine.SetState(nameof(AIChaseState));
        //}
    }
    public override void OnExit()
    {

    }


}