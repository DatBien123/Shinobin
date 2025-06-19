using UnityEngine;

namespace Training
{
    [System.Serializable]
    public enum EBehaviorState
    {
        None = 0,
        Executing,
        Finished
    }

    [System.Serializable]
    public enum EBehaviorCooldown
    {
        None = 0,
        Ready,
        WaitForCooldown
    }
    [CreateAssetMenu(fileName = "Behavior Data", menuName = "Combat System/Behavior")]
    [System.Serializable]
    public abstract class SO_Behavior : ScriptableObject
    {
        public float Cooldown;
        public EBehaviorCooldown EBehaviorCooldown = EBehaviorCooldown.Ready;
        public SO_Behavior currentClone = null;
        public abstract void Execute(CharacterAI agent);
        public abstract void Finished(CharacterAI agent);

        public bool isFinished(CharacterAI agent)
        {
            return agent.currentBehaviorState == EBehaviorState.Finished ? true : false;
        }

        public SO_Behavior Clone()
        {
            if (currentClone == null)
            {
                currentClone = Instantiate(this);
            }
            return currentClone;
        }
    }
}
