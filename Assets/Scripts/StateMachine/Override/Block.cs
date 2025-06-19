//using UnityEngine;

//public class Block : StateMachineBase
//{
//    public Block(StateMachineController stateMachineController) : base(stateMachineController) { }
//    public override void Enter()
//    {

//        stateMachineController.playerMovement.blockComponent.StartBlock();
//        stateMachineController.playerMovement.currentVelocity = Vector3.zero;
//    }
//    public override void Update()
//    {
//        Debug.Log("On Block State");
//        HandleMovement();
//        //Transition Handle Only When
//        //- Block Finish
//        if (!stateMachineController.playerMovement.blockComponent.isBlocking)
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
//    public override void Exit()
//    {
//        //Hàm này có tác dụng để ngắt block 
//        //cụ thể trong các trường hợp như Nhân vật bị đánh và sẽ chuyển sang trạng thái OnHit
//        //từ đó chuyển sang trạng thái HitReaction .

//        //Có nghĩa là cứ khi nào chuyển sang trạng thái HitReaction/ Chạy animation Knockback/Airborne
//        //thì sẽ tự động ngắt Block 
//        //if (stateMachineController.playerMovement.blockComponent.currentBlockState != EBlockState.OffBlock)
//        //{
//        //    stateMachineController.playerMovement.blockComponent.EndBlockProactive();
//        //}
//        /*else*/ stateMachineController.playerMovement.blockComponent.EndBlockPassive();
//    }
//    public override void HandleMovement()
//    {
        
//    }
//    public override void HandleRotation()
//    {
//        //Only Update Rotation
//        Vector3 cameraForward = stateMachineController.playerMovement.cameraTransform.forward;
//        cameraForward.y = 0;
//        cameraForward.Normalize();
//        stateMachineController.playerMovement.targetRotation = Quaternion.LookRotation(cameraForward);

//        stateMachineController.playerMovement.transform.rotation = Quaternion.Slerp(stateMachineController.playerMovement.transform.rotation,
//            stateMachineController.playerMovement.targetRotation, Time.fixedDeltaTime * stateMachineController.playerMovement.rotationFactor);
//    }
//    public override void FixedUpdate() { }
//    public override void HandleAnimation() { }
//    public override void HandleSpeed(){}
//    public override void HandleSmoothParams(){}
//}