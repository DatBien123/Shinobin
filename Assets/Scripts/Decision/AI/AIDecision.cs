using Training;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIDecision : MonoBehaviour
{
    public CharacterAI ownerAI;

    [Header("Character AI Input Values")]
    public Vector2 move;
    public bool jump;
    public bool sprint;

    private void Awake()
    {
        ownerAI = GetComponent<CharacterAI>();
        ownerAI.animator.SetBool(AnimationParams.Block_Param, ownerAI.isBlock);
    }
    #region Movement
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
    #endregion

    #region Combat
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
}
