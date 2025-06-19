//using UnityEngine;
//using UnityEngine.EventSystems;

//public class Dodge : StateMachineBase
//{
//    Vector3 dodgeDirection = Vector3.zero;
//    Vector3 rotationDirection = Vector3.zero;
//    public Dodge(StateMachineController stateMachineController) : base(stateMachineController) { }
//    public override void Enter()
//    {
//        //Debug.Log("Enter Dodge State");
//        //Animator Dodge Speed 
//        stateMachineController.playerMovement.dodgeComponent.isDodging = true;
        
//        if (stateMachineController.playerMovement.inputDirection != Vector2.zero)
//        {
//            dodgeDirection = stateMachineController.playerMovement.moveDirection;
//        }
//        else
//        {
//            dodgeDirection = stateMachineController.playerMovement.cameraTransform.forward;
//        }

//        HandleAnimation();
//        SpecifyDodgeType(stateMachineController.playerMovement.inputDirection);
//        stateMachineController.playerMovement.dodgeComponent.StartDodge(dodgeDirection);
//    }
//    public override void HandleAnimation()
//    {
//        float currentSpeedBlend = stateMachineController.playerMovement.animator.GetFloat(AnimationParams.Speed_Param);
//        stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Dodge_Speed_Param, Mathf.Round(currentSpeedBlend));
//    }
//    public override void Update()
//    {
//        Debug.Log("On Dodge State");
//        //Transition To Another States
//        //Run
//        if (stateMachineController.playerMovement.dodgeComponent.currentDodgeState == EDodgeState.OffDodge)
//        {
//            if (stateMachineController.playerMovement.inputDirection != Vector2.zero && stateMachineController.playerMovement.isSprint)
//            {
//                stateMachineController.SwitchState(stateMachineController.runState);
//            }
//            //Walk
//            else if (stateMachineController.playerMovement.inputDirection != Vector2.zero && !stateMachineController.playerMovement.isSprint)
//            {
//                stateMachineController.SwitchState(stateMachineController.walkState);
//            }
//            else if(stateMachineController.playerMovement.inputDirection == Vector2.zero)
//            {
//                stateMachineController.SwitchState(stateMachineController.idleState);
//            }
//        }
//    }
//    public override void FixedUpdate()
//    {
//        HandleRotation();
//    }
//    public override void Exit()
//    {
//        stateMachineController.playerMovement.dodgeComponent.isDodging = false;
//    }

//    public override void HandleRotation()
//    {
//        if (stateMachineController.playerMovement.cameraManager.currentCameraType == ECameraType.FollowCamera)
//        {
//            //Rotation Normal
//            if (stateMachineController.playerMovement.temporaryLocomotion.data.followCameraData.followMovementData.dodgeRotateType == Training.ERotateType.ToMoveDirection)
//            {
//                stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(stateMachineController.playerMovement.moveDirection);
//            }
//            else if (stateMachineController.playerMovement.temporaryLocomotion.data.followCameraData.followMovementData.dodgeRotateType == Training.ERotateType.ToTarget)
//            {
//                Vector3 targetDirection = stateMachineController.playerMovement.targetingComponent.target.transform.position - stateMachineController.playerMovement.transform.position;
//                targetDirection.Normalize();
//                stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(targetDirection);
//            }
//            if (stateMachineController.playerMovement.inputDirection == Vector2.zero)
//            {
//                Vector3 targetDirection = stateMachineController.playerMovement.cameraTransform.forward;
//                targetDirection.y = 0.0f;
//                stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(targetDirection);
//            }
//        }
//        else if (stateMachineController.playerMovement.cameraManager.currentCameraType == ECameraType.LockOnCamera)
//        {
//            //Rotation Normal
//            if (stateMachineController.playerMovement.temporaryLocomotion.data.lockOnCameraData.lockOnMovementData.dodgeRotateType == Training.ERotateType.ToMoveDirection)
//            {
//                stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(stateMachineController.playerMovement.moveDirection);
//            }
//            else if (stateMachineController.playerMovement.temporaryLocomotion.data.lockOnCameraData.lockOnMovementData.dodgeRotateType == Training.ERotateType.ToTarget)
//            {
//                Vector3 targetDirection = stateMachineController.playerMovement.targetingComponent.target.transform.position - stateMachineController.playerMovement.transform.position;
//                targetDirection.Normalize();
//                stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(targetDirection);
//            }
//        }

//        stateMachineController.playerMovement.transform.rotation = Quaternion.Slerp(stateMachineController.playerMovement.transform.rotation,
//            stateMachineController.playerMovement.targetRotation, Time.fixedDeltaTime * stateMachineController.playerMovement.rotationFactor);
//    }
//    public override void HandleMovement()
//    {
        
//    }
//    public override void HandleSpeed()
//    {
//        stateMachineController.playerMovement.speed = Mathf.Lerp(stateMachineController.playerMovement.speed,
//            stateMachineController.playerMovement.desiredSpeed, Time.deltaTime * stateMachineController.playerMovement.speedBlendFactor);
//    }
//    public override void HandleSmoothParams()
//    {

//    }

//    public void SpecifyDodgeType(Vector2 inputDirection)
//    {
//        if (stateMachineController.playerMovement.cameraManager.currentCameraType == ECameraType.LockOnCamera)
//        {

//            if (stateMachineController.playerMovement.temporaryLocomotion.data.lockOnCameraData.lockOnMovementData.dodgeRotateType == Training.ERotateType.ToMoveDirection)
//            {
//                AdjustToForward();
//            }
//            else if (stateMachineController.playerMovement.temporaryLocomotion.data.lockOnCameraData.lockOnMovementData.dodgeRotateType == Training.ERotateType.ToTarget)
//            {
//                AdjustToInput(inputDirection);
//            }
//        }
//        else if(stateMachineController.playerMovement.cameraManager.currentCameraType == ECameraType.FollowCamera)
//        {
//            if (stateMachineController.playerMovement.temporaryLocomotion.data.followCameraData.followMovementData.dodgeRotateType == Training.ERotateType.ToMoveDirection)
//            {
//                AdjustToForward();
//            }
//            else if (stateMachineController.playerMovement.temporaryLocomotion.data.followCameraData.followMovementData.dodgeRotateType == Training.ERotateType.ToTarget)
//            {
//                AdjustToInput(inputDirection);
//            }
//        }
//    }

//    void AdjustToInput(Vector2 inputDirection)
//    {
//        if (inputDirection == Vector2.zero)
//        {
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Dodge_Param, 0.0f);
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Dodge_Param, 1.0f);
//        }
//        else if (inputDirection == Vector2.up)
//        {
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Dodge_Param, 0.0f);
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Dodge_Param, 1.0f);
//        }
//        else if (inputDirection == Vector2.down)
//        {
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Dodge_Param, 0.0f);
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Dodge_Param, -1.0f);
//        }
//        else if (inputDirection == Vector2.left)
//        {
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Dodge_Param, -1.0f);
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Dodge_Param, 0.0f);
//        }
//        else if (inputDirection == Vector2.right)
//        {
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Dodge_Param, 1.0f);
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Dodge_Param, 0.0f);
//        }
//        else if (Mathf.Approximately(inputDirection.x, Mathf.Sqrt(2) / 2) && Mathf.Approximately(inputDirection.y, Mathf.Sqrt(2) / 2))
//        {
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Dodge_Param, Mathf.Sqrt(2) / 2);
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Dodge_Param, Mathf.Sqrt(2) / 2);
//        }
//        else if (Mathf.Approximately(inputDirection.x, Mathf.Sqrt(2) / 2) && Mathf.Approximately(inputDirection.y, -Mathf.Sqrt(2) / 2))
//        {
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Dodge_Param, Mathf.Sqrt(2) / 2);
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Dodge_Param, -Mathf.Sqrt(2) / 2);
//        }
//        else if (Mathf.Approximately(inputDirection.x, -Mathf.Sqrt(2) / 2) && Mathf.Approximately(inputDirection.y, -Mathf.Sqrt(2) / 2))
//        {
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Dodge_Param, -Mathf.Sqrt(2) / 2);
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Dodge_Param, -Mathf.Sqrt(2) / 2);
//        }
//        else if (Mathf.Approximately(inputDirection.x, -Mathf.Sqrt(2) / 2) && Mathf.Approximately(inputDirection.y, Mathf.Sqrt(2) / 2))
//        {
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Dodge_Param, -Mathf.Sqrt(2) / 2);
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Dodge_Param, Mathf.Sqrt(2) / 2);
//        }
//    }
//    void AdjustToForward()
//    {
//        stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Horizontal_Dodge_Param, 0.0f);
//        stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Input_Vertical_Dodge_Param, 1.0f);
//    }
//}