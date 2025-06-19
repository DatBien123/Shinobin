
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Training;
using UnityEngine;

[System.Serializable]
public enum ETaticalState
{

    Probe,
    Skirmish,
    Intro
}

public class AITatical : MonoBehaviour
{
    public CharacterAI ownerAI;

    public AIPerception ownerPerception;

    public BehaviorDecesion behaviorDecesion;

    public ETaticalState currentTaticalState;

    public int attackTimes = 0;
    public float delayBetweenTwoAttack = 2.0f;

    ///---- PArams-----//
    public float maxProbeTime = 5.0f;
    public float maxSkirmishTime = 5.0f;
    public float validDistanceForConvertToProbe = 10.0f;

    Coroutine C_ExecuteIntro;
    Coroutine C_ExecuteProbe;
    Coroutine C_ExecuteSkirmish;
    Coroutine C_ExecuteTatical;


    private void Awake()
    {
        ownerAI = GetComponent<CharacterAI>();
        behaviorDecesion = GetComponent<BehaviorDecesion>();
        //ownerPerception = GetComponent<AIPerception>();
    }
    private void Start()
    {
        StartExecuteIntro();
    }
    public void StartExecuteIntro()
    {
        if (C_ExecuteIntro != null) StopCoroutine(C_ExecuteIntro);
        C_ExecuteIntro = StartCoroutine(ExecuteIntro());
    }
    public void StartExecuteProbe()
    {
        if(C_ExecuteProbe != null)StopCoroutine(C_ExecuteProbe);
        C_ExecuteProbe = StartCoroutine(ExecuteProbe());
    }
    public void StartExecuteSkirmish()
    {
        if (C_ExecuteSkirmish != null)StopCoroutine(C_ExecuteSkirmish);
        C_ExecuteSkirmish = StartCoroutine(ExecuteSkirmish());
    }
    IEnumerator ExecuteProbe()
    {
        float elapsedTime = 0f;
        while (elapsedTime < maxProbeTime)
        {
            if (ownerAI.currentBehaviorState != EBehaviorState.Executing)
            {
                Debug.Log("Executing Probe");
                EvaluateAndExecuteProbeBehavior();
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Finish Probe");

        yield return new WaitWhile(() => ownerAI.currentBehaviorState == EBehaviorState.Executing);
        if (ownerAI.atributeComponent.currentHP > ownerAI.atributeComponent.maxHP / 2) StartExecuteProbe();
        else StartExecuteSkirmish();
    }

    IEnumerator ExecuteSkirmish()
    {
        float elapsedTime = 0f;
        while (elapsedTime < maxSkirmishTime)
        {
            if (ownerAI.currentBehaviorState != EBehaviorState.Executing)
            {
                Debug.Log("Executing Skirmish");
                EvaluateAndExecuteSkirmishBehavior();
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Finish Skirmish");

        yield return new WaitWhile(() => ownerAI.currentBehaviorState == EBehaviorState.Executing);
        if(ownerAI.atributeComponent.currentHP <= ownerAI.atributeComponent.maxHP / 2)StartExecuteSkirmish();
        else StartExecuteProbe();
    }
    IEnumerator ExecuteIntro()
    {
        while (ownerAI.targetingComponent.target == null)
        {
            if (ownerAI.currentBehaviorState != EBehaviorState.Executing)
            {
                Debug.Log("Executing Intro");
                EvaluateAndExecuteIntroBehavior();
            }
            yield return null;
        }
        Debug.Log("Finish Intro");

        yield return new WaitWhile(() => ownerAI.currentBehaviorState == EBehaviorState.Executing);
        StartExecuteProbe();
    }
    public void EvaluateAndExecuteProbeBehavior()
    {

        if (ownerAI.hitReactionComponent.currentHitReactionState == EHitReactionState.OnHit
            || ownerAI.hitReactionComponent.currentRecoverState == ERecoverState.OnRecover
            || behaviorDecesion.currentComboPriorityState == EComboPriorityState.Executing
            || ownerAI.isDoingCombo
            //|| ownerAI.hitReactionComponent.isOnStagger
            || ownerAI.atributeComponent.currentHP <= 0) return;

        List<ActionData> suffleActions = new List<ActionData>();
        List<SkirmishActionData> suffleSkirmishActions = new List<SkirmishActionData>();
        List<SkillActionData> suffleSkillActions = new List<SkillActionData>();

        //Check and Get All Actions - Skill Actions - Skirmush Actions has valid conditions and 0s Cooldown
        if (behaviorDecesion.actionDatas.Count > 0)
        {
            foreach (var actionData in behaviorDecesion.actionDatas)
            {
                if (behaviorDecesion.EvaluateCondition(actionData.condition))
                {
                    if (actionData.currentCooldown <= 0.0f && actionData.taticalState == ETaticalState.Probe)
                    {
                        Debug.Log("Action Added is: " + actionData.name);
                        suffleActions.Add(actionData);
                    }
                }
            }
        }
        if (behaviorDecesion.skirmishActionDatas.Count > 0)
        {
            foreach (var skirmishActionData in behaviorDecesion.skirmishActionDatas)
            {
                if (behaviorDecesion.EvaluateCondition(skirmishActionData.condition))
                {
                    if (skirmishActionData.currentCooldown <= 0.0f && skirmishActionData.taticalState == ETaticalState.Probe)
                    {
                        suffleSkirmishActions.Add(skirmishActionData);
                    }
                }
            }
        }
        if (behaviorDecesion.skillActionDatas.Count > 0)
        {
            foreach (var skillActionData in behaviorDecesion.skillActionDatas)
            {
                if (behaviorDecesion.EvaluateCondition(skillActionData.condition))
                {
                    if (skillActionData.currentCooldown <= 0.0f && skillActionData.taticalState == ETaticalState.Probe)
                    {
                        suffleSkillActions.Add(skillActionData);
                    }
                }
            }
        }

        //If Suffle Action - Skirmish Action - Skill Action have value then we EXECUTE the 
        if (suffleActions.Count > 0)
        {
            //Get Action has highest priority
            ActionData actionOutput = new ActionData();
            int maxPriority = 0;
            foreach (var action in suffleActions)
            {
                if (action.priority >= maxPriority)
                {
                    maxPriority = action.priority;
                }
            }
            foreach (var action in suffleActions)
            {
                if (action.priority == maxPriority)
                {
                    actionOutput = action;
                    break;
                }
            }

            behaviorDecesion.currentActionData = actionOutput;

            StartCoroutine(behaviorDecesion.StartCooldown(behaviorDecesion.currentActionData));

            behaviorDecesion.currentActionData.action.Execute(ownerAI);
            return;
        }
        //action has HIGHEST priorty value
        if (suffleSkillActions.Count > 0)
        {
            //Get Action has highest priority
            SkillActionData actionOutput = new SkillActionData();
            int maxPriority = 0;
            foreach (var skillAction in suffleSkillActions)
            {
                if (skillAction.priority >= maxPriority)
                {
                    maxPriority = skillAction.priority;
                }
            }
            foreach (var skillAction in suffleSkillActions)
            {
                if (skillAction.priority == maxPriority)
                {
                    actionOutput = skillAction;
                    break;
                }
            }

            behaviorDecesion.currentSkillActionData = actionOutput;

            StartCoroutine(behaviorDecesion.StartCooldown(behaviorDecesion.currentSkillActionData));

            behaviorDecesion.currentSkillActionData.skillAction.Execute(ownerAI);
            return;
        }
        if (suffleSkirmishActions.Count > 0)
        {
            //Get Action has highest priority
            SkirmishActionData actionOutput = new SkirmishActionData();
            int maxPriority = 0;
            foreach (var skirmishAction in suffleSkirmishActions)
            {
                if (skirmishAction.priority >= maxPriority)
                {
                    maxPriority = skirmishAction.priority;
                }
            }
            foreach (var skirmishAction in suffleSkirmishActions)
            {
                if (skirmishAction.priority == maxPriority)
                {
                    actionOutput = skirmishAction;
                    break;
                }
            }

            behaviorDecesion.currentSkirmishActionData = actionOutput;

            StartCoroutine(behaviorDecesion.StartCooldown(behaviorDecesion.currentSkirmishActionData));

            behaviorDecesion.currentSkirmishActionData.skirmishAction.Execute(ownerAI);
            return;
        }


    }
    public void EvaluateAndExecuteSkirmishBehavior()
    {
        if (ownerAI.hitReactionComponent.currentHitReactionState == EHitReactionState.OnHit
            || ownerAI.hitReactionComponent.currentRecoverState == ERecoverState.OnRecover
            || behaviorDecesion.currentComboPriorityState == EComboPriorityState.Executing
            || ownerAI.isDoingCombo
            //|| ownerAI.hitReactionComponent.isOnStagger
            || ownerAI.atributeComponent.currentHP <= 0) return;

        //Multiple Action
        if (behaviorDecesion.comboPriorityDatas.listComboPriority.Count > 0 && ownerAI.atributeComponent.currentHP <= ownerAI.atributeComponent.maxHP)
        {
            if (Time.time > behaviorDecesion.timeInput + behaviorDecesion.comboPriorityDelay && behaviorDecesion.currentComboPriorityState != EComboPriorityState.Executing)
            {
                behaviorDecesion.timeInput = Time.time;
                if (behaviorDecesion.isExecuteComboPriority())
                {
                    behaviorDecesion.StartExecuteComboPriority();
                    return;
                }
            }
        }

        List<ActionData> suffleActions = new List<ActionData>();
        List<SkirmishActionData> suffleSkirmishActions = new List<SkirmishActionData>();
        List<SkillActionData> suffleSkillActions = new List<SkillActionData>();

        //Check and Get All Actions - Skill Actions - Skirmush Actions has valid conditions and 0s Cooldown
        if (behaviorDecesion.actionDatas.Count > 0)
        {
            foreach (var actionData in behaviorDecesion.actionDatas)
            {
                if (behaviorDecesion.EvaluateCondition(actionData.condition))
                {
                    if (actionData.currentCooldown <= 0.0f && actionData.taticalState == ETaticalState.Skirmish)
                    {
                        Debug.Log("Action Added is: " + actionData.name);
                        suffleActions.Add(actionData);
                    }
                }
            }
        }
        if (behaviorDecesion.skirmishActionDatas.Count > 0)
        {
            foreach (var skirmishActionData in behaviorDecesion.skirmishActionDatas)
            {
                if (behaviorDecesion.EvaluateCondition(skirmishActionData.condition))
                {
                    if (skirmishActionData.currentCooldown <= 0.0f && skirmishActionData.taticalState == ETaticalState.Skirmish)
                    {
                        suffleSkirmishActions.Add(skirmishActionData);
                    }
                }
            }
        }
        if (behaviorDecesion.skillActionDatas.Count > 0)
        {
            foreach (var skillActionData in behaviorDecesion.skillActionDatas)
            {
                if (behaviorDecesion.EvaluateCondition(skillActionData.condition))
                {
                    if (skillActionData.currentCooldown <= 0.0f && skillActionData.taticalState == ETaticalState.Skirmish)
                    {
                        suffleSkillActions.Add(skillActionData);
                    }
                }
            }
        }

        //If Suffle Action - Skirmish Action - Skill Action have value then we EXECUTE the 
        //action has HIGHEST priorty value
        if (suffleSkillActions.Count > 0)
        {
            //Get Action has highest priority
            SkillActionData actionOutput = new SkillActionData();
            int maxPriority = 0;
            foreach (var skillAction in suffleSkillActions)
            {
                if (skillAction.priority >= maxPriority)
                {
                    maxPriority = skillAction.priority;
                }
            }
            foreach (var skillAction in suffleSkillActions)
            {
                if (skillAction.priority == maxPriority)
                {
                    actionOutput = skillAction;
                    break;
                }
            }

            behaviorDecesion.currentSkillActionData = actionOutput;

            StartCoroutine(behaviorDecesion.StartCooldown(behaviorDecesion.currentSkillActionData));

            behaviorDecesion.currentSkillActionData.skillAction.Execute(ownerAI);

            attackTimes++;

            return;
        }
        if (suffleSkirmishActions.Count > 0)
        {
            //Get Action has highest priority
            SkirmishActionData actionOutput = new SkirmishActionData();
            int maxPriority = 0;
            foreach (var skirmishAction in suffleSkirmishActions)
            {
                if (skirmishAction.priority >= maxPriority)
                {
                    maxPriority = skirmishAction.priority;
                }
            }
            foreach (var skirmishAction in suffleSkirmishActions)
            {
                if (skirmishAction.priority == maxPriority)
                {
                    actionOutput = skirmishAction;
                    break;
                }
            }

            behaviorDecesion.currentSkirmishActionData = actionOutput;

            StartCoroutine(behaviorDecesion.StartCooldown(behaviorDecesion.currentSkirmishActionData));

            behaviorDecesion.currentSkirmishActionData.skirmishAction.Execute(ownerAI);

            attackTimes++;

            return;
        }
        if (suffleActions.Count > 0)
        {
            //Get Action has highest priority
            ActionData actionOutput = new ActionData();
            int maxPriority = 0;
            foreach (var action in suffleActions)
            {
                if (action.priority >= maxPriority)
                {
                    maxPriority = action.priority;
                }
            }
            foreach (var action in suffleActions)
            {
                if (action.priority == maxPriority)
                {
                    actionOutput = action;
                    break;
                }
            }

            behaviorDecesion.currentActionData = actionOutput;

            StartCoroutine(behaviorDecesion.StartCooldown(behaviorDecesion.currentActionData));

            behaviorDecesion.currentActionData.action.Execute(ownerAI);

            return;

        }

    }
    public void EvaluateAndExecuteIntroBehavior()
    {
        //if (ownerAI.hitReactionComponent.isOnStagger) return;
        List<ActionData> suffleActions = new List<ActionData>();

        //Check and Get All Actions - Skill Actions - Skirmush Actions has valid conditions and 0s Cooldown
        if (behaviorDecesion.actionDatas.Count > 0)
        {
            foreach (var actionData in behaviorDecesion.actionDatas)
            {
                if (behaviorDecesion.EvaluateCondition(actionData.condition))
                {
                    if (actionData.currentCooldown <= 0.0f && actionData.taticalState == ETaticalState.Intro)
                    {
                        Debug.Log("Action Added is: " + actionData.name);
                        suffleActions.Add(actionData);
                    }
                }
            }
        }
        if (suffleActions.Count > 0)
        {
            //Get Action has highest priority
            ActionData actionOutput = new ActionData();
            int maxPriority = 0;
            foreach (var action in suffleActions)
            {
                if (action.priority >= maxPriority)
                {
                    maxPriority = action.priority;
                }
            }
            foreach (var action in suffleActions)
            {
                if (action.priority == maxPriority)
                {
                    actionOutput = action;
                    break;
                }
            }

            behaviorDecesion.currentActionData = actionOutput;

            StartCoroutine(behaviorDecesion.StartCooldown(behaviorDecesion.currentActionData));

            behaviorDecesion.currentActionData.action.Execute(ownerAI);

            return;

        }

    }



}