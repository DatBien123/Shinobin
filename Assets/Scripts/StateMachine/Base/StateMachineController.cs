
public class StateMachineController
{
    public CharacterPlayer playerMovement;

    public StateMachineBase currentState;

    public StateMachineBase idleState;
    public StateMachineBase runState;
    public StateMachineBase walkState;
    public StateMachineBase turnState;
    public StateMachineBase stopState;
    public StateMachineBase dodgeState;
    public StateMachineBase jumpState;
    public StateMachineBase blockState;
    public StateMachineBase hitReactionState;
    public StateMachineBase recoverState;

    public StateMachineController(CharacterPlayer playerMovement)
    {
        this.playerMovement = playerMovement;
    }

    public void Init()
    {
        //idleState = new Idle(this);
        //walkState = new Walk(this);
        //runState = new Run(this);
        //turnState = new Turn(this);
        //stopState = new Stop(this);
        //dodgeState = new Dodge(this);
        //jumpState = new Jump(this);
        //blockState = new Block(this);
        //hitReactionState = new HitReaction(this);
        //recoverState = new Recovery(this);

        SwitchState(idleState);
    }
    public void UpdateCurrentState()
    {
        if (currentState != null) { 
            currentState.Update();
        }
    }
    public void FixedUpdateCurrentState()
    {
        if (currentState != null)
        {
            currentState.FixedUpdate();
        }
    }
    public void SwitchState(StateMachineBase newState)
    {
        if (currentState != newState)
        {
            if(currentState != null)currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}