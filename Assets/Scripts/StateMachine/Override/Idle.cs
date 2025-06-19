//using UnityEngine;

//public class Idle : StateMachineBase
//{
//    public Idle(StateMachineController stateMachineController) : base(stateMachineController) { }
//    public override void Enter()
//    {
//        //Init all move props
//        stateMachineController.playerMovement.desiredSpeed = 0.0f;
//        stateMachineController.playerMovement.desiredSpeedBlend = 0.0f;
//        stateMachineController.playerMovement.targetVelocity = Vector3.zero;
//    }
//    public override void Update()
//    {
//        Debug.Log("On Idle State");
//        HandleSpeed();
//        HandleAnimation();
//        //Transition To Another States
//        //Run
//        if (stateMachineController.playerMovement.inputDirection != Vector2.zero && stateMachineController.playerMovement.isSprint)
//        {
//            stateMachineController.SwitchState(stateMachineController.runState);
//        }
//        //Walk
//        else if(stateMachineController.playerMovement.inputDirection != Vector2.zero && !stateMachineController.playerMovement.isSprint)
//        {
//            stateMachineController.SwitchState(stateMachineController.walkState);
//        }
//    }
//    public override void FixedUpdate()
//    {

//    }
//    public override void Exit()
//    {

//    }
//    public override void HandleAnimation()
//    {
//        //Speed Blend
//        if (stateMachineController.playerMovement.speedBlend > 0.1f)
//        {
//            stateMachineController.playerMovement.speedBlend = Mathf.Lerp(stateMachineController.playerMovement.speedBlend,
//            stateMachineController.playerMovement.desiredSpeedBlend, Time.deltaTime * stateMachineController.playerMovement.speedBlendFactor);
//            stateMachineController.playerMovement.animator.SetFloat(AnimationParams.Speed_Param, stateMachineController.playerMovement.speedBlend);
//        }
//        //Assign
//        stateMachineController.playerMovement.lastDesiredSpeedBlend = stateMachineController.playerMovement.desiredSpeedBlend;
//        stateMachineController.playerMovement.lastInputDirection = stateMachineController.playerMovement.inputDirection;
//    }
//    public override void HandleRotation()
//    {

//    }
//    public override void HandleMovement()
//    {

//    }
//    public override void HandleSpeed()
//    {
//        if (stateMachineController.playerMovement.speed > 0.1f)
//            stateMachineController.playerMovement.speed = Mathf.Lerp(stateMachineController.playerMovement.speed,
//            stateMachineController.playerMovement.desiredSpeed, Time.deltaTime * stateMachineController.playerMovement.speedBlendFactor);
//    }
//    public override void HandleSmoothParams()
//    {
        
//    }
//}