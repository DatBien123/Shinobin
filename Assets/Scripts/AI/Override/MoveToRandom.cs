using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Training
{
    [System.Serializable]
    public enum EMoveType
    {
        None = 0,
        NormalMove,
        StrafeMove
    }
    [CreateAssetMenu(fileName = "Behavior Data", menuName = "Combat System/Behavior/Move To Random Action")]
    public class MoveToRandom : Action
    {
        [Header("Props")]
        public float moveDuration = 1.0f;

        public EMoveType MoveType;
        public override void Execute(CharacterAI agent)
        {
            Vector3 direction = CalculatedRandomDirection();
            //agent.StartMoveRandom(moveDuration, direction);
        }
        public override void Finished(CharacterAI agent)
        {
            base.Finished(agent);
        }
        Vector3 CalculatedRandomDirection()
        {
            int randomDirectionIndex = UnityEngine.Random.Range(1, 5);
            Vector3 moveDirection = Vector3.zero;
            switch (randomDirectionIndex)
            {
                case 1:
                    moveDirection = Vector3.forward;
                    break;
                case 2:
                    moveDirection = Vector3.back;
                    break;
                case 3:
                    moveDirection = Vector3.right;
                    break;
                case 4:
                    moveDirection = Vector3.left;
                    break;
            }

            return moveDirection;

        }
    }
}