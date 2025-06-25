using System;
using System.Collections;
using System.Xml.Linq;
using UnityEngine;


namespace Training
{
    public enum EReactionType
    {
        Skill = 0,
        Combo
    }
    [System.Serializable]
    public enum EHitReactionState
    {
        None,
        OnHit,
        OffHit
    }
    [System.Serializable]
    public enum ERecoverState
    {
        None,
        OnRecover,
        OffRecover
    }
    public class HitReactionComponent : MonoBehaviour
    {

        [Header("Owner")]
        public Character owner;

        [Header("Stopping Layer")]
        public LayerMask stoppingLayer;

        [Header("Current HitReaction Data")]
        public HitReactionData currentHitReactionData;

        [Header("Current HitReaction Data Take")]
        public HitReactionData currentHitReactionDataTake;
        [Header("Current Combo Data Take")]
        public ComboData currentComboDataTake;

        [Header("Current List-HitReaction Data")]
        public ListHitReactionData currentListHitReactionData;
        [Header("Current HitReaction Data index")]
        public int currentHitReactionIndex = 0;


        public bool isOnHitReact = false;
        public bool isRecovering = false;

        //public EReactionType currentReactionType;
        public EHitReactionState currentHitReactionState;
        public ERecoverState currentRecoverState;

        [Header("Camera Reference")]
        public CameraManager cameraManager;
        //Hit impacts
        Coroutine C_Knockback;
        Coroutine C_Block;
        Coroutine C_Airborne;
        Coroutine C_Stagger;
        Coroutine C_AirborneOwner;
        Coroutine C_Freeze;
        Coroutine C_Silence;
        Coroutine C_AirborneTimeCoroutine;

        //Hit Feedback
        Coroutine C_CameraShake;
        Coroutine C_StopTime;
        Coroutine C_SlowMotion;
        Coroutine C_SightToHitPoint;

        public void ResetHitReaction()
        {
            currentHitReactionState = EHitReactionState.OffHit;
            currentRecoverState = ERecoverState.OffRecover;
        }
        private void Awake()
        {
            owner = GetComponent<Character>();
            cameraManager = FindObjectOfType<CameraManager>();
        }
        //Funcs public
        public void PlayHitReactions(Character causer, Character taker, Vector3 hitPoint)
        {
            PlayHitFeedback(hitPoint);

            if (currentHitReactionDataTake.hitImpact.BlockData.isBlockable && owner.isBlock)
            {
                PlayHitBlock(causer , taker,  hitPoint);
                return;
            }

            else if (currentHitReactionDataTake.hitImpact.KnockbackData.isKnockback) PlayHitKnockBack(causer, taker, hitPoint);


        }
        #region SlashFX/ HitFX: Knockback, Airborne, Stun, Silence, Freeze...(wait for extend)
        //SlashFX
        public void PlaySlashFX(Character causer)
        {
            if (currentHitReactionData.slashData.slashFX.slashParticleSystem != null || currentHitReactionData.slashData.slashSFX.slashAudioClip != null)
            {
                Vector3 fxPosition = causer.transform.position + causer.transform.TransformDirection(currentHitReactionData.slashData.slashFX.slashFXPositionOffset);
                Vector3 fxRotation = currentHitReactionData.slashData.slashFX.slashFXRotationOffset;
                Vector3 fxScale = currentHitReactionData.slashData.slashFX.slashFXScaleOffset;
                if (currentHitReactionData.slashData.slashFX.slashParticleSystem != null)
                {
                    // Instantiate knockback particle effect
                    ParticleSystem slashEffect = Instantiate(
                        currentHitReactionData.slashData.slashFX.slashParticleSystem,
                        fxPosition,
                        Quaternion.LookRotation(causer.transform.forward) * Quaternion.Euler(fxRotation)
                    );

                    slashEffect.transform.localScale = fxScale;

                    var mainModule = slashEffect.main;
                    mainModule.useUnscaledTime = true;

                    slashEffect.Play();
                    StartCoroutine(DestroyEffectWhenFinished(slashEffect));
                }

                PlaySlashSFX(fxPosition);

            }
        }
        public void PlaySlashSFX(Vector3 position)
        {
            if (currentHitReactionData.slashData.slashSFX.slashAudioClip != null)
            {
                // Thêm AudioSource vào GameObject này
                PlaySoundUnscaled(currentHitReactionData.slashData.slashSFX.slashAudioClip, position, currentHitReactionData.slashData.slashSFX.volume);
            }
        }
        //HitFX
        public void PlayHitFX(Vector3 hitPoint)
        {
            HitImpact hitImpact = new HitImpact();
            hitImpact = currentHitReactionDataTake.hitImpact;


            if (hitImpact.HitData.hitFX.hitParticleSystem != null /*&& currentHitReactionDataTake.hitFX.hitSFX.hitAudioClip != null*/)
            {
                Debug.Log("Playing");
                Vector3 fxPosition = hitImpact.HitData.hitFX.hitFXPositionOffset;
                Vector3 fxRotation = hitImpact.HitData.hitFX.hitFXRotationOffset;
                Vector3 fxScale = hitImpact.HitData.hitFX.hitFXScaleOffset;
                // Instantiate knockback particle effect
                ParticleSystem hitEffect = Instantiate(hitImpact.HitData.hitFX.hitParticleSystem);

                hitEffect.transform.SetParent(owner.transform);
                hitEffect.transform.localPosition = fxPosition;
                hitEffect.transform.localRotation = Quaternion.Euler(fxRotation);
                hitEffect.transform.localScale = fxScale;

                hitEffect.Play();
                StartCoroutine(DestroyEffectWhenFinished(hitEffect));

            }
            if (hitImpact.HitData.hitSFX.hitAudioClip != null)
            {
                PlayHitSFX(hitPoint, hitImpact);
            }

        }
        public void PlayHitSFX(Vector3 position, HitImpact hitImpact)
        {
            if (hitImpact.HitData.hitSFX.hitAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(hitImpact.HitData.hitSFX.hitAudioClip,
                    position,
                    hitImpact.HitData.hitSFX.volume);
                PlaySoundUnscaled(hitImpact.HitData.hitSFX.hitAudioClip, position, hitImpact.HitData.hitSFX.volume);
            }
        }
        //BlockFX
        public void PlayBlockReactFX(Vector3 hitPoint, HitImpact hitImpact, EBlockReactType blockReactType)
        {
            BlockFX blockFX = new BlockFX();
            switch (blockReactType)
            {
                case EBlockReactType.Defence:
                    blockFX = hitImpact.BlockData.defenceFX;
                    break;
                case EBlockReactType.Parry:
                    blockFX = hitImpact.BlockData.parryFX;
                    break;
                case EBlockReactType.Broken:
                    blockFX = hitImpact.BlockData.brokenFX;
                    break;
                case EBlockReactType.Rebound:
                    blockFX = hitImpact.BlockData.reboundFX;
                    break;
            }


            if (blockFX.blockReactParticleSystem != null /*&& currentHitReactionDataTake.hitFX.hitSFX.hitAudioClip != null*/)
            {
                Debug.Log("Playing");
                Vector3 fxPosition = blockFX.blockReactFXPositionOffset;
                Vector3 fxRotation = blockFX.blockReactFXRotationOffset;
                Vector3 fxScale = blockFX.blockReactFXScaleOffset;
                // Instantiate knockback particle effect
                ParticleSystem hitEffect = Instantiate(
                    blockFX.blockReactParticleSystem,
                    hitPoint + fxPosition,
                    Quaternion.Euler(fxRotation)
                );
                hitEffect.transform.localScale = fxScale;
                hitEffect.Play();
                StartCoroutine(DestroyEffectWhenFinished(hitEffect));

                PlayBlockHitSFX(hitPoint, hitImpact, blockReactType);
            }

        }
        public void PlayBlockHitSFX(Vector3 position, HitImpact hitImpact, EBlockReactType blockReactType)
        {
            BlockFX blockFX = new BlockFX();
            switch (blockReactType)
            {
                case EBlockReactType.Defence:
                    blockFX = hitImpact.BlockData.defenceFX;
                    break;
                case EBlockReactType.Parry:
                    blockFX = hitImpact.BlockData.parryFX;
                    break;
                case EBlockReactType.Broken:
                    blockFX = hitImpact.BlockData.brokenFX;
                    break;
                case EBlockReactType.Rebound:
                    blockFX = hitImpact.BlockData.reboundFX;
                    break;
            }

            if (blockFX.blockReactAudioClip != null)
            {
                PlaySoundUnscaled(blockFX.blockReactAudioClip, position, blockFX.volume);
            }
        }
        public void PlayImpactCollide(Vector3 position, HitImpact hitImpact)
        {
                if (hitImpact.CollideHitData.colliderHitFX.CollideParticleSystem != null)
                {
                    Vector3 fxPosition = hitImpact.CollideHitData.colliderHitFX.CollideFXPositionOffset;
                    Vector3 fxRotation = hitImpact.CollideHitData.colliderHitFX.CollideFXRotationOffset;
                    Vector3 fxScale = hitImpact.CollideHitData.colliderHitFX.CollideFXScaleOffset;
                    // Instantiate knockback particle effect
                    GameObject hitEffect = Instantiate(
                        hitImpact.CollideHitData.colliderHitFX.CollideParticleSystem,
                        fxPosition + position,
                        Quaternion.Euler(fxRotation)
                    );
                    hitEffect.transform.localScale = fxScale;

                }
        }
        #endregion

        #region [AdditiveFuncs]
        void PlaySoundUnscaled(AudioClip clip, Vector3 position, float volume = 1f)
        {
            if (clip == null) return;

            GameObject audioObj = new GameObject("TempAudio");
            audioObj.transform.position = position;

            AudioSource audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.spatialBlend = 1f; // 3D sound
            audioSource.playOnAwake = false;

            // 👇 Quan trọng: Phát âm thanh khi timeScale = 0
            audioSource.ignoreListenerPause = true;

            audioSource.Play();

            // Hủy sau khi clip kết thúc
            Destroy(audioObj, clip.length);
        }
        IEnumerator DestroyEffectWhenFinished(ParticleSystem effect)
        {
            yield return new WaitUntil(() => !effect.IsAlive());

            // Xóa hiệu ứng khỏi cảnh
            Destroy(effect.gameObject);
        }
        public Vector3 GetShakeDirection(EShakeDirection shakeDirection)
        {
            switch (shakeDirection)
            {
                case EShakeDirection.Left:
                    return new Vector3(-1, 0, 0);
                case EShakeDirection.Right:
                    return new Vector3(1, 0, 0);
                case EShakeDirection.Up:
                    return new Vector3(0, 1, 0);
                case EShakeDirection.Down:
                    return new Vector3(0, -1, 0);
                case EShakeDirection.Front:
                    return new Vector3(0, 0, 1);
                case EShakeDirection.Back:
                    return new Vector3(0, 0, -1);
                default:
                    return new Vector3(0, 0, 0);
            }
        }
        #endregion

        #region [KnockBackReact]
        public void PlayHitKnockBack(Character causer, Character taker, Vector3 hitPoint)
        {
            if (C_Knockback != null)
            {
                StopCoroutine(C_Knockback);
            }
            C_Knockback = StartCoroutine(Knockback(causer, taker, hitPoint));
        }
        IEnumerator Knockback(Character causer, Character taker, Vector3 hitPoint)
        {
            HitImpact hitImpact = new HitImpact();
            hitImpact = currentHitReactionDataTake.hitImpact;


            Vector3 direction = taker.gameObject.transform.position - causer.gameObject.transform.position;
            direction.Normalize();

            //Start Knockback
            taker.isApplyingKnockBack = true;
            Vector3 knockbackVelocity = new Vector3(direction.x, 0, direction.z) * hitImpact.KnockbackData.knockbackDistance;
            float elapsedTime = 0;

            if (hitImpact.KnockbackData.reactData.isRotateToHitDirection)
            {
                transform.LookAt(transform.position + -direction);
            }
            AnimationClip clip = GetValidAnimationClip(GetValidHitReaction(direction));
            if (clip != null) {
                taker.animator.CrossFadeInFixedTime(clip.name, .1f);
            }
            PlayHitFX(hitPoint);



            while (elapsedTime < hitImpact.KnockbackData.knockbackTime)
            {
                //Debug.Log("Knockbadasdasdasads");
                Vector3 movement = new Vector3(knockbackVelocity.x, 0, knockbackVelocity.z) * Time.fixedDeltaTime;
                knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, elapsedTime / hitImpact.KnockbackData.knockbackTime);

                // Di chuyển nhân vật chỉ theo X và Z
                taker.characterController.Move(new Vector3(movement.x, 0, movement.z));

                elapsedTime += Time.fixedDeltaTime;
                yield return null;
            }
            taker.isApplyingKnockBack = false;
        }
        AnimationClip GetValidAnimationClip(EHitReactDirection hitReactDirection)
        {
            if(currentHitReactionDataTake.hitImpact.KnockbackData.reactData.animationReactDatas.Count > 0)
            {
                foreach(var animationReactData in currentHitReactionDataTake.hitImpact.KnockbackData.reactData.animationReactDatas)
                {
                    if(animationReactData.hitReactDirection == hitReactDirection && animationReactData.hitReactClip != null)
                    {
                        return animationReactData.hitReactClip;
                    }
                }
            }
            return null;
        }
        public EHitReactDirection GetValidHitReaction(Vector3 direction)
        {
            EHitReactDirection hitReactionDirectionResult = new EHitReactDirection();
            Vector3 directionToCauser = -direction;
            Vector3 localDir = gameObject.transform.InverseTransformDirection(directionToCauser);

            hitReactionDirectionResult = GetDirection(localDir);
            Debug.Log("Hit Reaction Direction is: " + hitReactionDirectionResult.ToString());
            return hitReactionDirectionResult;
        }
        EHitReactDirection GetDirection(Vector3 localDir)
        {
            float angle = Mathf.Atan2(localDir.x, localDir.z) * Mathf.Rad2Deg;
            angle = (angle + 360f) % 360f;

            if (angle >= 337.5f || angle < 22.5f) return EHitReactDirection.F;
            if (angle >= 22.5f && angle < 67.5f) return EHitReactDirection.FR;
            if (angle >= 67.5f && angle < 112.5f) return EHitReactDirection.R;
            if (angle >= 112.5f && angle < 157.5f) return EHitReactDirection.BR;
            if (angle >= 157.5f && angle < 202.5f) return EHitReactDirection.B;
            if (angle >= 202.5f && angle < 247.5f) return EHitReactDirection.BL;
            if (angle >= 247.5f && angle < 292.5f) return EHitReactDirection.L;
            if (angle >= 292.5f && angle < 337.5f) return EHitReactDirection.FL;

            return EHitReactDirection.F;
        }
        #endregion

        #region [BlockReact]
        public enum EBlockReactType
        {
            Defence,
            Parry,
            Broken,
            Rebound
        }
        public void PlayHitBlock(Character causer, Character taker, Vector3 hitPoint)
        {
            if (C_Block != null)
            {
                StopCoroutine(C_Block);
            }
            C_Block = StartCoroutine(Block(causer, taker,  hitPoint));
        }
        IEnumerator Block(Character causer, Character taker, Vector3 hitPoint)
        {
            HitImpact hitImpact = new HitImpact();
            hitImpact = currentHitReactionDataTake.hitImpact;

            Vector3 direction = taker.gameObject.transform.position - causer.gameObject.transform.position;
            direction.Normalize();
            //Start Knockback
            taker.isApplyingKnockBack = true;
            Vector3 knockbackVelocity = new Vector3(direction.x, 0, direction.z) * hitImpact.BlockData.knockbackDistance;
            float elapsedTime = 0;

            transform.LookAt(transform.position + -direction);

            switch (GetValidBlockReactType(causer, taker, hitImpact))
            {
                case EBlockReactType.Defence:
                    if (hitImpact.BlockData.defenceReactClip != null) taker.animator.CrossFadeInFixedTime(hitImpact.BlockData.defenceReactClip.name, .1f);
                    PlayBlockReactFX(hitPoint, hitImpact, EBlockReactType.Defence);
                    break;
                case EBlockReactType.Parry:
                    if (hitImpact.BlockData.parryReactClip != null) taker.animator.CrossFadeInFixedTime(hitImpact.BlockData.parryReactClip.name, .1f);
                    PlayBlockReactFX(hitPoint, hitImpact, EBlockReactType.Parry);
                    break;
                case EBlockReactType.Broken:
                    if (hitImpact.BlockData.brokenReactClip != null) taker.animator.CrossFadeInFixedTime(hitImpact.BlockData.brokenReactClip.name, .1f);
                    PlayBlockReactFX(hitPoint, hitImpact, EBlockReactType.Broken);
                    break;
                //case EBlockReactType.Rebound:
                //    if (hitImpact.BlockData.reboundReactClip != null) taker.animator.CrossFadeInFixedTime(hitImpact.BlockData.reboundReactClip.name, .1f);
                //    break;
                default:
                    break;
            }

            while (elapsedTime < hitImpact.BlockData.knockbackTime)
            {
                //Debug.Log("Knockbadasdasdasads");
                Vector3 movement = new Vector3(knockbackVelocity.x, 0, knockbackVelocity.z) * Time.fixedDeltaTime;
                knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, elapsedTime / hitImpact.BlockData.knockbackTime);

                // Di chuyển nhân vật chỉ theo X và Z
                taker.characterController.Move(new Vector3(movement.x, 0, movement.z));

                elapsedTime += Time.fixedDeltaTime;
                yield return null;
            }

            taker.isApplyingKnockBack = false;
        }
        EBlockReactType GetValidBlockReactType(Character causer, Character taker, HitImpact hitImpact)
        {
            if(owner as CharacterPlayer)
            {
                //Parry
                if(taker.GetComponent<InputManager>().blockTime <= 0.3f)
                {
                    causer.atributeComponent.currentPosture += 200;
                    //Nếu parry thành công mà đối phương lại có posture max thì đối phương sẽ bị rebound
                    if (causer.atributeComponent.currentPosture >= causer.atributeComponent.maxPosture)
                    {
                        if (hitImpact.BlockData.reboundReactClip != null) causer.animator.CrossFadeInFixedTime(hitImpact.BlockData.reboundReactClip.name, .1f);

                        causer.atributeComponent.currentPosture = 0;
                        causer.comboComponent.ResetCombo();
                    }
                    return EBlockReactType.Parry;

                }
            }

            //Đỡ thông thường 
            if (taker.atributeComponent.currentPosture < taker.atributeComponent.maxPosture)
            {
                taker.atributeComponent.currentPosture += 200;
                return EBlockReactType.Defence;
            }
            else
            {
                taker.isBlock = false;
                taker.animator.SetBool(AnimationParams.Block_Param, taker.isBlock);
                taker.atributeComponent.currentPosture = 0;
                return EBlockReactType.Broken;
            }

        }
        #endregion


        #region [HitFeedBack]
        public void PlayHitFeedback(Vector3 hitPoint)
        {
            HitFeedbackData hitFeedbackData = new HitFeedbackData();
            hitFeedbackData = currentHitReactionDataTake.hitFeedbackData;
            if (currentHitReactionDataTake.hitFeedbackData.isShake) PlayCameraShake(hitFeedbackData);
            if (currentHitReactionDataTake.hitFeedbackData.isStop) PlayStopTime(hitFeedbackData);
            if (currentHitReactionDataTake.hitFeedbackData.isSlowMotion) PlaySlowMotion(hitFeedbackData);
        }
        public void PlayCameraShake(HitFeedbackData hitFeedbackData)
        {
            Vector3 shakeDirectionResult = Vector3.zero;
            switch (hitFeedbackData.shakeDirection)
            {
                case EShakeDirection.Up:
                    shakeDirectionResult = Vector3.up;
                    break;
                case EShakeDirection.Down:
                    shakeDirectionResult = Vector3.down;
                    break;
                case EShakeDirection.Left:
                    shakeDirectionResult = Vector3.left;
                    break;
                case EShakeDirection.Right:
                    shakeDirectionResult = Vector3.right;
                    break;
                case EShakeDirection.Front:
                    shakeDirectionResult = Vector3.forward;
                    break;
                case EShakeDirection.Back:
                    shakeDirectionResult = Vector3.back;
                    break;

            }
            cameraManager.Shake(shakeDirectionResult, hitFeedbackData.intensity);
        }
        public void PlayStopTime(HitFeedbackData hitFeedbackData)
        {
            if (C_StopTime != null)
            {
                StopCoroutine(C_StopTime);
            }
            C_StopTime = StartCoroutine(StopTime(hitFeedbackData));
        }
        IEnumerator StopTime(HitFeedbackData hitFeedbackData)
        {
            Time.timeScale = 0.0f;
            yield return new WaitForSecondsRealtime(hitFeedbackData.stopTime);
            C_StopTime = null;
            Time.timeScale = 1.0f;
        }
        public void PlaySlowMotion(HitFeedbackData hitFeedbackData)
        {
            if (C_SlowMotion != null)
            {
                StopCoroutine(C_SlowMotion);
            }
            C_SlowMotion = StartCoroutine(SlowMotion(hitFeedbackData));
        }
        IEnumerator SlowMotion(HitFeedbackData hitFeedbackData)
        {
            yield return null;
        }
        #endregion


    }
}
