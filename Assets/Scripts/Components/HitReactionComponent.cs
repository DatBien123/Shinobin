using System;
using System.Collections;
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

        public EReactionType currentReactionType;
        public EHitReactionState currentHitReactionState;
        public ERecoverState currentRecoverState;
        public bool isOnStagger;

        [Header("Camera Reference")]
        public CameraManager cameraManager;
        //Hit impacts
        Coroutine C_Knockback;
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
        #region Stagger
        public void StartStagger()
        {
            if (C_Stagger != null) StopCoroutine(C_Stagger);
            C_Stagger = StartCoroutine(Stagger());
        }
        IEnumerator Stagger()
        {

            isOnStagger = true;

            owner.comboComponent.ResetCombo();

            owner.atributeComponent.hyperArmor = 300;

            //owner.lookAtComponent.LookAtEnemy(owner.targetingComponent.target.transform.position, .1f);

            owner.animator.CrossFadeInFixedTime(AnimationParams.Stagger_State, .1f);

            float elapsedTime = 0.0f;

            bool isFinisherTrigger = false;

            while(elapsedTime <= 100f)
            {
                if (owner.hitReactionComponent.currentComboDataTake.finisherData.isFinisher) isFinisherTrigger = true;
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            if (!isFinisherTrigger)
            {
                isOnStagger = false;
                owner.animator.CrossFadeInFixedTime(AnimationParams.Recover_Stagger_State, .1f, 0, 0);
            }


            owner.hitReactionComponent.ResetHitReaction();
            if (owner as CharacterAI)
            {
                (owner as CharacterAI).ResetBehaviorState();
            }

        }
        public void SetStaggetState(int isOn)
        {
            if (isOn == 0) isOnStagger = true;
            else isOnStagger = false;
        }
        #endregion

        //Funcs public
        public void PlayHitReactions(Vector3 knockBackDirection, Vector3 hitPoint, EReactionType reactionType)
        {
            currentReactionType = reactionType;
            if(owner.atributeComponent.currentHP <= 0)
            {
                owner.animator.CrossFadeInFixedTime(AnimationParams.Death_State, .1f);
                return;
            }
            switch (reactionType)
            {
                case EReactionType.Combo:
                    PlayHitFX(hitPoint, reactionType);
                    if (currentHitReactionDataTake.hitImpact.KnockbackData.isKnockback) PlayHitKnockBack(knockBackDirection, hitPoint, reactionType);
                    if (currentHitReactionDataTake.hitImpact.AirborneData.isAirborne) PlayHitAirborne(hitPoint, reactionType);
                    break;
                case EReactionType.Skill:
                    PlayHitFX(hitPoint, reactionType);
                    //if (currentSkillDataTake.hitImpact.KnockbackData.isKnockback) PlayHitKnockBack(knockBackDirection, hitPoint, reactionType);
                    //if (currentSkillDataTake.hitImpact.AirborneData.isAirborne) PlayHitAirborne(hitPoint, reactionType);
                    break;
            }

        }
        public void PlayHitFeedback(Vector3 hitPoint, EReactionType reactionType)
        {
            HitFeedbackData hitFeedbackData = new HitFeedbackData();
            if(reactionType == EReactionType.Combo)
            {
                hitFeedbackData = currentHitReactionDataTake.hitFeedbackData;
            }
            else if(reactionType == EReactionType.Skill)
            {
                //hitFeedbackData = currentSkillDataTake.hitFeedbackData;
            }
            switch (reactionType)
            {
                case EReactionType.Combo:
                    if (currentHitReactionDataTake.hitFeedbackData.isShake) PlayCameraShake(hitFeedbackData);
                    if (currentHitReactionDataTake.hitFeedbackData.isStop) PlayStopTime(hitFeedbackData);
                    if (currentHitReactionDataTake.hitFeedbackData.isSlowMotion) PlaySlowMotion(hitFeedbackData);
                    break;
                case EReactionType.Skill:
                    //if (currentSkillDataTake.hitFeedbackData.isShake) PlayCameraShake(hitFeedbackData);
                    //if (currentSkillDataTake.hitFeedbackData.isStop) PlayStopTime(hitFeedbackData);
                    //if (currentSkillDataTake.hitFeedbackData.isSlowMotion) PlaySlowMotion(hitFeedbackData);
                    break;
            }

        }
        #region HitReaction
        public void PlayHitKnockBack(Vector3 direction,Vector3 hitPoint, EReactionType reactionType)
        {
            if (C_Knockback != null)
            {
                StopCoroutine(C_Knockback);
            }
            C_Knockback = StartCoroutine(Knockback(direction,hitPoint, reactionType));
        }
        public void PlayHitAirborne(Vector3 hitPoint, EReactionType reactionType)
        {
            if (C_Airborne != null)
            {
                StopCoroutine(C_Airborne);
            }
            //C_Airborne = StartCoroutine(Airborne(hitPoint, reactionType));
        }

        //Coroutines
        //Hint: These funcs make owner get knockback/Airborne/Stun after get hit
        IEnumerator Knockback(Vector3 direction, Vector3 hitPoint, EReactionType reactionType)
        {
            HitImpact hitImpact = new HitImpact();
            if (reactionType == EReactionType.Combo)
            {
                hitImpact = currentHitReactionDataTake.hitImpact;
            }
            else if (reactionType == EReactionType.Skill)
            {
                //hitImpact = currentSkillDataTake.hitImpact;
            }

            //Start Knockback
            //owner.applyingKnockback = true;
            Vector3 knockbackVelocity = new Vector3(direction.x, 0, direction.z) * hitImpact.KnockbackData.knockbackDistance;
            float elapsedTime = 0;

            if (owner as CharacterAI)
            {
                (owner as CharacterAI).navMeshAgent.enabled = false;
            }

            //if (hitImpact.KnockbackData.hitKnockBackClip != null)
            //{
            //    if(owner.TYPE != "Boss")owner.animator.CrossFadeInFixedTime(hitImpact.KnockbackData.hitKnockBackClip.name, .1f);
            //    else
            //    {
            //        if (owner.targetingComponent.target.comboComponent.currentComboData.comboData.finisherData.isFinisher)
            //        {
            //            owner.animator.CrossFadeInFixedTime(hitImpact.KnockbackData.hitKnockBackClip.name, .1f);
            //        }
            //    }
            //}

            while (elapsedTime < hitImpact.KnockbackData.knockbackTime)
            {
                Debug.Log("Knockbadasdasdasads");
                Vector3 movement = new Vector3(knockbackVelocity.x, 0, knockbackVelocity.z) * Time.fixedDeltaTime;
                knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, elapsedTime / hitImpact.KnockbackData.knockbackTime);

                // Di chuyển nhân vật chỉ theo X và Z
                owner.characterController.Move(new Vector3(movement.x, 0, movement.z));

                elapsedTime += Time.fixedDeltaTime;
                yield return null;
            }

            //owner.applyingKnockback = false;
            //if (owner as CharacterAI)
            //{
            //    (owner as CharacterAI).navMeshAgent.enabled = true;
            //}
        }
        public void SetAirborneTime(float airborneTime)
        {
            if (C_AirborneTimeCoroutine != null)
            {
                StopCoroutine(C_AirborneTimeCoroutine);
            }
            C_AirborneTimeCoroutine = StartCoroutine(PlayAirborneTime(airborneTime));
        }
        IEnumerator PlayAirborneTime(float airborneTime)
        {
            yield return null;
            //owner.jumpComponent.isJumping = true;
            //Debug.Log("Airbone");
            //yield return new WaitForSeconds(airborneTime);
            //owner.jumpComponent.isJumping = false;
        }
        #endregion
        #region HitFeedBack
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
        public void PlayStopTime(float timeStop)
        {
            if (C_StopTime != null)
            {
                StopCoroutine(C_StopTime);
            }
            C_StopTime = StartCoroutine(StopTime(timeStop));
        }
        public void PlaySlowMotion(HitFeedbackData hitFeedbackData)
        {
            if (C_SlowMotion != null)
            {
                StopCoroutine(C_SlowMotion);
            }
            C_SlowMotion = StartCoroutine(SlowMotion(hitFeedbackData));
        }

        //Coroutines
        //Hint: These funcs give player feedback after everysingle hit combo
        IEnumerator CameraShake()
        {

            yield return null;
        }
        IEnumerator StopTime(float timeStop)
        {
            Time.timeScale = 0.0f;
            yield return new WaitForSecondsRealtime(timeStop);
            C_StopTime = null;
            Time.timeScale = 1.0f;
        }
        IEnumerator StopTime(HitFeedbackData hitFeedbackData)
        {
            Time.timeScale = 0.0f;
            yield return new WaitForSecondsRealtime(hitFeedbackData.stopTime);
            C_StopTime = null;
            Time.timeScale = 1.0f;
        }
        IEnumerator SlowMotion(HitFeedbackData hitFeedbackData)
        {
            yield return null;
        }
        #endregion
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
                AudioSource.PlayClipAtPoint(currentHitReactionData.slashData.slashSFX.slashAudioClip,
                    position,
                    currentHitReactionData.slashData.slashSFX.volume);
            }
        }
        //HitFX
        public void PlayHitFX(Vector3 hitPoint, EReactionType reactionType)
        {
            HitImpact hitImpact = new HitImpact();
            if (reactionType == EReactionType.Combo)
            {
                hitImpact = currentHitReactionDataTake.hitImpact;
            }
            else if (reactionType == EReactionType.Skill)
            {
                //hitImpact = currentSkillDataTake.hitImpact;
            }

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
            }
        }
        //BlockFX
        public void PlayBlockHitFX(Vector3 hitPoint, HitImpact hitImpact)
        {
            if (hitImpact.BlockData.blockFX.hitParryParticleSystem != null /*&& currentHitReactionDataTake.hitFX.hitSFX.hitAudioClip != null*/)
            {
                Debug.Log("Playing");
                Vector3 fxPosition = hitImpact.BlockData.blockFX.hitParryFXPositionOffset;
                Vector3 fxRotation = hitImpact.BlockData.blockFX.hitParryFXRotationOffset;
                Vector3 fxScale = hitImpact.BlockData.blockFX.hitParryFXScaleOffset;
                // Instantiate knockback particle effect
                ParticleSystem hitEffect = Instantiate(
                    hitImpact.BlockData.blockFX.hitParryParticleSystem,
                    hitPoint + fxPosition,
                    Quaternion.Euler(fxRotation)
                );
                hitEffect.transform.localScale = fxScale;
                hitEffect.Play();
                StartCoroutine(DestroyEffectWhenFinished(hitEffect));

                PlayBlockHitSFX(hitPoint, hitImpact);
            }

        }
        public void PlayBlockHitSFX(Vector3 position, HitImpact hitImpact)
        {
            if (hitImpact.BlockData.blockSFX.hitParryAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(hitImpact.BlockData.blockSFX.hitParryAudioClip,
                    position,
                   hitImpact.BlockData.blockSFX.volume);
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
        #region AdditiveFuncs
        IEnumerator DestroyEffectWhenFinished(ParticleSystem effect)
        {
            yield return new WaitUntil(() => !effect.IsAlive());

            // Xóa hiệu ứng khỏi cảnh
            Destroy(effect.gameObject);
        }
        public bool ísCollideDuringImpacted(HitImpact hitImpact)
        {
            ///////////////////////REACTION TYPE IS : COMBO /////////////////


            if (hitImpact.CollideHitData.isCollide)
            {
                if (hitImpact.CollideHitData.collideType == ECollideType.None)
                {
                    if (Physics.CheckSphere(owner.transform.position + owner.transform.up * 2, 0.05f, stoppingLayer))
                    {
                        return true;
                    }
                    else if (Physics.CheckSphere(owner.transform.position, 0.05f, stoppingLayer))
                    {
                        return true;
                    }
                }
                else if (hitImpact.CollideHitData.collideType == ECollideType.FloorToAir)
                {
                    if (Physics.CheckSphere(owner.transform.position + owner.transform.up * 2, 0.5f, stoppingLayer))
                    {
                        //if (currentHitReactionDataTake.hitImpact.isCollide)
                        //{
                        //    //Run animation Hit Top Wall in here
                        //    owner._animator.CrossFadeInFixedTime(currentHitReactionDataTake.hitImpact.collideClip.name, 0.1f);
                        //}
                        return true;
                    }
                }
                else if (hitImpact.CollideHitData.collideType == ECollideType.AirToFloor)
                {
                    if (Physics.CheckSphere(owner.transform.position, .6f, stoppingLayer))
                    {
                        if (hitImpact.CollideHitData.isCollide)
                        {
                            //Run animation Hit Top Wall in here
                            Debug.Log(hitImpact.CollideHitData.collideClip.name);
                            owner.animator.CrossFadeInFixedTime(hitImpact.CollideHitData.collideClip.name, 0.1f);
                            Vector3 placeCollideFXPosition = owner.transform.position;
                            placeCollideFXPosition.y = 0.0f;
                            PlayImpactCollide(placeCollideFXPosition, hitImpact);

                        }
                        return true;
                    }
                }
                else if (hitImpact.CollideHitData.collideType == ECollideType.OnFloor)
                {
                    if (Physics.CheckSphere(owner.transform.position + owner.transform.up, 0.5f, stoppingLayer))
                    {
                        if (hitImpact.CollideHitData.isCollide)
                        {
                            //Run animation Hit Side Wall in here
                            owner.animator.CrossFadeInFixedTime(hitImpact.CollideHitData.collideClip.name, 0.1f);
                        }
                        return true;
                    }
                }
            }
                return false;
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

        private void OnDrawGizmosSelected()
        {

        }
        #endregion

        
    }
}
