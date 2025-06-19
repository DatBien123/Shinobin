using System.Collections;
using UnityEngine;


namespace Training
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Behavior Data", menuName = "Combat System/Behavior/Intro Action")]
    public class Intro : Action
    {

        public override void Execute(CharacterAI agent)
        {
            agent.currentBehaviorState = EBehaviorState.Executing;
            agent.StartExecuteIntro();
        }
        public override void Finished(CharacterAI agent)
        {
            base.Finished(agent);
        }
    }
}