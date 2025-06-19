using System.Collections;
using UnityEngine;


namespace Training
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Behavior Data", menuName = "Combat System/Behavior/Block Action")]
    public class Block : Action
    {
        public float Duration;
        public override void Execute(CharacterAI agent)
        {
            //agent.blockComponent.StartBlockAI(Duration);
        }
        public override void Finished(CharacterAI agent)
        {
            base.Finished(agent);
        }
    }
}