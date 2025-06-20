using StarterAssets;
using System;
using Training;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public enum ECombatState
{
    None = 0,
    Attacking,
    BeingBeaten,
    Untouchable
}
public enum EStateType
{
    None,
    Grounded,
    InAir
}
public class Character : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public CharacterController characterController;
    public AtributeComponent atributeComponent;
    public ComboComponent comboComponent;
    public TargetingComponent targetingComponent;
    public FreeflowComponent freeflowComponent;
    public HitReactionComponent hitReactionComponent;
    public HitTraceComponent hitTraceComponent;
    public DodgeComponent dodgeComponent;
    public CharacterRigComponent characterRigComponent;
    public TeleportComponent teleportComponent;
    public AnimationEvents animationEvent;

    [Header("Current Props")]
    [Header("Weapon")]
    public Weapon currentWeapon;
    public SO_LocomotionData temporaryLocomotion;

    [Header("Combat State")]
    public bool isBlock = false;
    public bool isApplyAnimationMove = false;
    public ECombatState currentCombatState;

    [Header("Movement State")]
    public float motionFactor = 5.0f;
    public EStateType currentState;

    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    [Tooltip("Player blocking proprs")]
    public float BlockSprintSpeed = 3.0f;

    // player
    public float _speed;
    public float _animationBlend;
    public float _targetRotation = 0.0f;
    public float _rotationVelocity;
    public float _verticalVelocity;
    public float _terminalVelocity = 53.0f;

    // timeout deltatime
    public float _jumpTimeoutDelta;
    public float _fallTimeoutDelta;

    //animation
    public float _inputHorizontalParam = 0.0f;
    public float _inputVerticalParam = 0.0f;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        atributeComponent = GetComponent<AtributeComponent>();
        hitReactionComponent = GetComponent<HitReactionComponent>();
        hitTraceComponent = GetComponent<HitTraceComponent>();
        comboComponent = GetComponent<ComboComponent>();
        targetingComponent = GetComponent<TargetingComponent>();
        freeflowComponent = GetComponent<FreeflowComponent>();
        dodgeComponent = GetComponent<DodgeComponent>();
        characterRigComponent = GetComponent<CharacterRigComponent>();
        animationEvent = GetComponent<AnimationEvents>();
        teleportComponent = GetComponent<TeleportComponent>();
    }
    protected virtual void Start(){
        animator.SetFloat(AnimationParams.BlockSide_Param, 1.0f);
    }
    protected virtual void Update(){}
    protected virtual void LateUpdate(){}
    protected virtual void FixedUpdate(){}
    protected virtual void OnAnimatorMove()
    {
        if (isApplyAnimationMove)
        {
            //Lấy thời gian đã chuẩn hóa trong animation
                float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            // Đọc các giá trị Curve từ Animator
            float xMovement = animator.GetFloat("xMovementSpeed");
            float yMovement = animator.GetFloat("yMovementSpeed");
            float zMovement = animator.GetFloat("zMovementSpeed");

            // Tạo vector chuyển động theo Curve
            Vector3 motion = new Vector3(xMovement, yMovement, zMovement) * transform.localScale.x * motionFactor;
            motion = transform.TransformDirection(motion); // Chuyển từ Local Space sang World Space

            if (targetingComponent.target != null)
            {
                float distanceToTarget = Utilities.Instance.DistanceCalculate(transform.position, targetingComponent.target.transform.position, true);
                if (distanceToTarget <= freeflowComponent.stoppingDistance)
                {
                    motion.x = 0.0f;
                    motion.z = 0.0f;

                }

            }

            // Áp dụng chuyển động theo CharacterController
            characterController.Move(motion * Time.deltaTime + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }
    }
    protected virtual void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character
        animator.SetBool(AnimationParams.IsGround_Param, Grounded);
    }
    //protected virtual void OnAnimatorMove()
    //{
    //if (isApplyAnimationSpeed)
    //{
    //    float animSpeed = animator.GetFloat("AnimSpeed");
    //    animator.speed = animSpeed * animSpeedMotionfactor;
    //}
    //if (isApplyRotation)
    //{
    //    float rotationSpeed = animator.GetFloat("RotationSpeed"); // Lấy tốc độ xoay từ Curve
    //    Debug.Log("Rotation Speed is: " + rotationSpeed);

    //    if (targetingComponent.target != null)
    //    {
    //        Vector3 directionToTarget = targetingComponent.target.transform.position - transform.position;
    //        directionToTarget.Normalize();
    //        directionToTarget.y = 0; // Giữ nhân vật quay ngang, không ngửa lên/xuống


    //        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    //    }

    //}
    //if (isApplyAnimationMove)
    //{
    //    // Lấy thời gian đã chuẩn hóa trong animation
    //    float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

    //    // Đọc các giá trị Curve từ Animator
    //    float xMovement = animator.GetFloat("xMovementSpeed");
    //    float yMovement = animator.GetFloat("yMovementSpeed");
    //    float zMovement = animator.GetFloat("zMovementSpeed");

    //    // Tạo vector chuyển động theo Curve
    //    Vector3 motion = new Vector3(xMovement, yMovement, zMovement) * transform.localScale.x * motionFactor;
    //    motion = transform.TransformDirection(motion); // Chuyển từ Local Space sang World Space

    //    if(targetingComponent.target != null)
    //    {
    //        float distanceToTarget = Utilities.Instance.DistanceCalculate(transform.position, targetingComponent.target.transform.position, true);
    //        if(distanceToTarget <= freeflowComponent.stoppingDistance) {
    //            motion.x = 0.0f;
    //            motion.z = 0.0f;

    //        }

    //    }

    //    // Áp dụng chuyển động theo CharacterController
    //    characterController.Move(motion * Time.deltaTime);
    //}
    //}
    public void SetWeightLayer(EAnimationLayer animationLayer, float weight)
    {
        animator.SetLayerWeight((int)EAnimationLayer.UpperBodyLayer, weight);
    }

}
