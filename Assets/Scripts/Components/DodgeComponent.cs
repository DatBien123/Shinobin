using System.Collections;
using Training;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[System.Serializable]
public enum EDodgeState
{
    None, OnDodge, OffDodge
}
public class DodgeComponent : MonoBehaviour
{
    [Header("Component")]
    public Character owner;

    [Header("Dodge Stuffs")]
    public float currentDodgeTime = 0.0f;
    public EDodgeState currentDodgeState = EDodgeState.None;
    public float dodgeDistance = 5.0f;
    public float dodgeDuration = 1.0f;
    public AnimationCurve dodgeCurve;
    public float dodgeSpeed = 1.0f;

    public bool isDodging = false;
    public bool canDodge = true;
    public float dodgeDelayTime = 1.5f;
    public float dodgeedTime = 0.0f;

    [Header("Dodge Events")]
    public UnityEvent onDodgeEvent;
    public UnityEvent offDodgeEvent;

    [Header("Dash Stuffs")]
    public float dashFactor = 1.5f;
    public AnimationCurve dashCurve;

    [Header("Dash Events")]
    public UnityEvent onDashEvent;
    public UnityEvent offDashEvent;

    [Header("Teleport Stuffs")]
    public float teleportDuration = 1.0f;

    [Header("Teleport Events")]
    public UnityEvent onTeleportEvent;
    public UnityEvent offTeleportEvent;

    //Coroutine
    public Coroutine C_Dodge;
    public Coroutine C_DodgeAI;
    public Coroutine C_Dash;
    public Coroutine C_Teleport;
    private void Awake()
    {
        owner = GetComponent<Character>();
    }
    public void StartDodge(Vector3 dodgeDirection)
    {
        //Break Lines
        if (owner.hitReactionComponent.currentHitReactionState == EHitReactionState.OnHit
            || owner.hitReactionComponent.currentRecoverState == ERecoverState.OnRecover) return;
        //
        if (C_Dodge != null)
        {
            StopCoroutine(C_Dodge);
        }
        C_Dodge = StartCoroutine(Dodge(dodgeDirection));
    }

    IEnumerator Dodge(Vector3 dodgeDirection)
    {
        //Initialized before Start
        Vector3 direction = dodgeDirection.normalized;
        float speed = dodgeDistance / dodgeDuration;

        float currentHorizontalFactor = owner.animator.GetFloat(AnimationParams.Input_Horizontal_Param);
        float currentVerticalFactor = owner.animator.GetFloat(AnimationParams.Input_Vertical_Param);
        float currentSpeedParam = owner.animator.GetFloat(AnimationParams.Speed_Param);

        owner.animator.SetFloat(AnimationParams.Input_Dodge_Horizontal_Param, currentHorizontalFactor);
        owner.animator.SetFloat(AnimationParams.Input_Dodge_Vertical_Param, currentVerticalFactor);
        owner.animator.SetFloat(AnimationParams.Dodge_Speed_Param, currentSpeedParam);
        
        owner.animator.CrossFadeInFixedTime(AnimationParams.Dodge_State, .1f);

        canDodge = false;
        onDodgeEvent?.Invoke();
        while(currentDodgeState != EDodgeState.OffDodge)
        { 
            yield return null;
        }
        yield return new WaitForSeconds(dodgeDelayTime);
        canDodge = true;
        offDodgeEvent?.Invoke();

    }



    #region [AI]
    public void StartDashAI(Vector3 dashDirection, Vector2 paramsVH)
    {
        //Break Lines
        if (owner.hitReactionComponent.currentHitReactionState == EHitReactionState.OnHit
            || owner.hitReactionComponent.currentRecoverState == ERecoverState.OnRecover) return;
        //
        if (C_Dash != null)
        {
            StopCoroutine(C_Dash);
        }
        C_Dash = StartCoroutine(DashAI(dashDirection, paramsVH));
    }
    public void StartDodgeAI(Vector3 dodgeDirection, Vector2 paramsVH)
    {
        if (C_DodgeAI != null)
        {
            StopCoroutine(C_DodgeAI);
        }
        C_DodgeAI = StartCoroutine(DodgeAI(dodgeDirection, paramsVH));
    }
    IEnumerator DodgeAI(Vector3 dodgeDirection, Vector2 paramsVH)
    {
        //Start Dodge AI
        CharacterAI ownerAI = owner as CharacterAI;

        ownerAI.currentBehaviorState = EBehaviorState.Executing;
        //Initialized before Start
        float elapsedTime = 0.0f;
        Vector3 direction = dodgeDirection.normalized;
        float speed = dodgeDistance / dodgeDuration;


        if (owner.targetingComponent.target != null) owner.transform.LookAt(owner.targetingComponent.target);
        //..
        owner.animator.SetFloat(AnimationParams.Input_Dodge_Horizontal_Param, paramsVH.x);
        owner.animator.SetFloat(AnimationParams.Input_Dodge_Vertical_Param, paramsVH.y);

        owner.animator.CrossFadeInFixedTime(AnimationParams.Dodge_State, .1f);

        currentDodgeState = EDodgeState.OnDodge;
        owner.isApplyDodgeMove = true;
        onDodgeEvent?.Invoke();
        while (elapsedTime < dodgeDuration)
        {
            elapsedTime += Time.deltaTime;
            float displaceDistance = dodgeCurve.Evaluate(elapsedTime);
            Vector3 displacement = direction * displaceDistance;
            owner.characterController.Move(displacement * speed * Time.deltaTime);
            currentDodgeTime += Time.deltaTime;
            yield return null;
        }

        if (owner.targetingComponent.target != null) owner.transform.LookAt(owner.targetingComponent.target);

        currentDodgeState = EDodgeState.OffDodge;
        owner.isApplyDodgeMove = false;
        offDodgeEvent?.Invoke();
        // End Dodge AI
        //ownerAI.navMeshAgent.enabled = false;
        ownerAI.currentBehaviorState = EBehaviorState.Finished;
        Debug.Log("End Dodge");
    }
    IEnumerator DashAI(Vector3 dashDirection, Vector2 paramsVH)
    {
        //Initialized before Start
        float elapsedTime = 0.0f;
        Vector3 direction = dashDirection;
        direction.Normalize();

        float speed = dodgeDistance / dodgeDuration;

        //Start Dodge AI
        CharacterAI ownerAI = owner as CharacterAI;

        ownerAI.currentBehaviorState = EBehaviorState.Executing;
        //ownerAI.navMeshAgent.enabled = false;

        //if (owner.targetingComponent.target != null) owner.lookAtComponent.LookAtEnemy(owner.targetingComponent.target.transform.position, .1f);

        //..
        //owner.animator.SetFloat(AnimationParams.Input_Horizontal_Dash_Param, paramsVH.x);
        //owner.animator.SetFloat(AnimationParams.Input_Vertical_Dash_Param, paramsVH.y);

        owner.animator.speed = dodgeSpeed;
        owner.animator.CrossFadeInFixedTime(AnimationParams.Dash_State, .1f);

        onDodgeEvent?.Invoke();
        while (elapsedTime < dodgeDuration && owner.hitReactionComponent.currentHitReactionState != EHitReactionState.OnHit)
        {
            elapsedTime += Time.deltaTime;
            float displaceDistance = dodgeCurve.Evaluate(elapsedTime);
            Vector3 displacement = direction * displaceDistance * dashFactor;
            owner.characterController.Move(displacement * speed * Time.deltaTime);
            currentDodgeTime += Time.deltaTime;
            yield return null;
        }

       

        offDodgeEvent?.Invoke();
        owner.animator.speed = 1.0f;

        // End Dodge AI
        //ownerAI.navMeshAgent.enabled = false;
        ownerAI.currentBehaviorState = EBehaviorState.Finished;
        Debug.Log("End Dash");
    }
    public void StartTeleport(Vector3 teleportTarget)
    {
        //Break Lines
        if (owner.hitReactionComponent.currentHitReactionState == EHitReactionState.OnHit
            || owner.hitReactionComponent.currentRecoverState == ERecoverState.OnRecover) return;
        //
        if (C_Teleport != null)
        {
            StopCoroutine(C_Teleport);
        }
        C_Dodge = StartCoroutine(Dodge(teleportTarget));
    }
    IEnumerator Teleport(Vector3 teleportTarget)
    {
        onTeleportEvent?.Invoke();

        yield return new WaitForSeconds(teleportDuration);

        offTeleportEvent?.Invoke();
    }
    public void ResetDodge()
    {
        isDodging = false;
        currentDodgeState = EDodgeState.OffDodge;
    }
    #endregion
}
