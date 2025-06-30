using System.Collections;
using System.Linq;
using Training;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Windows;
using static UnityEngine.UI.GridLayoutGroup;

public class CharacterAI : Character
{
    #region Old
    [Header("Component")]
    public BehaviorDecesion behavDecesion;
    public bool isDoingCombo = false;
    public EBehaviorState currentBehaviorState = EBehaviorState.None;

    [Header("Move Stuffs")]
    public float moveToTargetStoppingDistance = 1.0f;
    [Header("With Target")]
    public Vector3 lastTargetPosition = Vector3.zero;
    [Header("Move Starfe")]
    public Transform moveStrafeCheck;
    public LayerMask stoppingStrafeWalkLayer;
    //Coroutines
    Coroutine C_MoveRandom;
    Coroutine C_Intro;
    Coroutine C_MoveToTarget;
    Coroutine C_MoveStrafeToTarget;
    Coroutine C_Turn;
    #region Move
    public void StartMoveRandom()
    {
        if (C_MoveRandom != null)
        {
            StopCoroutine(C_MoveRandom);
        }
        C_MoveRandom = StartCoroutine(MoveRandom());
    }
    IEnumerator MoveRandom()
    {
        yield return null;
    }
    public void StartMoveToTarget(EMoveType moveType, float duration)
    {
        if (C_MoveToTarget != null)
        {
            StopCoroutine(C_MoveToTarget);
        }
        C_MoveToTarget = StartCoroutine(MoveToTarget(moveType, duration));
    }
    IEnumerator MoveToTarget(EMoveType moveType, float duration)
    {
        //Assign Factor Speed
        float speedBlendFactor = 0.0f;
        switch (moveType)
        {
            //case EMoveType.Walk:
            //    speedBlendFactor = 1.0f;
            //    //speed = 5.0f;
            //    break;
            //case EMoveType.Sprint:
            //    speedBlendFactor = 2.0f;
            //    //speed = 9.0f;
            //    break;
            //case EMoveType.Fast:
            //    speedBlendFactor = 3.0f;
            //    //speed = 7.0f;
            //    break;
        }
        //Start Move

        //Animation Blend
        animator.SetFloat(AnimationParams.Speed_Param, speedBlendFactor);
        animator.SetFloat(AnimationParams.Input_Horizontal_Param, 0.0f);
        animator.SetFloat(AnimationParams.Input_Vertical_Param, 1.0f);
        animator.CrossFadeInFixedTime(AnimationParams.Locomotion_State, .15f, 0, 0.1f);

        //if (navMeshAgent.enabled)
        //    //navMeshAgent.speed = speed;

        //    if (navMeshAgent.enabled) navMeshAgent.SetDestination(targetingComponent.target.transform.position); // Cập nhật điểm đến mới

        //float distanceToTarget = Vector3.Distance(navMeshAgent.transform.position, targetingComponent.target.transform.position);

        //float elapsedTime = 0.0f;
        //while (distanceToTarget >= navMeshAgent.stoppingDistance)
        //{
        //    if (targetingComponent.target == null) yield break;

        //    // Kiểm tra nếu mục tiêu đã di chuyển
        //    if (Vector3.Distance(targetingComponent.target.transform.position, lastTargetPosition) > 0.1f)
        //    {
        //        lastTargetPosition = targetingComponent.target.transform.position;
        //        if (navMeshAgent.enabled) navMeshAgent.SetDestination(targetingComponent.target.transform.position); // Cập nhật điểm đến mới
        //    }

        //    //Update Position
        //    distanceToTarget = Utilities.Instance.DistanceCalculate(navMeshAgent.transform.position, targetingComponent.target.transform.position, true);

        //    elapsedTime += Time.deltaTime;

        //    //Break Lines
        //    if (elapsedTime > duration
        //        || hitReactionComponent.currentHitReactionState == EHitReactionState.OnHit
        //        || distanceToTarget <= moveToTargetStoppingDistance) break;
        yield return null;
        //}

        //End Move
        currentBehaviorState = EBehaviorState.Finished;
        //speed = 0.0f;
        //Animation Blend
        animator.SetFloat(AnimationParams.Speed_Param, 0.0f);
        animator.SetFloat(AnimationParams.Input_Horizontal_Param, 0.0f);
        animator.SetFloat(AnimationParams.Input_Vertical_Param, 0.0f);
        //if (navMeshAgent.enabled) navMeshAgent.ResetPath();


    }
    public void StartMoveStrafeToTarget(float duration)
    {
        if (C_MoveStrafeToTarget != null)
        {
            StopCoroutine(C_MoveStrafeToTarget);
        }
        C_MoveStrafeToTarget = StartCoroutine(MoveStrafeToTarget(duration));
    }
    IEnumerator MoveStrafeToTarget(float duration)
    {
        //Vector3 targetStrafePosition = Vector3.zero;
        ////Animation Blend
        //animator.SetFloat(AnimationParams.Speed_Param, 1.0f);
        ////Start Move
        //animator.CrossFadeInFixedTime(AnimationParams.Locomotion_State, .15f, 0, .1f);

        ////speed = 3.5f;
        //if (navMeshAgent.enabled)
        //{
        //    navMeshAgent.isStopped = false;           // Dừng NavMeshAgent
        //    //navMeshAgent.speed = speed;
        //    navMeshAgent.updateRotation = false;
        //}
        ////Side detection
        //float strafeSide = StrafeSide();
        //if (strafeSide > 0)
        //{
        //    animator.SetFloat(AnimationParams.Input_Horizontal_Param, 1.0f);
        //    animator.SetFloat(AnimationParams.Input_Vertical_Param, 0);
        //}
        //else
        //{
        //    animator.SetFloat(AnimationParams.Input_Horizontal_Param, -1.0f);
        //    animator.SetFloat(AnimationParams.Input_Vertical_Param, 0);
        //}
        ////Params Coroutine
        //float distanceToTarget = Vector3.Distance(navMeshAgent.transform.position, targetingComponent.target.transform.position);
        //float elapsedTime = 0.0f;
        //float elapsedTime1 = 0.0f;

        //while (elapsedTime <= duration )
        //{
        //    if (targetingComponent.target == null) yield break;

        //    // Kiểm tra nếu mục tiêu đã di chuyển
        //    //Update Rotation
        //    Vector3 direction = targetingComponent.target.transform.position - transform.position;
        //    Quaternion targetRotation = Quaternion.LookRotation(direction);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f);

        //    //Update Position
        //    Vector3 destinationPosition = strafeSide > 0 ? (transform.position + transform.right * 10.0f) : (transform.position + -transform.right * 10.0f);
        //    if(navMeshAgent.enabled)navMeshAgent.SetDestination(destinationPosition);

        //    //Update Distance
        //    distanceToTarget = Vector3.Distance(navMeshAgent.transform.position, targetingComponent.target.transform.position);
        //    elapsedTime += Time.fixedDeltaTime;
        //    elapsedTime1 += Time.fixedDeltaTime;

        //    //Break Lines
        //    Collider[] hit = Physics.OverlapSphere(transform.position, 2.0f, stoppingStrafeWalkLayer);
        //    if (distanceToTarget <= 2.0f 
        //        || hitReactionComponent.currentHitReactionState == EHitReactionState.OnHit
        //        || targetingComponent.target.currentCombatState == ECombatState.Attacking
        //        /*|| hit != null*/)
        //    {
        //        break;
        //    }
        //    yield return null;
        //}

        //// Reset NavMeshAgent
        //if (navMeshAgent.enabled)
        //{
        //    navMeshAgent.isStopped = true;           // Dừng NavMeshAgent
        //    navMeshAgent.velocity = Vector3.zero;   // Đặt vận tốc về 0
        //    navMeshAgent.ResetPath();               // Xóa đường đi
        //    navMeshAgent.updateRotation = true;     // Bật lại updateRotation nếu cần
        //}

        ////// Reset Animation
        ////animator.SetFloat(AnimationParams.Speed_Param, 0.0f);

        ////float startSpeed = animator.GetFloat(AnimationParams.Speed_Param);
        ////float elapsedTimeSmooth = 0.0f;
        ////float smoothDuration = 1.0f; // Thời gian làm mượt

        ////while (elapsedTimeSmooth < smoothDuration && distanceToTarget >2.0f)
        ////{
        ////    elapsedTimeSmooth += Time.deltaTime;
        ////    float smoothSpeed = Mathf.Lerp(startSpeed, 0.0f, elapsedTimeSmooth / smoothDuration);
        ////    animator.SetFloat(AnimationParams.Speed_Param, smoothSpeed);
        ////    yield return null;
        ////}

        //animator.SetFloat(AnimationParams.Speed_Param, 0.0f); // Đảm bảo chắc chắn là về đúng 0 sau khi làm mượt



        //// Đặt trạng thái hành vi hiện tại
        //currentBehaviorState = EBehaviorState.Finished;

        yield return null;
    }
    public float StrafeSide()
    {
        // Hướng từ nhân vật đến đối tượng
        Vector3 directionToTarget = (targetingComponent.target.transform.position - transform.position).normalized;

        // Tích vô hướng giữa hướng ngang (transform.right) và directionToTarget
        float dotProduct = Vector3.Dot(transform.right, directionToTarget);

        // Kiểm tra vị trí
        if (dotProduct > 0)
        {
            Debug.Log("Target is on the right side.");
        }
        else if (dotProduct < 0)
        {
            Debug.Log("Target is on the left side.");
        }
        else
        {
            Debug.Log("Target is directly in front or behind.");
        }
        return dotProduct;
    }
    #endregion
    #region Intro
    public void StartExecuteIntro()
    {
        if (C_Intro != null) StopCoroutine(C_Intro);
        C_Intro = StartCoroutine(PlayIntro());
    }
    IEnumerator PlayIntro()
    {

        float elapsedTime = 0.0f;

        animator.CrossFadeInFixedTime(AnimationParams.Sitting_State, .1f);
        while (targetingComponent.target == null)
        {
            Debug.Log("Playing Intro");
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        animator.CrossFadeInFixedTime(AnimationParams.Sit_To_Stand_State, .1f);

        yield return new WaitForSeconds(3.0f);

        currentBehaviorState = EBehaviorState.Finished;
    }
    #endregion

    #region TurnAI
    public void StartTurnToTarget(float duration)
    {
        if (C_Turn != null) StopCoroutine(C_Turn);
        C_Turn = StartCoroutine(TurnToTarget(duration));
    }
    IEnumerator TurnToTarget(float duration)
    {
        SpecifyTurnAngle();
        currentBehaviorState = EBehaviorState.Executing;
        //freeflowComponent.LookAtEnemy(targetingComponent.target.transform.position, duration);
        yield return new WaitForSeconds(1.5f);
        currentBehaviorState = EBehaviorState.Finished;
    }
    public Vector2 SpecifyTurnAngle()
    {
        Vector2 result = Vector2.zero;

        Vector3 characterForward = transform.forward;
        Vector3 directionToTarget = (targetingComponent.target.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(characterForward, directionToTarget);
        Debug.Log("Góc giữa forward của nhân vật và mục tiêu: " + angle);

        Vector3 right = transform.right; // Hướng phải của nhân vật
        Vector3 cross = Vector3.Cross(characterForward, directionToTarget);


        if (angle <= 90)
        {
            if (cross.y > 0)
            {
                Debug.Log("Mục tiêu nằm bên phai của nhân vật");
                result = new Vector2(1, 0);
            }
            else
            {
                Debug.Log("Mục tiêu nằm bên trái của nhân vật");
                result = new Vector2(-1, 0);
            }
        }
        else
        {
            result = new Vector2(0, -1);
        }

        //animator.SetFloat(AnimationParams.Input_Horizontal_Turn_Param, result.x);
        //animator.SetFloat(AnimationParams.Input_Vertical_Turn_Param, result.y);

        ////Speed 
        //if (navMeshAgent.speed == 0.0f)
        //{
        //    animator.SetFloat(AnimationParams.Speed_Param, 0.0f);
        //}
        //else if (navMeshAgent.speed == 1.0f)
        //{
        //    animator.SetFloat(AnimationParams.Speed_Param, 1.0f);
        //}
        //else if (navMeshAgent.speed == 2.0f)
        //{
        //    animator.SetFloat(AnimationParams.Speed_Param, 2.0f);
        //}
        //else if (navMeshAgent.speed == 3.0f)
        //{
        //    animator.SetFloat(AnimationParams.Speed_Param, 3.0f);
        //}

        animator.CrossFadeInFixedTime(AnimationParams.Turn_State, .1f);
        return result;
    }
    #endregion
    public void ResetBehaviorState()
    {
        currentBehaviorState = EBehaviorState.Finished;
    }

    #endregion


    public AIDecision aIDecision;

    protected override void Awake()
    {
        behavDecesion = GetComponent<BehaviorDecesion>();
        aIDecision = GetComponent<AIDecision>();
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        aIDecision = GetComponent<AIDecision>();

        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }
    protected override void Update()
    {
        base.Update();
        JumpAndGravity();
        GroundedCheck();

        //Chỉ di chuyển/xoay khi ko bị đánh !
        if (!isApplyingKnockBack
            && comboComponent.currentComboState != EComboState.Playing
            && hitReactionComponent.currentHitReactionState != EHitReactionState.OnHit
            && dodgeComponent.currentDodgeState != EDodgeState.OnDodge) Move();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void OnAnimatorMove()
    {
        base.OnAnimatorMove();
    }
    private void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = aIDecision.sprint ?
            (isBlock ? BlockSprintSpeed : SprintSpeed) : MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (aIDecision.desiredMoveDirection == Vector3.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f; 

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (aIDecision.desiredMoveDirection != Vector3.zero)
        {
            _targetRotation = Mathf.Atan2(aIDecision.desiredMoveDirection.x, aIDecision.desiredMoveDirection.z) * Mathf.Rad2Deg;
            if (aIDecision.currentMoveType == EMoveType.StrafeMove)
            {
                Vector3 directionToTarget = targetingComponent.target.transform.position - transform.position;
                _targetRotation = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
            }
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        if (aIDecision.currentMoveType == EMoveType.StrafeMove)
        {
            targetDirection = transform.right * aIDecision.move.x;
            targetDirection.Normalize();
        }
        // move the player
        characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator if using character

        _inputVerticalParam = Mathf.Lerp(_inputVerticalParam, aIDecision.move.y, SpeedChangeRate * Time.deltaTime);
        _inputHorizontalParam = Mathf.Lerp(_inputHorizontalParam, aIDecision.move.x, SpeedChangeRate * Time.deltaTime);

        animator.SetFloat(AnimationParams.Input_Horizontal_Param, _inputHorizontalParam);
        animator.SetFloat(AnimationParams.Input_Vertical_Param, _inputVerticalParam);
        animator.SetFloat(AnimationParams.Speed_Param, _animationBlend);
        animator.SetFloat(AnimationParams.Motion_Speed_Param, inputMagnitude);

    }
    private void JumpAndGravity()
    {
        if (Grounded)
        {
            currentState = EStateType.Grounded;
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // update animator if using character
            animator.SetBool(AnimationParams.Jump_Param, false);
            animator.SetBool(AnimationParams.FreeFall_Param, false);


            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump////////////////////////////////
            if (aIDecision.jump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                // update animator if using character
                animator.SetBool(AnimationParams.Jump_Param, true);
            }
            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            currentState = EStateType.InAir;
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // update animator if using character
                animator.SetBool(AnimationParams.FreeFall_Param, true);
            }

            // if we are not grounded, do not jump
            aIDecision.jump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }


}
