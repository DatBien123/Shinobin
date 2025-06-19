using System.Collections;
using UnityEngine;


namespace Training
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Behavior Data", menuName = "Combat System/Behavior/Turn Action")]
    public class Turn : Action
    {
        public float turnDuration = 1.0f;
        public override void Execute(CharacterAI agent)
        {
            //agent.lookAtComponent.StartTurnToTarget();
        }
        public override void Finished(CharacterAI agent)
        {
            base.Finished(agent);
        }
    }
}