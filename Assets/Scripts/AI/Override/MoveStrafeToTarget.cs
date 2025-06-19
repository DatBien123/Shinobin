using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Training
{
    [System.Serializable]
    public enum EMoveStrafeType
    {
        WalkLeftStrafe,
        WalkRightStrafe,
        WalkBackStrafe
    }
    [System.Serializable]
    [CreateAssetMenu(fileName = "Behavior Data", menuName = "Combat System/Behavior/Move Strafe To Target Action")]
    public class MoveStrafeToTarget : Action
    {
        public EMoveStrafeType MoveStrafeType;

        public float Duration;
        public override void Execute(CharacterAI agent)
        {
            if (agent.navMeshAgent.enabled)
            {
                agent.currentBehaviorState = EBehaviorState.Executing;
                //agent.navMeshAgent.SetDestination(agent.targetingComponent.target.transform.position);
                agent.StartMoveStrafeToTarget(Duration);

            }
        }
        public override void Finished(CharacterAI agent)
        {

        }

        

    }
}