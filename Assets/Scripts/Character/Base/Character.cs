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
    public float _verticalVelocity;

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


    protected virtual void Start()
    {
    }
    protected virtual void Update()
    {
    }
    protected virtual void LateUpdate()
    {

    }
    protected virtual void FixedUpdate()
    {

    }
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
