//using Training;
//using UnityEngine;

//public class Recovery : StateMachineBase
//{
//    public Recovery(StateMachineController stateMachineController) : base(stateMachineController) { }
//    public override void Enter()
//    {
//        stateMachineController.playerMovement.hitReactionComponent.isRecovering = true;
//        stateMachineController.playerMovement.currentVelocity = Vector3.zero;
//    }
//    public override void Update()
//    {
//        Debug.Log("On Recover State");
//        HandleMovement();
//        //Transition Handle Only When
//        //- Block Finish
//        if (stateMachineController.playerMovement.hitReactionComponent.currentRecoverState == ERecoverState.OffRecover)
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

//    }
//    public override void Exit()
//    {
//        stateMachineController.playerMovement.hitReactionComponent.isRecovering = false;
//        stateMachineController.playerMovement.hitReactionComponent.currentRecoverState = ERecoverState.OffRecover;
//    }
//    public override void HandleAnimation()
//    {

//    }
//    public override void HandleRotation()
//    {

//    }
//    public override void HandleMovement()
//    {

//    }
//    public override void HandleSpeed()
//    {

//    }
//    public override void HandleSmoothParams()
//    {

//    }
//}