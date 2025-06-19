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
    void InputBinding()
    {
        //Player Movement
        playerControls.Movement.Move.performed += Move;
        playerControls.Movement.Move.canceled += Move;

        playerControls.Movement.Sprint.performed += Sprint;
        playerControls.Movement.Sprint.canceled += Sprint;

        playerControls.Movement.Jump.performed += Jump;
        playerControls.Movement.Jump.canceled += Jump;

        playerControls.Camera.Look.performed += Look;
        playerControls.Camera.Look.canceled += Look;

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
    #endregion

    #region Combat

    public void LightAttack(InputAction.CallbackContext context)
    {
        if (context.performed /*&& player.dodgeComponent.currentDodgeState == EDodgeState.OffDodge*/)
        {
            //if (player.currentWeapon.currentWeaponState != EWeaponState.Equip) player.characterRigComponent.EquipWeapon();
            player.comboComponent.SetCurrentInput(EKeystroke.LightAttack);
        }
    }
    public void Block(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.isBlock = true;
            player.animator.SetBool(AnimationParams.Block_Param, true);
        }
        else if (context.canceled)
        {
            player.isBlock = false;
            player.animator.SetBool(AnimationParams.Block_Param, false);
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