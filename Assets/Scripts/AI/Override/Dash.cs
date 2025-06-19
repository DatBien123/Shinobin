using UnityEngine;

namespace Training
{
    [System.Serializable]
    public enum EDashType
    {
        CloseDistance,
        GapDistance,
        Horizontal
    }
    [System.Serializable]
    [CreateAssetMenu(fileName = "Behavior Data", menuName = "Combat System/Behavior/Dash Action")]
    public class Dash : Action
    {
        public EDashType dashType = EDashType.CloseDistance;
        public override void Execute(CharacterAI agent)
        {
            agent.comboComponent.ResetCombo();
            //agent.skillComponent.ResetSkill();

            Vector3 item1 = GetDashDirection(agent).Item1;
            Vector2 item2 = GetDashDirection(agent).Item2;

            agent.dodgeComponent.StartDashAI(item1, item2);
        }
        public override void Finished(CharacterAI agent)
        {
            base.Finished(agent);
        }

        public (Vector3, Vector2) GetDashDirection(CharacterAI agent)
        {
            if (dashType == EDashType.GapDistance)
            {
                return (-agent.transform.forward, new Vector2(0, -1));
            }
            else if (dashType == EDashType.Horizontal)
            {

                int randomNumber = Random.Range(0, 2);
                if (randomNumber == 0) return (-agent.transform.right, new Vector2(-1, 0));
                else return (agent.transform.right, new Vector2(1, 0));
            }
            else return (agent.transform.forward, new Vector2(0, 1));
        }
    }
}