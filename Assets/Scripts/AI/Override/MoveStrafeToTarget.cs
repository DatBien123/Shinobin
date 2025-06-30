using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Training
{
    [System.Serializable]
    public enum EMoveStrafeType
    {
        None = 0,
        LeftStrafe = -1,
        RightStrafe = 1,
        BackStrafe = -1,
    }
    [System.Serializable]
    [CreateAssetMenu(fileName = "Behavior Data", menuName = "Combat System/Behavior/Move Strafe To Target Action")]
    public class MoveStrafeToTarget : Action
    {
        public EMoveType MoveStrafeType;
        public bool isSprint;
        public float Duration;
        public override void Execute(CharacterAI agent)
        {
            //if (agent.navMeshAgent.enabled)
            //{
                agent.currentBehaviorState = EBehaviorState.Executing;
                //agent.navMeshAgent.SetDestination(agent.targetingComponent.target.transform.position);
                agent.aIDecision.StartMoveStrafeToTarget(MoveStrafeType, isSprint, Duration);

            //}
        }
        public override void Finished(CharacterAI agent)
        {

        }

        

    }
}