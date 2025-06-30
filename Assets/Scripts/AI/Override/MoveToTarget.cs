using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Training
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Behavior Data", menuName = "Combat System/Behavior/Move To Target Action")]
    public class MoveToTarget : Action
    {
        [Header("Props")]
        public float Duration = 3.0f;
        
        public EMoveType MoveType;
        public bool isSprint;

        public override void Execute(CharacterAI agent)
        {
            //if (agent.navMeshAgent.enabled)
            //{
                agent.currentBehaviorState = EBehaviorState.Executing;
                //agent.navMeshAgent.SetDestination(agent.targetingComponent.target.transform.position);
                agent.aIDecision.StartMoveToTarget(MoveType, isSprint);

            //}
        }
        public override void Finished(CharacterAI agent)
        {

        }
        
    }
}