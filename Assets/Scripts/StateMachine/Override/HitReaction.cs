//using Training;
//using UnityEngine;

//public class HitReaction : StateMachineBase
//{
//    public HitReaction(StateMachineController stateMachineController) : base(stateMachineController) { }
//    public override void Enter()
//    {
//        stateMachineController.playerMovement.hitReactionComponent.isOnHitReact = true;
//        stateMachineController.playerMovement.currentVelocity = Vector3.zero;
//    }
//    public override void Update()
//    {
//        Debug.Log("On Hit Reaction State");
//        HandleMovement();
//        //Transition Handle Only When
//        //- Hit Reaction Finish   &&  Current HitReactionType is Weak or Strong !

//        if(stateMachineController.playerMovement.hitReactionComponent.currentReactionType == EReactionType.Combo)
//        {
//            UpdateComboReaction();
//        }
//        else if(stateMachineController.playerMovement.hitReactionComponent.currentReactionType == EReactionType.Skill)
//        {
//            UpdateSkillReaction();
//        }



//    }

//    void UpdateSkillReaction()
//    {
//        if (stateMachineController.playerMovement.hitReactionComponent.currentHitReactionState == EHitReactionState.OffHit)
//        {
//                if (stateMachineController.playerMovement.inputDirection != Vector2.zero && stateMachineController.playerMovement.isSprint)
//                {
//                    stateMachineController.SwitchState(stateMachineController.runState);
//                }
//                else if (stateMachineController.playerMovement.inputDirection != Vector2.zero && !stateMachineController.playerMovement.isSprint)
//                {
//                    stateMachineController.SwitchState(stateMachineController.walkState);
//                }
//                else if (stateMachineController.playerMovement.inputDirection == Vector2.zero)
//                {
//                    stateMachineController.SwitchState(stateMachineController.idleState);
//                }

//        }
//    }
//    void UpdateComboReaction()
//    {
//        if (stateMachineController.playerMovement.hitReactionComponent.currentHitReactionState == EHitReactionState.OffHit)
//        {
//                if (stateMachineController.playerMovement.inputDirection != Vector2.zero && stateMachineController.playerMovement.isSprint)
//                {
//                    stateMachineController.SwitchState(stateMachineController.runState);
//                }
//                else if (stateMachineController.playerMovement.inputDirection != Vector2.zero && !stateMachineController.playerMovement.isSprint)
//                {
//                    stateMachineController.SwitchState(stateMachineController.walkState);
//                }
//                else if (stateMachineController.playerMovement.inputDirection == Vector2.zero)
//                {
//                    stateMachineController.SwitchState(stateMachineController.idleState);
//                }

//        }
//    }
//    public override void FixedUpdate()
//    {

//    }
//    public override void Exit()
//    {
//        stateMachineController.playerMovement.hitReactionComponent.isOnHitReact = false;
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