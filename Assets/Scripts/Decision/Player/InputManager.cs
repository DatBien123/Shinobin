using System.Collections;
using Training;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    private PlayerControls playerControls;
    public CharacterPlayer player;

    public float blockTime = 0.0f;
    void Awake()
    {
        playerControls = new PlayerControls();
        player = GetComponent<CharacterPlayer>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InputBinding();
    }
    private void Update()
    {
        if (player.isBlock)
        {
            blockTime += Time.deltaTime;
            player.animator.SetFloat(AnimationParams.BlockTime_Param, blockTime);
        }
        else
        {
            blockTime = 0.0f;
            if(player.animator.GetFloat(AnimationParams.BlockTime_Param) != 0.0f)
                player.animator.SetFloat(AnimationParams.BlockTime_Param, blockTime);
        }
    }
    void InputBinding()
    {
        //Camera
        playerControls.Camera.Look.performed += Look;
        playerControls.Camera.Look.canceled += Look;

        //Player Movement
        playerControls.Movement.Move.performed += Move;
        playerControls.Movement.Move.canceled += Move;

        playerControls.Movement.Sprint.performed += Sprint;
        playerControls.Movement.Sprint.canceled += Sprint;

        playerControls.Movement.Jump.performed += Jump;
        playerControls.Movement.Jump.canceled += Jump;



        playerControls.Movement.Dodge.performed += Dodge;

        //Combat
        playerControls.Combat.Block.performed += Block;
        playerControls.Combat.Block.canceled += Block;

        playerControls.Combat.LightAttack.performed += LightAttack;


    }
    #region InputBinding

    #region Movement
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            move = context.ReadValue<Vector2>();
            player.animator.SetBool(AnimationParams.HasInputMove_Param, true);
        }
        else if (context.canceled)
        {
            move = Vector2.zero;
            player.animator.SetBool(AnimationParams.HasInputMove_Param, false);
        }
    }
    public void Look(InputAction.CallbackContext context)
    {
        if (cursorInputForLook)
        {
            if (context.performed)
            {
                look = context.ReadValue<Vector2>();
                //player.animator.SetBool(AnimationParams.HasInputMove_Param, true);
            }
            else if (context.canceled)
            { 
                look = Vector2.zero;
                //player.animator.SetBool(AnimationParams.HasInputMove_Param, false);
            }
        }
    }
    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            sprint = true;
        }
        else if (context.canceled)
        {
            sprint = false;
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && player.comboComponent.currentComboState != EComboState.Playing)
        {
            jump = true;
        }
        else if (context.canceled){
            jump = false;
        }
    }
    public void Dodge(InputAction.CallbackContext context)
    {
        if (context.performed 
            && player.comboComponent.currentComboState != EComboState.Playing
            && player.hitReactionComponent.currentHitReactionState != EHitReactionState.OnHit
            && player.dodgeComponent.currentDodgeState != EDodgeState.OnDodge)
        {
            player.dodgeComponent.StartDodge(player.transform.forward);
        }

    }
    #endregion

    #region Combat
    #region Attack
    public void LightAttack(InputAction.CallbackContext context)
    {
        if (context.performed && player.dodgeComponent.currentDodgeState != EDodgeState.OnDodge)
        {
            //if (player.currentWeapon.currentWeaponState != EWeaponState.Equip) player.characterRigComponent.EquipWeapon();
            player.comboComponent.SetCurrentInput(EKeystroke.LightAttack);
        }
    }
    public void StrongAttack(InputAction.CallbackContext context)
    {
        if (context.performed && player.dodgeComponent.currentDodgeState == EDodgeState.OffDodge)
        {
            if (player.currentWeapon.currentWeaponState != EWeaponState.Equip) player.characterRigComponent.EquipWeapon();
            player.comboComponent.SetCurrentInput(EKeystroke.StrongAttack);
        }
    }
    public void HoldAttack(InputAction.CallbackContext context)
    {
        if (context.performed && player.dodgeComponent.currentDodgeState == EDodgeState.OffDodge)
        {
            if (player.currentWeapon.currentWeaponState != EWeaponState.Equip) player.characterRigComponent.EquipWeapon();
            player.comboComponent.SetCurrentInput(EKeystroke.HoldAttack);
        }
    }
    public void FinisherAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //if (player.targetingComponent.target.hitReactionComponent.isOnStagger)
            //{
            //    player.comboComponent.ResetCombo();
            //    player.comboComponent.SetCurrentInput(EKeystroke.FinisherAttack);
            //}
        }
    }
    #endregion

    #region Block
    Coroutine C_BlockDelay;
    public void StartBlockDelay(bool desiredBlockTarget)
    {
        if(C_BlockDelay != null) StopCoroutine(C_BlockDelay);
        C_BlockDelay =  StartCoroutine(BlockDelay(desiredBlockTarget));
    }
    IEnumerator BlockDelay(bool desiredBlockTarget)
    {
        if(player.isBlock != desiredBlockTarget)
        {
            yield return new WaitForSeconds(.2f);
        }
        player.isBlock = desiredBlockTarget;
        player.animator.SetBool(AnimationParams.Block_Param, player.isBlock);
    }
    public void Block(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.isBlock = true;
            player.animator.SetBool(AnimationParams.Block_Param, player.isBlock);
        }
        else if (context.canceled)
        {
            StartBlockDelay(false);
        }
    }
    #endregion

    #endregion

    #region SwitchWeapon
    public void SwitchWeapon1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Switch To Weapon1");
            player.characterRigComponent.SwitchWeapon(player.characterRigComponent.weapon1Prefab);
        }
    }
    public void SwitchWeapon2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Switch To Weapon2");
            player.characterRigComponent.SwitchWeapon(player.characterRigComponent.weapon2Prefab);
        }
    }
    public void SwitchWeapon3(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Switch To Weapon3");
            player.characterRigComponent.SwitchWeapon(player.characterRigComponent.weapon3Prefab);
        }
    }
    public void SwitchWeapon4(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Switch To Weapon4");
            player.characterRigComponent.SwitchWeapon(player.characterRigComponent.weapon4Prefab);
        }
    }
    #endregion

    #endregion
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }
}