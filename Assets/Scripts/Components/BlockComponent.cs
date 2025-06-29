using System.Collections;
using Training;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public enum EBlockState
{
    None,
    OnBlock,
    OnCounterAttackChance,
    OffBlock
}
public class BlockComponent : MonoBehaviour
{
    public Character owner;
    public EBlockState currentBlockState = EBlockState.None;



    Coroutine C_BlockAI;

    private void Awake()
    {
        owner = GetComponent<Character>();
    }
    public void StartBlock(float blockTime)
    {
        if (C_BlockAI != null)StopCoroutine(C_BlockAI);
        C_BlockAI = StartCoroutine(BlockAI(blockTime));

    }
    IEnumerator BlockAI(float blockTime)
    {
        owner.isBlock = true;
        yield return new WaitForSeconds(blockTime);
    }
    ////Player Block
    //public void StartBlock()
    //{
    //    isBlocking = true;
    //    owner.animator.CrossFadeInFixedTime(AnimationParams.Block_Start_State, 0.1f);
    //    owner.animator.SetBool(AnimationParams.Block_Param, true);
    //}
    //public void EndBlockProactive()
    //{
    //    isBlocking = false;
    //    owner.animator.CrossFadeInFixedTime(AnimationParams.Block_End_State, 0.1f);
    //    owner.animator.SetBool(AnimationParams.Block_Param, false);
    //}
    //public void EndBlockPassive()
    //{
    //    isBlocking = false;
    //    owner.blockComponent.currentBlockState = EBlockState.OffBlock;
    //    owner.animator.SetBool(AnimationParams.Block_Param, false);
    //}
    //public void ResetBlock()
    //{
    //    currentBlockState = EBlockState.OffBlock;
    //    EndBlockPassive();
    //}

    ////AI Block
    //public void StartBlockAI(float duration)
    //{
    //    if (C_BlockAI != null)
    //    {
    //        StopCoroutine(C_BlockAI);
    //    }
    //    C_BlockAI = StartCoroutine(BlockAI(duration));
    //}
    //IEnumerator BlockAI(float duration)
    //{

    //    CharacterAI ownerAI = owner as CharacterAI;
    //    ownerAI.currentBehaviorState = Training.EBehaviorState.Executing;

    //    if (owner.currentWeapon.currentWeaponState != EWeaponState.Equip) owner.characterRigComponent.SwitchWeapon(owner.characterRigComponent.weapon1Prefab);
    //    StartBlock();

    //    float elapsedTime = 0.0f;
    //    while (elapsedTime <= duration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        if (owner.targetingComponent.target != null)
    //        {
    //            owner.lookAtComponent.LookAtEnemy(owner.targetingComponent.target.transform.position);
    //        }

    //        yield return null;
    //    }
    //    EndBlockPassive();
    //    ownerAI.currentBehaviorState = Training.EBehaviorState.Finished;

    //}
}