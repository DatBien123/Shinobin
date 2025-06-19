//using UnityEngine;
//public class Run : StateMachineBase
//{
//    public Run(StateMachineController stateMachineController) : base(stateMachineController) { }
//    public override void Enter()
//    {
//        //Debug.Log("Enter Run State");
//        //Init all move props
//        stateMachineController.playerMovement.desiredSpeed = stateMachineController.playerMovement.sprintSpeed;
//        stateMachineController.playerMovement.desiredSpeedBlend = 2.0f;

//    }
//    public override void Update()
//    {
//        Debug.Log("On Run State");
//        HandleSpeed();
//        HandleAnimation();
//        //Transition To Another States

//        //Idle
//        if (stateMachineController.playerMovement.inputDirection == Vector2.zero)
//        {
//            stateMachineController.SwitchState(stateMachineController.idleState);
//        }
//        //Walk
//        else if (stateMachineController.playerMovement.inputDirection != Vector2.zero && !stateMachineController.playerMovement.isSprint)
//        {
//            stateMachineController.SwitchState(stateMachineController.walkState);
//        }

//        ////Transition to Run Fast
//        //if (stateMachineController.playerMovement.isSprint)
//        //{
//        //    stateMachineController.playerMovement.totalSprintTime += Time.deltaTime;
//        //}
//        //else
//        //{
//        //    stateMachineController.playerMovement.totalSprintTime = 0.0f;
//        //}
//        //if (stateMachineController.playerMovement.totalSprintTime > 2.5f)
//        //{
//        //    stateMachineController.playerMovement.desiredSpeedBlend = 3.0f;
//        //}
//    }
//    public override void FixedUpdate()
//    {
//        HandleRotation();
//        HandleMovement();
//    }
//    public override void Exit()
//    {

//    }
//    public override void HandleAnimation()
//    {
//        stateMachineController.playerMovement.lastInputDirection.x = stateMachineController.playerMovement.animator.GetFloat(AnimationParams.Input_Horizontal_Param);
//        stateMachineController.playerMovement.lastInputDirection.y = stateMachineController.playerMovement.animator.GetFloat(AnimationParams.Input_Vertical_Param);

//        //HandleTurnWithCameraRotationDiff();
//        HandleSmoothParams();
//        //Speed Blend
//        stateMachineController.playerMovement.speedBlend = Mathf.Lerp(stateMachineController.playerMovement.speedBlend,
//            stateMachineController.playerMovement.desiredSpeedBlend, Time.deltaTime * stateMachineController.playerMovement.speedBlendFactor);
//        if (stateMachineController.playerMovement.speedBlend > 0.1f) stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Speed_Param, stateMachineController.playerMovement.speedBlend);

//        //Assign
//        stateMachineController.playerMovement.lastDesiredSpeedBlend = stateMachineController.playerMovement.desiredSpeedBlend;
//        stateMachineController.playerMovement.lastInputDirection = stateMachineController.playerMovement.inputDirection;
//    }
//    public override void HandleRotation()
//    {
//        if (stateMachineController.playerMovement.cameraManager.currentCameraType == ECameraType.FollowCamera)
//        {
//            //Rotation Normal
//            if (stateMachineController.playerMovement.temporaryLocomotion.data.followCameraData.followMovementData.sprintRotateType == Training.ERotateType.ToMoveDirection)
//            {
//                stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(stateMachineController.playerMovement.moveDirection);
//            }
//            else if (stateMachineController.playerMovement.temporaryLocomotion.data.followCameraData.followMovementData.sprintRotateType == Training.ERotateType.ToTarget)
//            {
//                Vector3 targetDirection = stateMachineController.playerMovement.targetingComponent.target.transform.position - stateMachineController.playerMovement.transform.position;
//                targetDirection.Normalize();
//                targetDirection.y = 0.0f;
//                stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(targetDirection);
//            }
//        }
//        else if (stateMachineController.playerMovement.cameraManager.currentCameraType == ECameraType.LockOnCamera)
//        {
//            //Rotation Normal
//            if (stateMachineController.playerMovement.temporaryLocomotion.data.lockOnCameraData.lockOnMovementData.sprintRotateType == Training.ERotateType.ToMoveDirection)
//            {
//                stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(stateMachineController.playerMovement.moveDirection);
//            }
//            else if (stateMachineController.playerMovement.temporaryLocomotion.data.lockOnCameraData.lockOnMovementData.sprintRotateType == Training.ERotateType.ToTarget)
//            {
//                Vector3 targetDirection = stateMachineController.playerMovement.targetingComponent.target.transform.position - stateMachineController.playerMovement.transform.position;
//                targetDirection.Normalize();
//                targetDirection.y = 0.0f;
//                stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(targetDirection);
//            }
//        }

//        stateMachineController.playerMovement.transform.rotation = Quaternion.Slerp(stateMachineController.playerMovement.transform.rotation,
//            stateMachineController.playerMovement.targetRotation, Time.fixedDeltaTime * stateMachineController.playerMovement.rotationFactor);
//    }
//    public override void HandleSmoothParams()
//    {
//        //Normal Move
//        float smoothInputDirectionX = Mathf.Lerp(stateMachineController.playerMovement.lastInputDirection.x,
//            stateMachineController.playerMovement.inputDirection.x, stateMachineController.playerMovement.speedBlendFactor * Time.deltaTime);
//        float smoothInputDirectionY = Mathf.Lerp(stateMachineController.playerMovement.lastInputDirection.y,
//            stateMachineController.playerMovement.inputDirection.y, stateMachineController.playerMovement.speedBlendFactor * Time.deltaTime);

//        if (stateMachineController.playerMovement.cameraManager.currentCameraType == ECameraType.FollowCamera)
//        {
//            //Animation Params Handle : FollowCam
//            if (stateMachineController.playerMovement.temporaryLocomotion.data.followCameraData.followAnimationData.animationParamHandleType == Training.EAnimationParamHandleType.OnlyForward)
//            {
//                smoothInputDirectionX = 0.0f;
//                smoothInputDirectionY = 1.0f;
//            }
//            else if (stateMachineController.playerMovement.temporaryLocomotion.data.followCameraData.followAnimationData.animationParamHandleType == Training.EAnimationParamHandleType.AdjustToInputDirection)
//            {
//                smoothInputDirectionX = Mathf.Lerp(stateMachineController.playerMovement.lastInputDirection.x,
//                stateMachineController.playerMovement.inputDirection.x, stateMachineController.playerMovement.speedBlendFactor * Time.deltaTime);

//                smoothInputDirectionY = Mathf.Lerp(stateMachineController.playerMovement.lastInputDirection.y,
//                stateMachineController.playerMovement.inputDirection.y, stateMachineController.playerMovement.speedBlendFactor * Time.deltaTime);
//            }
//        }
//        else if (stateMachineController.playerMovement.cameraManager.currentCameraType == ECameraType.LockOnCamera)
//        {
//            //Animation Params Handle : LockOnCam
//            if (stateMachineController.playerMovement.temporaryLocomotion.data.lockOnCameraData.lockOnAnimationData.animationParamHandleType == Training.EAnimationParamHandleType.OnlyForward)
//            {
//                smoothInputDirectionX = 0.0f;
//                smoothInputDirectionY = 1.0f;
//            }
//            else if (stateMachineController.playerMovement.temporaryLocomotion.data.lockOnCameraData.lockOnAnimationData.animationParamHandleType == Training.EAnimationParamHandleType.AdjustToInputDirection)
//            {
//                smoothInputDirectionX = Mathf.Lerp(stateMachineController.playerMovement.lastInputDirection.x,
//                stateMachineController.playerMovement.inputDirection.x, stateMachineController.playerMovement.speedBlendFactor * Time.deltaTime);

//                smoothInputDirectionY = Mathf.Lerp(stateMachineController.playerMovement.lastInputDirection.y,
//                stateMachineController.playerMovement.inputDirection.y, stateMachineController.playerMovement.speedBlendFactor * Time.deltaTime);
//            }
//        }

//        stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Param, smoothInputDirectionY);
//        stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Param, smoothInputDirectionX);

//        //Dodge Move
//        //float smoothDodgeInputDirectionX = Mathf.Lerp(stateMachineController.playerMovement.lastInputDodgeDirection.x,
//        //      stateMachineController.playerMovement.inputDirection.x, stateMachineController.playerMovement.speedBlendFactor / 10.0f * Time.deltaTime);
//        //float smoothDodgeInputDirectionY = Mathf.Lerp(stateMachineController.playerMovement.lastInputDodgeDirection.y,
//        //      stateMachineController.playerMovement.inputDirection.y, stateMachineController.playerMovement.speedBlendFactor / 10.0f * Time.deltaTime);

//        //stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Dodge_Param, smoothDodgeInputDirectionY);
//        //stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Dodge_Param, smoothDodgeInputDirectionX);
//    }
//    public override void HandleMovement()
//    {
        
//        stateMachineController.playerMovement.targetVelocity = stateMachineController.playerMovement.moveDirection.normalized * stateMachineController.playerMovement.speed;

//        // 2. Smooth chuyển đổi từ hướng hiện tại sang hướng mới
//        stateMachineController.playerMovement.currentVelocity = Vector3.MoveTowards(stateMachineController.playerMovement.currentVelocity, stateMachineController.playerMovement.targetVelocity,
//            (stateMachineController.playerMovement.currentVelocity.magnitude > stateMachineController.playerMovement.targetVelocity.magnitude 
//            ? stateMachineController.playerMovement.deceleration : stateMachineController.playerMovement.acceleration) * Time.fixedDeltaTime);

//        // Di chuyển nhân vật
//        stateMachineController.playerMovement.characterController.Move(stateMachineController.playerMovement.currentVelocity * Time.fixedDeltaTime);
//    }
//    public override void HandleSpeed()
//    {
//        stateMachineController.playerMovement.speed = Mathf.Lerp(stateMachineController.playerMovement.speed, 
//            stateMachineController.playerMovement.desiredSpeed, Time.deltaTime * stateMachineController.playerMovement.speedBlendFactor);
//    }

//    public void HandleTurnWithCameraRotationDiff()
//    {
//        //if (Time.time > stateMachineController.playerMovement.turnedTime + stateMachineController.playerMovement.turnDelayTime)
//        //{
//        //    Vector2 lastInputDirectionNorm = stateMachineController.playerMovement.lastInputDirection.normalized;
//        //    Vector2 inputDirectionNorm = stateMachineController.playerMovement.inputDirection.normalized;
//        //    if (lastInputDirectionNorm == -inputDirectionNorm && stateMachineController.playerMovement.speed >= stateMachineController.playerMovement.sprintSpeed * .5f)
//        //    {
//        //        stateMachineController.playerMovement.animator.SetTrigger(AnimationParams.Turn_Trigger);
//        //        //stateMachineController.playerMovement.desiredSpeed = 0.0f;
//        //        stateMachineController.playerMovement.turnedTime = Time.time;
//        //    }
//        //    else if (stateMachineController.playerMovement.lastInputDirection != -stateMachineController.playerMovement.inputDirection)
//        //    {
//        //        if (Time.time > stateMachineController.playerMovement.cameraUpdatedTime + stateMachineController.playerMovement.cameraUpdatedDelay)
//        //        {
//        //            stateMachineController.playerMovement.angle = Vector3.Angle(stateMachineController.playerMovement.lastCameraDirection, stateMachineController.playerMovement.cameraTransform.forward);
//        //            // Lấy thông tin state hiện tại của Animator ở Layer 0
//        //            AnimatorStateInfo stateInfo = stateMachineController.playerMovement.animator.GetCurrentAnimatorStateInfo(0);
//        //            if (stateMachineController.playerMovement.angle > stateMachineController.playerMovement.angleRequired && !stateInfo.IsName("Turn") && stateMachineController.playerMovement.speed >= stateMachineController.playerMovement.sprintSpeed * .75f)
//        //            {

//        //                stateMachineController.playerMovement.animator.CrossFadeInFixedTime(AnimationParams.Turn_State, .1f);
//        //                //stateMachineController.playerMovement.desiredSpeed = 0.0f;
//        //                stateMachineController.playerMovement.turnedTime = Time.time;
//        //            }
//        //            stateMachineController.playerMovement.lastCameraDirection = stateMachineController.playerMovement.cameraTransform.forward;
//        //            stateMachineController.playerMovement.cameraUpdatedTime = Time.time;
//        //        }
//        //    }
//        //}
//    }
//}