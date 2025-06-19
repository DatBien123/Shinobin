using System.Collections;
using UnityEngine;


namespace Training
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Behavior Data", menuName = "Combat System/Behavior/Jump Action")]
    public class Jump : Action
    {

        public float jumpHeight;
        public float jumpDuration;
        public override void Execute(CharacterAI agent)
        {
            agent.comboComponent.ResetCombo();

            //agent.StartJump(jumpHeight, jumpDuration);

        }
        public override void Finished(CharacterAI agent)
        {
            base.Finished(agent);
        }
    }
}