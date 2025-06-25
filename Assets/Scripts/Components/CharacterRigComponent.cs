using UnityEngine;

namespace Training
{
    public class CharacterRigComponent : MonoBehaviour
    {
        public Character owner;

        [Header("Equip - UnEquip Weapon Socket")]
        public Transform equipSocket;
        public Transform unEquipSocket;

        [Header("Equip - UnEquip Rig Socket")]
        public Transform leftShoulderSocket;
        public Transform rightShoulderSocket;
        public Transform leftHandSocket;
        public Transform rightHandSocket;

        public Transform skillCastSocket;


        public GameObject weapon1Prefab;
        public GameObject weapon2Prefab;
        public GameObject weapon3Prefab;
        public GameObject weapon4Prefab;

        [Header("Camera Follow Socket")]
        public Transform cameraFollowSocket;
        public Transform cameraLookAtSocket;
        public Transform cameraFinisherSocket;

        private void Awake()
        {
            owner = GetComponent<Character>();

        }
        private void Start()
        {
            SwitchWeapon(weapon1Prefab);
            EquipWeapon();
        }

        public void SwitchWeapon(GameObject weaponPrefab)
        {
            if (weaponPrefab != null)
            {
                DeleteCurrentWeapon();
                GameObject weapon = Instantiate(weaponPrefab);
                weapon.GetComponent<Weapon>().SetWeaponOwner(owner);
                owner.currentWeapon = weapon.GetComponent<Weapon>();

                ClassifySocket();

                if (owner as CharacterPlayer)
                {
                    (owner as CharacterPlayer).temporaryLocomotion = Instantiate(owner.currentWeapon.weaponLocomotionData);
                }

                EquipWeapon();

                owner.comboComponent.ResetCombo();
                owner.finisherComponent.LoadFinisher();
                //owner.skillComponent.ResetSkill();
            }
        }
        public void DeleteCurrentWeapon()
        {
            if (owner.currentWeapon != null)
            {
                GameObject currentWeapon = owner.currentWeapon.gameObject;
                owner.currentWeapon = null;
                Destroy(currentWeapon);
            }
        }
        public void EquipWeapon()
        {
            if (owner.currentWeapon.gameObject != null && owner != null)
            {
                owner.currentWeapon.gameObject.transform.SetParent(this.equipSocket);
                owner.currentWeapon.gameObject.transform.localPosition = owner.currentWeapon.gameObject.GetComponent<Weapon>().equipPositionOffset;
                owner.currentWeapon.gameObject.transform.localRotation = Quaternion.Euler(owner.currentWeapon.gameObject.GetComponent<Weapon>().equipRotationOffset);
                owner.currentWeapon.gameObject.transform.localScale = owner.currentWeapon.gameObject.GetComponent<Weapon>().equipScaleOffset;
                owner.currentWeapon.currentWeaponState = EWeaponState.Equip;

                owner.animator.runtimeAnimatorController = owner.currentWeapon.weaponEquipAnimator;
            }
        }
        public void UnEquipWeapon()
        {
            if (owner.currentWeapon.gameObject != null && owner != null)
            {
                owner.currentWeapon.gameObject.transform.SetParent(this.unEquipSocket);
                owner.currentWeapon.gameObject.transform.localPosition = owner.currentWeapon.gameObject.GetComponent<Weapon>().unEquipPositionOffset;
                owner.currentWeapon.gameObject.transform.localRotation = Quaternion.Euler(owner.currentWeapon.gameObject.GetComponent<Weapon>().unEquipRotationOffset);
                owner.currentWeapon.gameObject.transform.localScale = owner.currentWeapon.gameObject.GetComponent<Weapon>().unEquipScaleOffset;
                owner.currentWeapon.currentWeaponState = EWeaponState.Unequip;

                owner.animator.runtimeAnimatorController = owner.currentWeapon.weaponUnEquipAnimator;
                
                owner.SetWeightLayer(EAnimationLayer.UpperBodyLayer, 0.0f);
                
            }
        }

        public void ClassifySocket()
        {
            //Classify Equip Socket
            if(owner.currentWeapon.gameObject != null && owner != null)
            {
                if(owner.currentWeapon.rigTransformEquip == ERigTransform.LeftHand)
                {
                    equipSocket = leftHandSocket;
                }
                else if(owner.currentWeapon.rigTransformEquip == ERigTransform.RightHand)
                {
                    equipSocket = rightHandSocket;
                }
                else if(owner.currentWeapon.rigTransformEquip == ERigTransform.LeftShoulder)
                {
                    equipSocket = leftShoulderSocket;
                }
                else if(owner.currentWeapon.rigTransformEquip == ERigTransform.RightShoulder)
                {
                    equipSocket = rightShoulderSocket;
                }
            }

            //Classify UnEquip Socket
            if (owner.currentWeapon.gameObject != null && owner != null)
            {
                if (owner.currentWeapon.rigTransformUnEquip == ERigTransform.LeftHand)
                {
                    unEquipSocket = leftHandSocket;
                }
                else if (owner.currentWeapon.rigTransformUnEquip == ERigTransform.RightHand)
                {
                    unEquipSocket = rightHandSocket;
                }
                else if (owner.currentWeapon.rigTransformUnEquip == ERigTransform.LeftShoulder)
                {
                    unEquipSocket = leftShoulderSocket;
                }
                else if (owner.currentWeapon.rigTransformUnEquip == ERigTransform.RightShoulder)
                {
                    unEquipSocket = rightShoulderSocket;
                }
            }
        }

    }
}
