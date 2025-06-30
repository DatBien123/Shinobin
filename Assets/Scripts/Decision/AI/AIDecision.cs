using System.Collections;
using Training;
using UnityEngine;
using UnityEngine.AI;

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

    #region [MoveToTarget]
    Coroutine C_MoveToTarget;
    public void StartMoveToTarget(EMoveType moveType, bool isSprint)
    {
        if (C_MoveToTarget != null)StopCoroutine(C_MoveToTarget);
        C_MoveToTarget = StartCoroutine(MoveToTarget(moveType, isSprint));
    }
    IEnumerator MoveToTarget(EMoveType moveType, bool isSprint)
    {
        if (ownerAI.targetingComponent.target == null) yield break;

        currentMoveType = moveType;

            sprint = isSprint;


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


    Coroutine C_MoveStrafeToTarget;
    public void StartMoveStrafeToTarget(EMoveType moveType, bool isSprint,  float duration)
    {
        if (C_MoveStrafeToTarget != null)
        {
            StopCoroutine(C_MoveStrafeToTarget);
        }
        C_MoveStrafeToTarget = StartCoroutine(MoveStrafeToTarget(moveType,isSprint, duration));
    }
    IEnumerator MoveStrafeToTarget(EMoveType moveType,bool isSprint, float duration)
    {
        if (ownerAI.targetingComponent.target == null) yield break;

        currentMoveType = moveType;
        float strafeSide = StrafeSide();
        if (strafeSide > 0)
        {
            Move(new Vector2(1, 0));
        }
        else
        {
            Move(new Vector2(-1, 0));
        }
            sprint = isSprint;


        desiredMoveDirection = ownerAI.targetingComponent.target.transform.position - ownerAI.transform.position;
        desiredMoveDirection.Normalize();
        float elapsedTime = 0.0f;

        while (ownerAI.targetingComponent.target != null
            && (Utilities.Instance.DistanceCalculate(ownerAI.targetingComponent.target.transform.position,
            ownerAI.transform.position) > stoppingDistanceToTarget || elapsedTime < duration))
        {
            if (ownerAI.targetingComponent.target == null) break;

            desiredMoveDirection = ownerAI.targetingComponent.target.transform.position - ownerAI.transform.position;
            desiredMoveDirection = new Vector3(desiredMoveDirection.z, 0, -desiredMoveDirection.x);
            desiredMoveDirection.Normalize();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentMoveType = EMoveType.None;
        sprint = false;
        Move(new Vector2(0, 0));
        desiredMoveDirection = Vector3.zero;
        ownerAI.currentBehaviorState = EBehaviorState.Finished;
    }

    public float StrafeSide()
    {
        // Hướng từ nhân vật đến đối tượng
        Vector3 directionToTarget = (ownerAI.targetingComponent.target.transform.position - transform.position).normalized;

        // Tích vô hướng giữa hướng ngang (transform.right) và directionToTarget
        float dotProduct = Vector3.Dot(transform.right, directionToTarget);

        // Kiểm tra vị trí
        if (dotProduct > 0)
        {
            Debug.Log("Target is on the right side.");
        }
        else if (dotProduct < 0)
        {
            Debug.Log("Target is on the left side.");
        }
        else
        {
            Debug.Log("Target is directly in front or behind.");
        }
        return dotProduct;
    }




}
