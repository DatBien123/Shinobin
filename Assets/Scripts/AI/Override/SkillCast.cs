using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Training
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Behavior Data", menuName = "Combat System/Behavior/Skill Cast Action")]
    public class SkillCast : SkillAction
    {
        public override void Execute(CharacterAI agent)
        {
            //agent.skillComponent.SaveSkillAI(skill);
        }
        public override void Finished(CharacterAI agent)
        {
            base.Finished(agent);
        }
    }
}