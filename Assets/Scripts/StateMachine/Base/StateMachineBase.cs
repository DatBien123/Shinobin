
using UnityEngine;
public abstract class StateMachineBase
{
    public StateMachineController stateMachineController;
    public StateMachineBase(StateMachineController stateMachineController)
    {
        this.stateMachineController = stateMachineController; 
    }
    public abstract void Enter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();
    public abstract void HandleAnimation();
    public abstract void HandleSpeed();
    public abstract void HandleMovement();
    public abstract void HandleRotation();
    public abstract void HandleSmoothParams();
}