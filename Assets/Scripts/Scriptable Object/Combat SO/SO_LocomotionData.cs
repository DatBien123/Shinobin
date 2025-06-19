using UnityEngine;

namespace Training
{
    
    [System.Serializable]
    public enum ERotateType
    {
        None,
        ToTarget,
        ToMoveDirection
    }
    [System.Serializable]
    public enum EAnimationParamHandleType
    {
        None,
        OnlyForward,
        AdjustToInputDirection
    }
    [System.Serializable]
    public struct MovementData
    {
        [Header("Walk")]
        public ERotateType walkRotateType;
        [Header("Sprint")]
        public ERotateType sprintRotateType;
        [Header("Dodge")]
        public ERotateType dodgeRotateType;
    }
    [System.Serializable]
    public struct AnimationData
    {
        public EAnimationParamHandleType animationParamHandleType;
    }
    [System.Serializable]
    public struct LockOnTargetCameraData
    {
        public MovementData lockOnMovementData;
        public AnimationData lockOnAnimationData;
    }
    [System.Serializable]
    public struct FollowCameraData
    {
        public MovementData followMovementData;
        public AnimationData followAnimationData;
    }
    [System.Serializable]
    public struct LocomotionData
    {
        public FollowCameraData followCameraData;
        public LockOnTargetCameraData lockOnCameraData;
    }
    [System.Serializable]
    [CreateAssetMenu(fileName = "Locomotion Data", menuName = "Combat System/Locomotion/Locomotion Data")]
    public class SO_LocomotionData : ScriptableObject
    {
        public LocomotionData data;
    }
}
