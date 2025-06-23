using System.Collections.Generic;
using UnityEngine;
using System.Collections;


namespace Training
{
    #region Actions
    [System.Serializable]
    public class ActionData
    {
        public string name;
        public Action action;
        public int priority;
        public Condition condition;
        public float delayTimeAfterFinish;

        public float currentCooldown;
    }
    [System.Serializable]
    public class SkirmishActionData
    {

        public string name;
        public ComboAttackAction skirmishAction;
        public int priority;
        public Condition condition;
        public float delayTimeAfterFinish;

        public float currentCooldown;
    }
    #endregion
    #region ComboPriority

    [System.Serializable]
    public enum EComboPriorityState
    {
        Executing,
        Finish,
        None
    }
    [System.Serializable]
    public class TraceActionData
    {
        public string actionName;
        public int index;
        public SO_Behavior action;

        public float currentCooldown;
        public float Cooldown;
    }
    [System.Serializable]
    public class ComboPriorityData
    {
        public string comboName;
        public int priority;
        public float timeDelayAfterCombo;
        public List<TraceActionData> traceActionDatas; 
        public Condition condition;
    }
    [System.Serializable]
    public class ComboPriorityDatas
    {
        public List<ComboPriorityData> listComboPriority;
    }
    #endregion
    public class BehaviorDecesion : MonoBehaviour
    {
        [Header("Enemy Owner")]
        public CharacterAI ownerAI;
        [Header("Available Normal Actions")]
        public List<ActionData> actionDatas;
        public List<SkirmishActionData> skirmishActionDatas;
        [Header("Current Normal Actions")]
        public ActionData currentActionData;
        public SkirmishActionData currentSkirmishActionData;

        [Header("Available Combo Actions Priority")]
        public EComboPriorityState currentComboPriorityState;
        public ComboPriorityDatas comboPriorityDatas;
        public List<TraceActionData> priorityCombos;


        [Header("Combo Delay")]
        public float timeDelayAfterCombo = 0.0f;
        public float comboPriorityDelay = 2.5f;
        public float timeInput = 0.0f;

        public Coroutine C_ExecuteBehavior;
        public Coroutine C_ExecuteComboPriority;


        private void Awake()
        {
            ownerAI = GetComponent<CharacterAI>();
        }
        public void ResetAllActionsState()
        {
            //Normal Actions
            if (actionDatas.Count > 0)
            {
                foreach (var actionData in actionDatas)
                {
                    actionData.currentCooldown = 0.0f;
                }
            }
            if (skirmishActionDatas.Count > 0)
            {
                foreach (var skirmishActionData in skirmishActionDatas)
                {
                    skirmishActionData.currentCooldown = 0.0f;
                }
            }
            //Combo Priority Actions
            if (comboPriorityDatas.listComboPriority.Count > 0)
            {
                foreach (var comboPriorityData in comboPriorityDatas.listComboPriority)
                {
                    foreach (var action in comboPriorityData.traceActionDatas)
                    {
                        action.currentCooldown = 0.0f;
                    }
                }
            }
        }

        private void Start()
        {
            ResetAllActionsState();
            StartExecuteBehaviors();

        }
        #region ComboPriority
        public void StartExecuteComboPriority()
        {
            if (C_ExecuteComboPriority != null)
            {
                StopCoroutine(C_ExecuteComboPriority);
            }
            C_ExecuteComboPriority = StartCoroutine(ComboExecute());

        }
        IEnumerator ComboExecute()
        {
            yield return new WaitWhile(() => ownerAI.currentBehaviorState == EBehaviorState.Executing);
            ownerAI.isDoingCombo = true;
            currentComboPriorityState = EComboPriorityState.Executing;
            while (priorityCombos.Count > 0)
            {
                yield return new WaitWhile(() => ownerAI.currentBehaviorState == EBehaviorState.Executing);
                priorityCombos[0].action.Execute(ownerAI);
                StartCoroutine(StartCooldown(priorityCombos[0]));
                priorityCombos.RemoveAt(0);

                
                yield return new WaitWhile(() => ownerAI.currentBehaviorState == EBehaviorState.Executing);
            }

            yield return new WaitForSeconds(timeDelayAfterCombo);
            ownerAI.isDoingCombo = false;
            ResetPriorityCombo();
        }
        public bool isExecuteComboPriority()
        {
            if (ownerAI.currentBehaviorState != EBehaviorState.Executing)
            {
                priorityCombos = GetValidComboPriority();
                if (priorityCombos.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public List<TraceActionData> GetValidComboPriority()
        {
            List<TraceActionData> comboOutput = new List<TraceActionData>();
            if (comboPriorityDatas.listComboPriority.Count > 0)
            {

                //So sánh từ Combo có độ ưu tiên cao nhất (trong Unity Editor, nó đã được đặt lên trên đầu)
                foreach (var comboPriorityData in comboPriorityDatas.listComboPriority)
                {
                    if (isComboPriorityValid(comboPriorityData))
                    {
                        Debug.Log("Executing: " + comboPriorityData.comboName);
                        timeDelayAfterCombo = comboPriorityData.timeDelayAfterCombo;
                        foreach (var traceActionData in comboPriorityData.traceActionDatas)
                        {
                            comboOutput.Add(traceActionData);
                        }
                        break;
                    }
                }
            }
            return comboOutput;
        }
        public bool isComboPriorityValid(ComboPriorityData comboPriorityData)
        {
            //Check Condition First
            if (!EvaluateCondition(comboPriorityData.condition)) return false;
            //Then check number of Ready Action
            int numberOfReadyAction = 0;
            foreach (var actionCombo in comboPriorityData.traceActionDatas)
            {
                if(actionCombo.currentCooldown <= 0.0f)numberOfReadyAction++;
            }
            Debug.Log(numberOfReadyAction);
           
            return (numberOfReadyAction == comboPriorityData.traceActionDatas.Count) ? true : false;
        }


        public void ResetPriorityCombo()
        {
            ownerAI.currentBehaviorState = EBehaviorState.Finished;
            currentComboPriorityState = EComboPriorityState.Finish;
            priorityCombos.Clear();
        }
        #endregion
        public void StartExecuteBehaviors()
        {
            if(C_ExecuteBehavior != null)
            {
                StopCoroutine(C_ExecuteBehavior);
            }

            C_ExecuteBehavior = StartCoroutine(ExecuteBehaviors());
        }
        IEnumerator ExecuteBehaviors()
        {
            while(ownerAI.atributeComponent.currentHP > 0)
            {
                yield return new WaitWhile(() => ownerAI.currentBehaviorState == EBehaviorState.Executing);
                EvaluateAndExecuteBehavior();
                yield return new WaitWhile(() => ownerAI.currentBehaviorState == EBehaviorState.Executing);
            }
        }
        public void EvaluateAndExecuteBehavior()
        {
            if (ownerAI.hitReactionComponent.currentHitReactionState == EHitReactionState.OnHit
                || ownerAI.hitReactionComponent.currentRecoverState == ERecoverState.OnRecover
                || currentComboPriorityState == EComboPriorityState.Executing
                || ownerAI.isDoingCombo) return;

            ////Multiple Action
            //if (comboPriorityDatas.listComboPriority.Count > 0)
            //{
            //    if (Time.time > timeInput + comboPriorityDelay && currentComboPriorityState != EComboPriorityState.Executing)
            //    {
            //        timeInput = Time.time;
            //        if (isExecuteComboPriority())
            //        {
            //            StartExecuteComboPriority();
            //            return;
            //        }
            //    }
            //}


                //Single Action
                 List <ActionData> suffleActions = new List<ActionData>();
                List<SkirmishActionData> suffleSkirmishActions = new List<SkirmishActionData>();

                //Check and Get All Actions - Skill Actions - Skirmush Actions has valid conditions and 0s Cooldown
                if (actionDatas.Count > 0)
                {
                    foreach (var actionData in actionDatas)
                    {
                        if (EvaluateCondition(actionData.condition))
                        {
                            if (actionData.currentCooldown <= 0.0f)
                            {
                            Debug.Log("Action Added is: " + actionData.name);
                                suffleActions.Add(actionData);
                            }
                        }
                    }
                }
                if (skirmishActionDatas.Count > 0)
                {
                    foreach (var skirmishActionData in skirmishActionDatas)
                    {
                        if (EvaluateCondition(skirmishActionData.condition))
                        {
                            if (skirmishActionData.currentCooldown <= 0.0f)
                            {
                                suffleSkirmishActions.Add(skirmishActionData);
                            }
                        }
                    }
                }


                //If Suffle Action - Skirmish Action - Skill Action have value then we EXECUTE the 
                //action has HIGHEST priorty value

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

                    currentSkirmishActionData = actionOutput;

                    StartCoroutine(StartCooldown(currentSkirmishActionData));

                    currentSkirmishActionData.skirmishAction.Execute(ownerAI);
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

                    currentActionData = actionOutput;

                    StartCoroutine(StartCooldown(currentActionData));

                    currentActionData.action.Execute(ownerAI);
                    return;

                }
            
        }
        #region ActionCooldown
        public IEnumerator StartCooldown(TraceActionData traceActionData)
        {
            //Start Cooldown
            traceActionData.currentCooldown = traceActionData.Cooldown;

            while (traceActionData.currentCooldown >= 0.0f)
            {
                traceActionData.currentCooldown -= Time.deltaTime;
                yield return null;
            }
            traceActionData.currentCooldown = 0.0f;
        }
        public IEnumerator StartCooldown(ActionData actionData)
        {
            //Start Cooldown
            actionData.currentCooldown = actionData.action.Cooldown;

            while(actionData.currentCooldown >= 0.0f)
            {
                actionData.currentCooldown -= Time.deltaTime;
                yield return null;
            }
            actionData.currentCooldown = 0.0f;
        }
        public IEnumerator StartCooldown(SkirmishActionData skirmishActionData)
        {
            //Start Cooldown
            skirmishActionData.currentCooldown = skirmishActionData.skirmishAction.Cooldown;

            while (skirmishActionData.currentCooldown >= 0.0f)
            {
                skirmishActionData.currentCooldown -= Time.deltaTime;
                yield return null;
            }
            skirmishActionData.currentCooldown = 0.0f;

        }
        #endregion
        #region CheckValue
        private bool EvaluateCheckBool(BoolValue boolValue)
        {
            bool variableValue = GetBoolValue(boolValue.variableType);
            switch (boolValue.compareType)
            {
                case ECompareType.Equal:
                    return variableValue == boolValue.value;

                default:
                    return false;
            }
        }
        private bool EvaluateCheckInt(IntValue intValue)
        {
            int variableValue = GetIntValue(intValue.variableType);
            switch (intValue.compareType)
            {
                case ECompareType.Equal:
                    return variableValue == intValue.value;

                case ECompareType.GreaterThanOrEqual:
                    return variableValue > intValue.value 
                        || variableValue == intValue.value;

                case ECompareType.GreaterThan:
                    return variableValue > intValue.value;

                case ECompareType.LessThanOrEqual:
                    return variableValue < intValue.value 
                        || variableValue == intValue.value;

                case ECompareType.LessThan:
                    return variableValue < intValue.value;

                default:
                    return false;
            }
        }
        private bool EvaluateCheckFloat(FloatValue floatValue)
        {
            float variableValue = GetFloatValue(floatValue.variableType);
            switch (floatValue.compareType)
            {
                case ECompareType.Equal:
                    return Mathf.Approximately(variableValue, floatValue.value);

                case ECompareType.GreaterThan:
                    return variableValue > floatValue.value;

                case ECompareType.GreaterThanOrEqual:
                    return variableValue > floatValue.value 
                        || Mathf.Approximately(variableValue, floatValue.value);

                case ECompareType.LessThan:
                    return variableValue < floatValue.value;

                case ECompareType.LessThanOrEqual:
                    return variableValue < floatValue.value 
                        || Mathf.Approximately(variableValue, floatValue.value);

                default:
                    return false;
            }
        }
        public  bool EvaluateCondition(Condition condition)
        {
            foreach (var checkBool in condition.boolValue)
            {
                if (!EvaluateCheckBool(checkBool)) return false;
            }

            foreach (var checkInt in condition.intValue)
            {
                if (!EvaluateCheckInt(checkInt)) return false;
            }

            foreach (var checkFloat in condition.floatValue)
            {
                if (!EvaluateCheckFloat(checkFloat)) return false;
            }

            return true;
        }
        private bool GetBoolValue(EBoolType type)
        {
            switch (type)
            {
                case EBoolType.Ground:
                    {
                        return ownerAI.characterController.isGrounded;
                    }
                case EBoolType.TargetGround:
                    {
                        Transform target = ownerAI.targetingComponent.target;
                        return false;
                            //(target != null
                            //&& target.isGround);
                    }
                case EBoolType.HasTarget:
                    {
                        return ownerAI.targetingComponent.target != null;
                    }

                case EBoolType.Attackable:
                    {
                        return (ownerAI.comboComponent.currentComboState != EComboState.Playing 
                            && ownerAI.currentCombatState != ECombatState.BeingBeaten);
                    }
                case EBoolType.Jumpable:
                    {
                        //Character target = ownerAI.targetingComponent.target;
                        return false;
                            //!target.isGround;
                    }
                case EBoolType.Dodgeable:
                    {
                        return false;
                        //Character target = ownerAI.targetingComponent.target;
                        //return (target != null
                        //    && (target.currentCombatState == ECombatState.Attacking || ownerAI.hitReactionComponent.currentHitReactionState == EHitReactionState.OnHit));
                            /*&& enemyOwner.currentCombatState != ECombatState.Attacking*/
                            //|| (ownerAI.currentCombatState == ECombatState.BeingBeaten 
                            //&& ownerAI.skillComponent.currentSkillState == ESkillState.Finished);
                    }
                case EBoolType.Blockable:
                    {
                        return false;
                        //Character target = ownerAI.targetingComponent.target;
                        //return (target != null
                        //    && target.currentCombatState == ECombatState.Attacking);
                    }
                case EBoolType.Shootable:
                    {
                        Vector3 startPos = transform.position; // Vị trí nhân vật
                        Vector3 targetPos = ownerAI.targetingComponent.target.transform.position; // Vị trí mục tiêu
                        Vector3 direction = (targetPos - startPos).normalized; // Hướng từ nhân vật đến mục tiêu
                        float distance = Vector3.Distance(startPos, targetPos); // Khoảng cách giữa nhân vật và mục tiêu
                        int layerMask = LayerMask.GetMask("Default"); // Chỉ kiểm tra với Layer Default

                        // Vẽ đường raycast trong Scene để kiểm tra
                        Debug.DrawRay(startPos, direction * distance, Color.red);

                        if (Physics.Raycast(startPos, direction, out RaycastHit hit, distance, layerMask))
                        {
                            Debug.Log("Raycast chạm vào: " + hit.collider.gameObject.name);
                            return false;
                        }
                        else
                        {
                            Debug.Log("Raycast không chạm vào bất cứ GameObject nào trong layer Default");
                            return true;
                        }

                    }
                case EBoolType.Walkable:
                    //if (!ownerAI.targetingComponent.target.isSprint) 
                        return true;
                    //else return false;
                case EBoolType.Runable:
                    //if (ownerAI.targetingComponent.target.isSprint) 
                        return true;
                    //else return false;
                default:
                    return false;
            }
        }
        private int GetIntValue(EIntType type)
        {
            switch (type)
            {
                default:
                    return 0;
            }
        }
        private float GetFloatValue(EFloatType type)
        {
            switch (type)
            {
                case EFloatType.Distance:
                    {
                        if (ownerAI.GetComponent<TargetingComponent>() && ownerAI.targetingComponent.target != null)
                        {
                            return Utilities.Instance.DistanceCalculate(ownerAI.transform.position, ownerAI.targetingComponent.target.transform.position, true);
                        }
                            return 0.0f;
                    }
                case EFloatType.AngleToTarget:
                    {
                        Vector3 targetDirection = ownerAI.targetingComponent.target.transform.position - ownerAI.transform.position;
                        targetDirection.Normalize();
                        float angle = Vector3.Angle(ownerAI.transform.forward, targetDirection);
                        //Debug.Log("Angle to Target: " + angle);
                        return angle;
                    }

                case EFloatType.CurrentHP:
                    {
                        return ownerAI.atributeComponent.currentHP * 100.0f / ownerAI.atributeComponent.maxHP;
                    }

                default:
                    return 0.0f;
            }
        }
        #endregion

        public void ResetBehavior()
        {
            ownerAI.currentBehaviorState = EBehaviorState.Finished;
        }
    }
}
//public void Rotation()
//{
//    // Tính toán hướng từ Enemy tới Player
//    Vector3 directionToPlayer = (enemyOwner.targetingComponent.target.transform.position - transform.position).normalized;

//    // Tạo góc quay target để Enemy hướng về phía Player
//    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

//    // Quay Enemy từ từ về hướng Player bằng cách nội suy giữa góc hiện tại và target góc
//    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3.0f * Time.deltaTime);
//}
