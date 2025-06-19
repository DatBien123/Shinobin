using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Training
{
    public enum EComboState
    {
        None = 0,
        Playing = 1,
        Stop,
        Finish
    }

    public class ComboComponent : MonoBehaviour
    {
        [Header("Onwer")]
        public Character owner;

        [Header("Loaded Combos")]
        public List<SO_Combo> loadCombos;
        public int inputIndex = 0;

        [Header("Current Inputs")]
        public float inputDelayTime = .1f;
        private float lastInputTime = 0.0f;
        private EKeystroke keystroke = EKeystroke.None;
        public List<EKeystroke> currentInputs;

        public EComboState currentComboState = EComboState.None;
        public float comboResetTime = 0.5f;

        [Header("Save Combo")]
        public List<AnimationClip> saveCombos;

        [Header("Current Combo Index")]
        public SO_Combo currentComboData;
        public SO_Combo lastComboData;
        public int currentComboIndex = 0;

        Coroutine C_ComboSequence;

        [Header("Enemy Combo")]
        public SO_Combo enemyCombos;
        private void Awake()
        {
            owner = GetComponent<Character>();
            LoadCombo();
        }

        public void LoadCombo()
        {
            if (owner.currentWeapon != null)
            {
                var comboDatas = Resources.LoadAll<SO_Combo>($"Combo/{owner.currentWeapon.weaponType}");
                foreach (var data in comboDatas)
                {
                    loadCombos.Add(data);
                }
            }
            else
            {
                var comboDatas = Resources.LoadAll<SO_Combo>($"Combo/None");
                foreach (var data in comboDatas)
                {
                    loadCombos.Add(data);
                }
            }
        }
        private SO_Combo FilterCombo()
        {
            List<SO_Combo> filterCombos = loadCombos;
            foreach (var combo in filterCombos)
            {
                if (combo.comboData.comboInputs[0] == keystroke && combo.comboData.requiredState == owner.currentState /*&& combo.comboData.isSprint == owner.isSprint*/)
                {
                    if (lastComboData == null || combo.name != lastComboData.name)
                    {
                        lastComboData = combo;
                    }
                    return combo;
                }
            }

            Debug.Log("No combo found");
            return null;
        }

        public void SetCurrentInput(EKeystroke keystroke)
        {
            ////Break Lines
            //if (owner.hitReactionComponent.currentHitReactionState == EHitReactionState.OnHit
            //    || owner.hitReactionComponent.currentRecoverState == ERecoverState.OnRecover) return;

            if (lastInputTime + inputDelayTime <= Time.time)
            {
                currentInputs.Add(keystroke);
                lastInputTime = Time.time;
                this.keystroke = keystroke;
                SaveCombo();
            }
        }

        private void Update()
        {
            //Debug.Log("Filer" + FilterCombos()[0].name);
        }

        public void SaveCombo()
        {
            if (currentComboState == EComboState.Finish && owner.currentCombatState != ECombatState.BeingBeaten)
                currentComboData = FilterCombo();

            if (currentComboData != null)
            {
                if (inputIndex < currentComboData.comboData.comboInputs.Count)
                {
                    saveCombos.Add(currentComboData.comboData.comboClips[inputIndex]);
                    PlayComboSequence();
                    inputIndex++;
                    if (inputIndex >= currentComboData.comboData.comboInputs.Count) inputIndex = 0;
                }

            }

        }

        public void SaveComboEnemy(SO_Combo combo)
        {
            if (currentComboState == EComboState.Finish)
            {
                if ((owner as CharacterAI).currentBehaviorState != EBehaviorState.Executing)
                {
                    ResetCombo();
                    (owner as CharacterAI).currentBehaviorState = EBehaviorState.Executing;
                    currentComboData = combo;

                    for (int i = 0; i < currentComboData.comboData.comboClips.Count; i++)
                    {
                        saveCombos.Add(currentComboData.comboData.comboClips[i]);
                    }

                    PlayComboSequence();
                }
            }
        }
        void PlayComboSequence()
        {
            if (C_ComboSequence != null)
            {
                StopCoroutine(C_ComboSequence);
            }
            C_ComboSequence = StartCoroutine(PlayCombo());
        }
        IEnumerator PlayCombo()
        {
            yield return new WaitWhile(() => currentComboState == EComboState.Playing);
            while (saveCombos.Count > 0)
            {
                owner.animator.CrossFadeInFixedTime(saveCombos[0].name, 0.1f);
                saveCombos.RemoveAt(0);

                yield return new WaitForFixedUpdate();
                yield return new WaitWhile(() => currentComboState == EComboState.Playing);
            }
            yield return new WaitForSeconds(comboResetTime);

            ResetCombo();
        }
        public void ResetCombo()
        {
            //Reset Index
            inputIndex = 0;
            currentComboIndex = 0;
            //Reset List 
            loadCombos.Clear();
            currentInputs.Clear();
            saveCombos.Clear();
            owner.hitTraceComponent.OffTrace();
            LoadCombo();
            owner.currentCombatState = ECombatState.None;
            currentComboState = EComboState.Finish;

            //Navmesh
            if (owner as CharacterAI)
            {
                (owner as CharacterAI).navMeshAgent.enabled = true;
            }

        }
    }
}
