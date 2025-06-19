using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

namespace Training
{
    [System.Serializable]
    public enum EDodgeDirection
    {
        None = 0,
        Forward,
        Backward,
        Right,
        Left
    }
    [System.Serializable]
    [CreateAssetMenu(fileName = "Behavior Data", menuName = "Combat System/Behavior/Dodge Action")]
    public class Dodge : Action
    {
        public EDodgeDirection Direction = EDodgeDirection.None;
        public override void Execute(CharacterAI agent)
        {
            agent.comboComponent.ResetCombo();
            //agent.skillComponent.ResetSkill();

            Vector3 item1 = GetDodgeDirection(agent, Direction).Item1;
            Vector2 item2 = GetDodgeDirection(agent, Direction).Item2;

            agent.dodgeComponent.StartDodgeAI(item1, item2);
        }
        public override void Finished(CharacterAI agent)
        {
            base.Finished(agent);
        }

        public (Vector3, Vector2) GetDodgeDirection(CharacterAI agent, EDodgeDirection Direction)
        {

            switch (Direction)
            {
                case EDodgeDirection.Backward:
                    return (-agent.transform.forward, new Vector2(0, -1));
                case EDodgeDirection.Forward:
                    return (agent.transform.forward, new Vector2(0, 1));
                case EDodgeDirection.Left:
                    return (-agent.transform.right, new Vector2(-1, 0));
                case EDodgeDirection.Right:
                    return (agent.transform.right, new Vector2(1, 0));
                default:
                    return (agent.transform.right, new Vector2(1, 0));
            }
        }
    }
}