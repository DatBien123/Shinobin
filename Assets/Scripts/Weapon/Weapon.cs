using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

namespace Training
{
    public enum EWeaponType
    {
        None = 0,
        Katana,
        Sword,
        GreatSword,
        Acher
    }
    public enum EWeaponState
    {
        None,
        Equip,
        Unequip
    }

    [System.Serializable]
    public enum ERigTransform
    {
        LeftHand,
        RightHand,
        LeftShoulder,
        RightShoulder

    }
    public class Weapon : MonoBehaviour
    {
        public Character owner;
        public EWeaponType weaponType;
        public EWeaponState currentWeaponState = EWeaponState.None;
        public RuntimeAnimatorController weaponEquipAnimator;
        public RuntimeAnimatorController weaponUnEquipAnimator;

        public ERigTransform rigTransformEquip;
        public ERigTransform rigTransformUnEquip;

        [Header("Socket Equip TRANSFORM/OFFSET")]
        public Vector3 equipPositionOffset;
        public Vector3 equipRotationOffset;
        public Vector3 equipScaleOffset;

        [Header("Socket UnEquip TRANSFORM/OFFSET")]
        public Vector3 unEquipPositionOffset;
        public Vector3 unEquipRotationOffset;
        public Vector3 unEquipScaleOffset;

        [Header("Weapon Collider Detection")]
        public Transform weaponStartPoint; // Điểm đầu của capsule
        public Transform weaponEndPoint;   // Điểm cuối của capsule
        public float capsuleRadius = 0.5f; // Bán kính của capsule
        public LayerMask damageLayer;

        public bool isAttacking = false; // Trạng thái tấn công

        public SO_LocomotionData weaponLocomotionData;

        public UnityEvent onAttackEvent;
        public UnityEvent offAttackEvent;

        public void SetWeaponOwner(Character owner)
        {
            this.owner = owner;
        }
    }
}
