
using Training;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public Character owner;
    public void Awake()
    {
        owner = GetComponent<Character>();
    }
    #region Dodge
    public void Anim_OnDodge()
    {
        owner.dodgeComponent.currentDodgeState = EDodgeState.OnDodge;
        owner.isApplyDodgeMove = true;
    }
    public void Anim_OffDodge()
    {
        owner.dodgeComponent.currentDodgeState = EDodgeState.OffDodge;
        owner.isApplyDodgeMove = false;

    }
    #endregion
    #region HitReaction
    public void Anim_OnBlock(float blockSide)
    {
        owner.hitReactionComponent.currentHitReactionState = EHitReactionState.OnHit;
        owner.animator.SetFloat(AnimationParams.BlockSide_Param, blockSide);
    }
    public void Anim_OffBlock()
    {
        owner.hitReactionComponent.currentHitReactionState = EHitReactionState.OffHit;
    }
    public void Anim_OnHit(int staggerSign)
    {
        owner.hitReactionComponent.currentHitReactionState = EHitReactionState.OnHit;

        if(staggerSign == 1)
        {
            owner.isOnStagger = true;
        }
        //owner.hitReactionComponent.currentRecoverState = ERecoverState.OffRecover;
        //if (owner as CharacterPlayer)
        //{
        //    (owner as CharacterPlayer).sMController.SwitchState((owner as CharacterPlayer).sMController.hitReactionState);
        //}
    }
    public void Anim_OffHit()
    {
        owner.hitReactionComponent.currentHitReactionState = EHitReactionState.OffHit;
        owner.isOnStagger = false;
        //owner.hitReactionComponent.ResetHitReaction();
        //if (owner as CharacterAI)
        //{
        //    (owner as CharacterAI).ResetBehaviorState();
        //}
    }
    #endregion

    #region ComboAttack
    public void Anim_OnComboBegin(int currentComboIndex)
    {
        owner.hitReactionComponent.currentListHitReactionData = owner.comboComponent.currentComboData.comboData.listHitReactionDatas[currentComboIndex];
        owner.comboComponent.currentComboState = EComboState.Playing;
        owner.currentCombatState = ECombatState.Attacking;
        //owner.animator.applyRootMotion = true;
        owner.isApplyAnimationMove = true;
        owner.freeflowComponent.StartLOL();
    }
    public void Anim_OnComboEnd(int end)
    {
        if (end == 0)
        {
            owner.comboComponent.currentComboState = EComboState.Stop;
            //owner.currentCombatState = ECombatState.None;
        }
        else if (end == 1)
        {

            owner.comboComponent.ResetCombo();
            owner.comboComponent.currentComboState = EComboState.Finish;
        }
        //owner.animator.applyRootMotion = false;
    }
    public void Anim_OnAttack(int hitReactionIndex) 
    {
        if (hitReactionIndex > owner.hitReactionComponent.currentListHitReactionData.hitReactionDatas.Count - 1) hitReactionIndex = 0;
        owner.hitReactionComponent.currentHitReactionData = owner.hitReactionComponent.currentListHitReactionData.hitReactionDatas[hitReactionIndex];
        owner.hitTraceComponent.BeginTrace();

    }

    public void Anim_OffAttack()
    {
        owner.hitTraceComponent.OffTrace();

    }


    public void Anim_SetAirborneTime(float airborneTime)
    {
        //owner.hitReactionComponent.SetAirborneTime(airborneTime);
    }

    public void Anim_OnFreeflow(int freeflowIndex)
    {
        //owner.freeflowComponent.StartLOL();
    }
    #endregion

    #region Death
    public void Anim_OnDeath()
    {
        owner.gameObject.SetActive(false);
    }
    #endregion

    public void Anim_OnFinisherBegin()
    {

    }
}