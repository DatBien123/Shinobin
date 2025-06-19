using System.Collections;
using UnityEngine;
using UnityEngine.AI;


namespace Training
{
    [System.Serializable]
    public enum ETeleportType
    {
        Left,
        Right,
        Forward,
        Back
    }
    [System.Serializable]
    public enum ETeleportTargetType
    {
        Self,
        Target
    }
    [System.Serializable]
    [CreateAssetMenu(fileName = "Behavior Data", menuName = "Combat System/Behavior/Teleport Action")]
    public class Teleport : Action
    {
        public ETeleportTargetType teleportTargetType;

        public ETeleportType teleportType;
        public float teleportDistance;
        public float duration;
        public override void Execute(CharacterAI agent)
        {
            agent.comboComponent.ResetCombo();
            //agent.skillComponent.ResetSkill();

            agent.teleportComponent.StartTeleport(GetTeleportPosition(agent, teleportTargetType, teleportType), duration);
        }
        public override void Finished(CharacterAI agent)
        {
            base.Finished(agent);
        }
        public Vector3 GetTeleportPosition(CharacterAI agent,ETeleportTargetType teleportTargetType, ETeleportType teleportType)
        {
            if (teleportTargetType == ETeleportTargetType.Target)
            {
                switch (teleportType)
                {
                    case ETeleportType.Left:
                        return (agent.targetingComponent.target.transform.position + -teleportDistance * agent.targetingComponent.target.transform.right);
                    case ETeleportType.Right:
                        return (agent.targetingComponent.target.transform.position + teleportDistance * agent.targetingComponent.target.transform.right);
                    case ETeleportType.Forward:
                        return (agent.targetingComponent.target.transform.position + teleportDistance * agent.targetingComponent.target.transform.forward);
                    case ETeleportType.Back:
                        return (agent.targetingComponent.target.transform.position + -teleportDistance * agent.targetingComponent.target.transform.forward);
                }
                return (agent.targetingComponent.target.transform.position + -teleportDistance * agent.targetingComponent.target.transform.right);
            }
            else
            {
                switch (teleportType)
                {
                    case ETeleportType.Left:
                        return (agent.transform.position + -teleportDistance * agent.transform.right);
                    case ETeleportType.Right:
                        return (agent.transform.position + teleportDistance * agent.transform.right);
                    case ETeleportType.Forward:
                        return (agent.transform.position + teleportDistance * agent.transform.forward);
                    case ETeleportType.Back:
                        return (agent.transform.position + -teleportDistance * agent.transform.forward);
                }
                return (agent.transform.position + -teleportDistance * agent.transform.right);
            }
        }
    }
}