//using UnityEngine;
//using UnityEngine.EventSystems;

//public class Jump : StateMachineBase
//{
//    public Jump(StateMachineController stateMachineController) : base(stateMachineController) { }
//    public override void Enter()
//    {
//        stateMachineController.playerMovement.jumpComponent.StartJump();
//    }
//    public override void Update()
//    {
//        Debug.Log("On Jump State");
//        HandleSpeed();
//        HandleAnimation();

//        //Transition To Another States
//        if (stateMachineController.playerMovement.jumpComponent.currentJumpState == EJumpState.None || stateMachineController.playerMovement.isGround)
//        {
//            if (stateMachineController.playerMovement.inputDirection != Vector2.zero && stateMachineController.playerMovement.isSprint)
//            {
//                stateMachineController.SwitchState(stateMachineController.runState);
//            }
//            else if (stateMachineController.playerMovement.inputDirection != Vector2.zero && !stateMachineController.playerMovement.isSprint)
//            {
//                stateMachineController.SwitchState(stateMachineController.walkState);
//            }
//            else if (stateMachineController.playerMovement.inputDirection == Vector2.zero)
//            {
//                stateMachineController.SwitchState(stateMachineController.idleState);
//            }
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
//    public override void HandleRotation()
//    {
//        //Rotation
//        if (stateMachineController.playerMovement.inputDirection != Vector2.zero)
//        {
//            stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(stateMachineController.playerMovement.moveDirection);
//            //if (!stateMachineController.playerMovement.temporaryLocomotion.data.isRotateWhenJump)
//            //{
//            //    Vector3 cameraForward = stateMachineController.playerMovement.cameraTransform.forward;
//            //    cameraForward.y = 0;
//            //    cameraForward.Normalize();
//            //    stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(cameraForward);
//            //}

//            stateMachineController.playerMovement.transform.rotation = Quaternion.Slerp(stateMachineController.playerMovement.transform.rotation,
//                stateMachineController.playerMovement.targetRotation, Time.fixedDeltaTime * stateMachineController.playerMovement.rotationFactor);

//        }
//    }
//    public override void HandleMovement()
//    {
//        if (!stateMachineController.playerMovement.jumpComponent.isJumping)
//        {
//            stateMachineController.playerMovement.targetVelocity = stateMachineController.playerMovement.moveDirection.normalized * stateMachineController.playerMovement.speed;
//            //Decrease Velocity on Heavy Fall
//            if (stateMachineController.playerMovement.jumpComponent.currentJumpState == EJumpState.OnHeavyJumpEnd)
//                stateMachineController.playerMovement.targetVelocity = stateMachineController.playerMovement.moveDirection.normalized * stateMachineController.playerMovement.speed / 5.0f;

//            // 2. Smooth chuyển đổi từ hướng hiện tại sang hướng mới
//            stateMachineController.playerMovement.currentVelocity = Vector3.MoveTowards(stateMachineController.playerMovement.currentVelocity, stateMachineController.playerMovement.targetVelocity,
//                (stateMachineController.playerMovement.currentVelocity.magnitude > stateMachineController.playerMovement.targetVelocity.magnitude
//                ? stateMachineController.playerMovement.deceleration : stateMachineController.playerMovement.acceleration) * Time.fixedDeltaTime);

//            // Di chuyển nhân vật
//            stateMachineController.playerMovement.characterController.Move(stateMachineController.playerMovement.currentVelocity * Time.fixedDeltaTime);
//            //}
//        }
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
//            float smoothInputDirectionX = Mathf.Lerp(stateMachineController.playerMovement.lastInputDirection.x,
//                stateMachineController.playerMovement.inputDirection.x, stateMachineController.playerMovement.speedBlendFactor * Time.deltaTime);
//            float smoothInputDirectionY = Mathf.Lerp(stateMachineController.playerMovement.lastInputDirection.y,
//                stateMachineController.playerMovement.inputDirection.y, stateMachineController.playerMovement.speedBlendFactor * Time.deltaTime);

//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Param, smoothInputDirectionY);
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Param, smoothInputDirectionX);
//        }
//    }
//}