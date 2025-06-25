using System.Collections.Generic;
using UnityEngine;

namespace Training
{
    [System.Serializable]
    public enum EFinisherType
    {
        Ambush,
        Execute
    }
    [System.Serializable]
    public struct CauserData
    {
        public AnimationClip executionClip;
        public Vector3 causerPositionOffset;
        public Vector3 causerRotationOffset;

        public FinisherFX causerFinisherFX;
    }
    [System.Serializable]
    public struct TakerData
    {
        public AnimationClip executedClip;
        public Vector3 takerPositionOffset;
        public Vector3 takerRotationOffset;

        public FinisherFX takerFinisherFX;
    }
    [System.Serializable]
    public struct FinisherFX
    {
        public FinisherVFX finisherVFXData;
        public FinisherSFX finisherSFXData;
    }
    [System.Serializable]
    public struct FinisherVFX
    {
        public ParticleSystem particleSystem;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;
        public Vector3 scaleOffset;
    }
    [System.Serializable]
    public struct FinisherSFX
    {
        public AudioClip audioClip;
        public float volume;
        public float pitch;
    }
    [System.Serializable]
    public struct FinisherData
    {
        public EFinisherType finisherType;
        public CauserData causerData;
        public TakerData takerData;
        public ListHitReactionData ListHitReactionData;
    }
    [CreateAssetMenu(menuName = "Combat System/Combat/Finisher Data", fileName = "Finisher Data")]
    public class SO_Finisher : ScriptableObject
    {
        public FinisherData data;
    }
}
