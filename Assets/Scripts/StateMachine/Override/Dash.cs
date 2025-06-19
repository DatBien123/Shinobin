//using UnityEngine;
//using UnityEngine.EventSystems;

//public class Dash : StateMachineBase
//{
//    Vector3 dodgeDirection = Vector3.zero;
//    Vector3 rotationDirection = Vector3.zero;
//    public Dash(StateMachineController stateMachineController) : base(stateMachineController) { }
//    public override void Enter()
//    {
//        //Debug.Log("Enter Dodge State");
//        //Animator Dodge Speed 
//        stateMachineController.playerMovement.dodgeComponent.isDodging = true;
//        float currentSpeedBlend = stateMachineController.playerMovement.animator.GetFloat(AnimationParams.Speed_Param);


//        //H??ng di chuy?n và H??ng quay c?a nhân v?t
//        if (stateMachineController.playerMovement.inputDirection != Vector2.zero)
//        {
//            dodgeDirection = stateMachineController.playerMovement.moveDirection;
//            if (stateMachineController.playerMovement.isSprint)
//            {
//                rotationDirection = dodgeDirection;
//                stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Dodge_Speed_Param, 2.0f);
//            }
//            else
//            {
//                rotationDirection = stateMachineController.playerMovement.cameraTransform.forward;
//                stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Dodge_Speed_Param, Mathf.Round(currentSpeedBlend));
//            }
//        }
//        else
//        {
//            //Debug.Log("Dodge Forward 0 Speed");
//            dodgeDirection = stateMachineController.playerMovement.transform.forward;
//            rotationDirection = dodgeDirection;
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Dodge_Speed_Param, Mathf.Round(currentSpeedBlend));
//        }

//        SpecifyDodgeType(stateMachineController.playerMovement.inputDirection);
//        stateMachineController.playerMovement.dodgeComponent.StartDodge(dodgeDirection);
//    }
//    public override void Update()
//    {
//        Debug.Log("On Dodge State");
//        HandleMovement();
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
//            else if (stateMachineController.playerMovement.inputDirection == Vector2.zero)
//            {
//                stateMachineController.SwitchState(stateMachineController.idleState);
//            }
//        }
//    }
//    public override void FixedUpdate()
//    {

//    }
//    public override void Exit()
//    {
//        stateMachineController.playerMovement.dodgeComponent.isDodging = false;
//    }
//    public override void HandleAnimation()
//    {

//    }
//    public override void HandleRotation()
//    {
//        stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(rotationDirection);

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
//}