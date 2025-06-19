//using UnityEngine;
//public class Walk : StateMachineBase
//{
//    public Walk(StateMachineController stateMachineController) : base(stateMachineController) { }
//    public override void Enter()
//    {
//        //Debug.Log("Enter Walk State");
//        //Init all move props
//        stateMachineController.playerMovement.desiredSpeed = stateMachineController.playerMovement.moveSpeed;
//        stateMachineController.playerMovement.desiredSpeedBlend = 1.0f;
//    }
//    public override void Update()
//    {
//        Debug.Log("On Walk State");
//        HandleSpeed();
//        HandleAnimation();
//        //Transition To Another States
//        //Idle
//        if (stateMachineController.playerMovement.inputDirection == Vector2.zero)
//        {
//            stateMachineController.SwitchState(stateMachineController.idleState);
//        }
//        //Run
//        else if (stateMachineController.playerMovement.inputDirection != Vector2.zero && stateMachineController.playerMovement.isSprint)
//        {
//            stateMachineController.SwitchState(stateMachineController.runState);
//        }
//    }
//    public override void FixedUpdate()
//    {
//        HandleRotation();
//        HandleMovement();
//    }
//    public override void Exit()
//    {

//    }
//    public override void HandleRotation()
//    {

//        if (stateMachineController.playerMovement.cameraManager.currentCameraType == ECameraType.FollowCamera)
//        {
//            //Rotation Normal
//            if(stateMachineController.playerMovement.temporaryLocomotion.data.followCameraData.followMovementData.walkRotateType == Training.ERotateType.ToMoveDirection)
//            {
//                stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(stateMachineController.playerMovement.moveDirection);
//            }
//            else if(stateMachineController.playerMovement.temporaryLocomotion.data.followCameraData.followMovementData.walkRotateType == Training.ERotateType.ToTarget)
//            {
//                Vector3 targetDirection = stateMachineController.playerMovement.targetingComponent.target.transform.position - stateMachineController.playerMovement.transform.position;
//                targetDirection.Normalize();
//                stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(targetDirection);
//            }
//        }
//        else if (stateMachineController.playerMovement.cameraManager.currentCameraType == ECameraType.LockOnCamera)
//        {
//            //Rotation Normal
//            if (stateMachineController.playerMovement.temporaryLocomotion.data.lockOnCameraData.lockOnMovementData.walkRotateType == Training.ERotateType.ToMoveDirection)
//            {
//                stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(stateMachineController.playerMovement.moveDirection);
//            }
//            else if (stateMachineController.playerMovement.temporaryLocomotion.data.lockOnCameraData.lockOnMovementData.walkRotateType == Training.ERotateType.ToTarget)
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
//    public override void HandleAnimation()
//    {
//        stateMachineController.playerMovement.lastInputDirection.x = stateMachineController.playerMovement.animator.GetFloat(AnimationParams.Input_Horizontal_Param);
//        stateMachineController.playerMovement.lastInputDirection.y = stateMachineController.playerMovement.animator.GetFloat(AnimationParams.Input_Vertical_Param);

//        HandleSmoothParams();
//        //Speed Blend
//        stateMachineController.playerMovement.speedBlend = Mathf.Lerp(stateMachineController.playerMovement.speedBlend,
//            stateMachineController.playerMovement.desiredSpeedBlend, Time.deltaTime * stateMachineController.playerMovement.speedBlendFactor);
//        if (stateMachineController.playerMovement.speedBlend > 0.1f) stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Speed_Param, stateMachineController.playerMovement.speedBlend);

//        //Assign
//        stateMachineController.playerMovement.lastDesiredSpeedBlend = stateMachineController.playerMovement.desiredSpeedBlend;
//        stateMachineController.playerMovement.lastInputDirection = stateMachineController.playerMovement.inputDirection;
//    }
//    public override void HandleSpeed()
//    {
//        stateMachineController.playerMovement.speed = Mathf.Lerp(stateMachineController.playerMovement.speed,
//            stateMachineController.playerMovement.desiredSpeed, Time.deltaTime * stateMachineController.playerMovement.speedBlendFactor);
//    }
//    public override void HandleSmoothParams()
//    {
//        //Normal Move
//        if (stateMachineController.playerMovement.dodgeComponent.currentDodgeState != EDodgeState.OnDodge)
//        {
//            //
//            float smoothInputDirectionX = Mathf.Lerp(stateMachineController.playerMovement.lastInputDirection.x,
//                stateMachineController.playerMovement.inputDirection.x, stateMachineController.playerMovement.speedBlendFactor * Time.deltaTime);
//            float smoothInputDirectionY = Mathf.Lerp(stateMachineController.playerMovement.lastInputDirection.y,
//                stateMachineController.playerMovement.inputDirection.y, stateMachineController.playerMovement.speedBlendFactor * Time.deltaTime);
//            //

//            if (stateMachineController.playerMovement.cameraManager.currentCameraType == ECameraType.FollowCamera)
//            {
//                //Animation Params Handle : FollowCam
//                if (stateMachineController.playerMovement.temporaryLocomotion.data.followCameraData.followAnimationData.animationParamHandleType == Training.EAnimationParamHandleType.OnlyForward)
//                {
//                    smoothInputDirectionX = 0.0f;
//                    smoothInputDirectionY = 1.0f;
//                }
//                else if (stateMachineController.playerMovement.temporaryLocomotion.data.followCameraData.followAnimationData.animationParamHandleType == Training.EAnimationParamHandleType.AdjustToInputDirection)
//                {
//                    smoothInputDirectionX = Mathf.Lerp(stateMachineController.playerMovement.lastInputDirection.x,
//                    stateMachineController.playerMovement.inputDirection.x, stateMachineController.playerMovement.speedBlendFactor * Time.deltaTime);

//                    smoothInputDirectionY = Mathf.Lerp(stateMachineController.playerMovement.lastInputDirection.y,
//                    stateMachineController.playerMovement.inputDirection.y, stateMachineController.playerMovement.speedBlendFactor * Time.deltaTime);
//                }
//            }
//            else if (stateMachineController.playerMovement.cameraManager.currentCameraType == ECameraType.LockOnCamera)
//            {
//                //Animation Params Handle : LockOnCam
//                if (stateMachineController.playerMovement.temporaryLocomotion.data.lockOnCameraData.lockOnAnimationData.animationParamHandleType == Training.EAnimationParamHandleType.OnlyForward)
//                {
//                    smoothInputDirectionX = 0.0f;
//                    smoothInputDirectionY = 1.0f;
//                }
//                else if (stateMachineController.playerMovement.temporaryLocomotion.data.lockOnCameraData.lockOnAnimationData.animationParamHandleType == Training.EAnimationParamHandleType.AdjustToInputDirection)
//                {
//                    smoothInputDirectionX = Mathf.Lerp(stateMachineController.playerMovement.lastInputDirection.x,
//                    stateMachineController.playerMovement.inputDirection.x, stateMachineController.playerMovement.speedBlendFactor * Time.deltaTime);

//                    smoothInputDirectionY = Mathf.Lerp(stateMachineController.playerMovement.lastInputDirection.y,
//                    stateMachineController.playerMovement.inputDirection.y, stateMachineController.playerMovement.speedBlendFactor * Time.deltaTime);
//                }
//            }



//            //Assign param value
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Param, smoothInputDirectionY);
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Param, smoothInputDirectionX);

//            ////Normal Move
//            //float smoothDodgeInputDirectionX = stateMachineController.playerMovement.inputDirection.x;
//            //float smoothDodgeInputDirectionY = stateMachineController.playerMovement.inputDirection.y;

//            //stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Dodge_Param, smoothDodgeInputDirectionY);
//            //stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Dodge_Param, smoothDodgeInputDirectionX);

//            ////Dodge Move
//            //float smoothDodgeInputDirectionX = Mathf.Lerp(stateMachineController.playerMovement.lastInputDodgeDirection.x,
//            //      stateMachineController.playerMovement.inputDirection.x, stateMachineController.playerMovement.speedBlendFactor / 10.0f * Time.deltaTime);
//            //float smoothDodgeInputDirectionY = Mathf.Lerp(stateMachineController.playerMovement.lastInputDodgeDirection.y,
//            //      stateMachineController.playerMovement.inputDirection.y, stateMachineController.playerMovement.speedBlendFactor / 10.0f * Time.deltaTime);

//            //stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Dodge_Param, smoothDodgeInputDirectionY);
//            //stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Dodge_Param, smoothDodgeInputDirectionX);
//        }
//    }
//}