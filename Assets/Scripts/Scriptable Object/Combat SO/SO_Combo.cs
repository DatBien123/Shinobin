
using System.Collections.Generic;
using UnityEngine;
using static Training.FreeflowComponent;

namespace Training
{




    [System.Serializable]
    public enum EAirborneDirection
    {
        None = 0,
        Up,
        Down
    }
    [System.Serializable]
    public enum EAttackDirection
    {
        None = 0,
        Left,
        Right
    }
    [System.Serializable]
    public enum ECollideType
    {
        None = 0,
        AirToFloor,
        FloorToAir,
        OnFloor
    }

    #region ReactData

    [System.Serializable]
    public enum EIntensityType
    {
        Normal,
        Heavy,
        Massive
    }
    [System.Serializable]
    public enum EHitReactDirection
    {
        F,FL,FR,B,BL,BR,L,R
    }
    [System.Serializable]
    public struct IntensityData
    {
        public EIntensityType intensityType;
        public float intensity;
    }
    [System.Serializable]
    public struct AnimationReactData
    {
        public EHitReactDirection hitReactDirection;
        public AnimationClip hitReactClip;

    }
    [System.Serializable]
    public struct ReactData
    {
        public bool isRotateToHitDirection;
        public IntensityData intensityData;
        public List<AnimationReactData> animationReactDatas;
    }
    #endregion

    #region Knockback

    [System.Serializable]
    public struct KnockbackData
    {
        public bool isKnockback;
        public float knockbackTime;
        public float knockbackDistance;
        public ReactData reactData;
        public KnockbackFX knockbackFX;
        public KnockbackSFX knockbackSFX;
    }
    [System.Serializable]
    public struct KnockbackFX
    {
        public ParticleSystem KnockbackParticleSystem;
        public Vector3 KnockbackFXPositionOffset;
        public Vector3 KnockbackFXRotationOffset;
        public Vector3 KnockbackFXScaleOffset;
    }
    [System.Serializable]
    public struct KnockbackSFX
    {
        public AudioClip knockbackAudioClip;
        public float volume;
        public float pitch;
    }
    #endregion
    #region Airborne
    [System.Serializable]
    public struct AirborneData
    {
        public bool isAirborne;
        public EAirborneDirection airborneDirection;
        public float height;
        public float airborneDuration;
        public float airborneTime;
        public AnimationClip hitAirborneClip;
        public AirborneFX airborneFX;
        public AirborneSFX airborneSFX;
    }
    [System.Serializable]
    public struct AirborneFX
    {
        public ParticleSystem AirborneParticleSystem;
        public Vector3 AirborneFXPositionOffset;
        public Vector3 AirborneFXRotationOffset;
        public Vector3 AirborneFXScaleOffset;
    }
    [System.Serializable]
    public struct AirborneSFX
    {
        public AudioClip knockbackAudioClip;
        public float volume;
        public float pitch;
    }
    #endregion
    #region CollideHit
    [System.Serializable]
    public struct CollideHitData
    {
        public bool isCollide;
        public ECollideType collideType;
        public AnimationClip collideClip;
        public ColliderHitFX colliderHitFX;
        public ColliderHitSFX colliderHitSFX;
    }
    [System.Serializable]
    public struct ColliderHitFX
    {
        public GameObject CollideParticleSystem;
        public Vector3 CollideFXPositionOffset;
        public Vector3 CollideFXRotationOffset;
        public Vector3 CollideFXScaleOffset;
    }
    [System.Serializable]
    public struct ColliderHitSFX
    {
        public AudioClip collideHitAudioClip;
        public float volume;
        public float pitch;
    }
    #endregion
    #region Slash
    [System.Serializable]
    public struct SlashData
    {
        public SlashFX slashFX;
        public SlashSFX slashSFX;
    }
    [System.Serializable]
    public struct SlashFX
    {
        public ParticleSystem slashParticleSystem;
        public Vector3 slashFXPositionOffset;
        public Vector3 slashFXRotationOffset;
        public Vector3 slashFXScaleOffset;
    }
    [System.Serializable]
    public struct SlashSFX
    {
        public AudioClip slashAudioClip;
        public float volume;
        public float pitch;
    }
    #endregion
    #region Hit
    [System.Serializable]
    public struct HitData
    {
        public HitFX hitFX;
        public HitSFX hitSFX;
    }
    [System.Serializable]
    public struct HitFX
    {
        public ParticleSystem hitParticleSystem;
        public Vector3 hitFXPositionOffset;
        public Vector3 hitFXRotationOffset;
        public Vector3 hitFXScaleOffset;
    }
    [System.Serializable]
    public struct HitSFX
    {
        public AudioClip hitAudioClip;
        public float volume;
        public float pitch;
    }
    #endregion
    #region Block
    [System.Serializable]
    public struct BlockData
    {
        public bool isBlockable;
        public float knockbackTime;
        public float knockbackDistance;  

        public AnimationClip defenceReactClip;
        public AnimationClip parryReactClip;
        public AnimationClip brokenReactClip;
        public AnimationClip reboundReactClip;


        public BlockFX defenceFX;

        public BlockFX parryFX;

        public BlockFX brokenFX;

        public BlockFX reboundFX;
    }
    [System.Serializable]
    public struct BlockFX
    {
        [Tooltip("Parrying FX")]
        public ParticleSystem blockReactParticleSystem;
        public Vector3 blockReactFXPositionOffset;
        public Vector3 blockReactFXRotationOffset;
        public Vector3 blockReactFXScaleOffset;

        [Header("Parrying SoundFX")]
        public AudioClip blockReactAudioClip;
        public float volume;
        public float pitch;
    }
    [System.Serializable]
    public struct BlockSFX
    {

    }
    #endregion
    #region AttackExtend
    [System.Serializable]
    public struct AttackExtendData
    {
        public bool isApplyHeightWithOwner;
        public float height;

        public AttackExtendFX attackExtendFX;
        public AttackExtendSFX attackExtendSFX;
    }
    [System.Serializable]
    public struct AttackExtendFX
    {
        public GameObject attackFXExtendGO;
        public Vector3 attackFXPositionOffset;
        public Vector3 attackFXRotationOffset;
        public Vector3 attackFXScaleOffset;
    }
    [System.Serializable]
    public struct AttackExtendSFX
    {
        public AudioClip attackExtendAudioClip;
        public float volume;
        public float pitch;
    }
    #endregion

    [System.Serializable]
    public struct HitImpact
    {
        public KnockbackData KnockbackData;
        public AirborneData AirborneData;
        public CollideHitData CollideHitData;
        public HitData HitData;
        public BlockData BlockData;
    }
    [System.Serializable]
    public struct HitTrace
    {
        public ETraceType traceType;
        public float traceAngle;
        [Tooltip("TraceType is Sphere")]
        public float traceRadius;
        [Tooltip("TraceType is Box")]
        public Vector3 boxOffset;
        public Vector3 boxSize;
    }
    [System.Serializable]
    public struct HitFeedbackData
    {
        public bool isApplyWithoutHit;
        [Header("1. Camera Shake")]
        public bool isShake;
        public EShakeDirection shakeDirection;
        public float shakeTime;
        public float intensity;

        [Header("2. Stop Time")]
        public bool isStop;
        public float stopTime;

        [Header("3. Slow Motion")]
        public bool isSlowMotion;
        public float slowMotionFactor;
        public float slowMotionTime;

        [Header("4. Move To Hit Point")]
        public bool isSightToHitPoint;
        public float stayTime;
        public float duration;
    }

    [System.Serializable]
    public struct HitReactionData
    {
        public string Name;
        public EAttackDirection attackDirection;
        public HitTrace hitTrace;

        public SlashData slashData;
        public AttackExtendData attackExtendData;
        public HitImpact hitImpact;
        public HitFeedbackData hitFeedbackData;
    }
    [System.Serializable]
    public struct ListHitReactionData
    {
        public string Name;
        public List<HitReactionData> hitReactionDatas;
    }
    [System.Serializable]
    public struct ComboData
    {
        public List<EKeystroke> comboInputs;
        public List<AnimationClip> comboClips;
        public EStateType requiredState;
        public List<ListHitReactionData> listHitReactionDatas;

    }
    [CreateAssetMenu(menuName = "Combat System/Combat/Combo Data", fileName = "Combo Data")]
    public class SO_Combo : ScriptableObject
    {
        public ComboData comboData;
    }
}
