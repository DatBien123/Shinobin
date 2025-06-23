using System.Collections;
using Training;
using UnityEngine;

public class AIDecision : MonoBehaviour
{
    public CharacterAI ownerAI;

    [Header("Character AI Input Values")]
    public Vector2 move;
    public bool jump;
    public bool sprint;

    [Header("Movement")]
    public EMoveType currentMoveType;
    public EMoveStrafeType currentMoveStrafeType;

    public Vector3 desiredMoveDirection;
    public float stoppingDistanceToTarget = 1.5f;


    private void Awake()
    {
        ownerAI = GetComponent<CharacterAI>();
    }
    #region SetParams
    public void Move(Vector2 moveAIValue)
    {
        if (moveAIValue.magnitude > 0)
        {
            move = moveAIValue;
            ownerAI.animator.SetBool(AnimationParams.HasInputMove_Param, true);
        }
        else if (moveAIValue.magnitude == 0)
        {
            move = Vector2.zero;
            ownerAI.animator.SetBool(AnimationParams.HasInputMove_Param, false);
        }
    }
    public void Sprint(bool isAISprint)
    {
        if (isAISprint)
        {
            sprint = true;
        }
        else if (!isAISprint)
        {
            sprint = false;
        }
    }
    public void Jump(bool isAIJump)
    {
        if (isAIJump && ownerAI.comboComponent.currentComboState != EComboState.Playing)
        {
            jump = true;
        }
        else if (!isAIJump)
        {
            jump = false;
        }
    }
    public void Block(bool isAIBlock)
    {
        if (isAIBlock)
        {
            ownerAI.isBlock = true;
            ownerAI.animator.SetBool(AnimationParams.Block_Param, true);
        }
        else if (!isAIBlock)
        {
            ownerAI.isBlock = false;
            ownerAI.animator.SetBool(AnimationParams.Block_Param, false);
        }
    }
    #endregion

    private void Start()
    {
        //StartMoveToTarget(EMoveType.Sprint);

    }

    #region MoveToTarget
    Coroutine C_MoveToTarget;
    public void StartMoveToTarget(EMoveType moveType)
    {
        if (C_MoveToTarget != null)StopCoroutine(C_MoveToTarget);
        C_MoveToTarget = StartCoroutine(MoveToTarget(moveType));
    }
    IEnumerator MoveToTarget(EMoveType moveType)
    {
        if (ownerAI.targetingComponent.target == null) yield break;

        currentMoveType = moveType;
        if (currentMoveType == EMoveType.Sprint)
        {
            sprint = true;
        }
        Move(new Vector2(0, 1));
        desiredMoveDirection = ownerAI.targetingComponent.target.transform.position - ownerAI.transform.position;
        desiredMoveDirection.Normalize();

        while (ownerAI.targetingComponent.target != null
            && Utilities.Instance.DistanceCalculate(ownerAI.targetingComponent.target.transform.position,
            ownerAI.transform.position) > stoppingDistanceToTarget)
        {
            if (ownerAI.targetingComponent.target == null) break;

            desiredMoveDirection = ownerAI.targetingComponent.target.transform.position - ownerAI.transform.position;
            desiredMoveDirection.Normalize();

            yield return null;
        }

        currentMoveType = EMoveType.None;
        sprint = false;
        Move(new Vector2(0, 0));
        desiredMoveDirection = Vector3.zero;
        ownerAI.currentBehaviorState = EBehaviorState.Finished;
    }
    #endregion





}
