//using JetBrains.Annotations;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Training
//{
//    public enum ESkillInputType
//    {
//        None = 0,
//        SkillQ = 1,
//        SkillE = 2,
//        SkillR = 3
//    }

//    public enum ESkillState
//    {
//        None = 0,
//        Casting = 1,
//        Finished
//    }
//    public class SkillComponent : MonoBehaviour
//    {
//        [Header("Character Owner")]
//        public Character owner;

//        [Header("Damage Layer")]
//        public LayerMask damageLayer;

//        [Header("Load Skills")]
//        public List<SO_SkillData> loadSkills;

//        [Header("Current Skill Data")]
//        public SO_SkillData currentSkillData;
//        public ESkillState currentSkillState;
//        public ESkillInputType currentSkillInputType;

//        [Header("Delay Skill Cast")]
//        public float inputDelayTime = .1f;
//        private float lastInputTime = 0.0f;

//        [Header("Current Targets")]
//        public List<Transform> currentDamagedTargets;

//        public AnimationClip saveSkills;

//        //GIZMOS
//        private Vector3 lastPosition; // Vị trí gần nhất được kiểm tra
//        private float lastRadius; // Bán kính gần nhất được kiểm tra
//        private bool showGizmos = false; // Kiểm soát hiển thị Gizmos

//        private Vector3 lastBoxPosition; // Vị trí gần nhất của vùng box
//        private Vector3 lastBoxSize; // Kích thước gần nhất của vùng box
//        private Quaternion lastBoxRotation; // Hướng xoay gần nhất của box
//        private bool showBoxGizmos = false; // Kiểm soát hiển thị Gizmos box
//        //

//        Coroutine C_CastSkill;



//        private void Awake()
//        {
//            owner = GetComponent<Character>();
//        }
//        private void LoadSkillDatas()
//        {
//            var skillDatas = Resources.LoadAll<SO_SkillData>($"Skill/{owner.currentWeapon.weaponType}");
//            foreach (var data in skillDatas)
//            {
//                loadSkills.Add(data);
//            }
//        }
//        public void SetSkillInput(ESkillInputType skillInputType)
//        {
//            //Break Lines
//            if (owner.hitReactionComponent.currentHitReactionState == EHitReactionState.OnHit
//                || owner.hitReactionComponent.currentRecoverState == ERecoverState.OnRecover) return;

//            if (lastInputTime + inputDelayTime <= Time.time)
//            {
//                lastInputTime = Time.time;
//                this.currentSkillInputType = skillInputType;
//                if (currentSkillState != ESkillState.Casting)
//                    SaveSkill();
//            }
//        }
//        public SO_SkillData getSkillByCondition()
//        {
//            foreach (var skill in loadSkills)
//            {
//                if (skill.skillData.requiredState == owner.currentState && skill.skillData.skillInput == currentSkillInputType)
//                {
//                    return skill;
//                }
//            }

//            return null;
//        }
//        public void SaveSkill()
//        {
//            SO_SkillData skillData = getSkillByCondition();
//            if ((skillData))
//            {
//                Debug.Log("Assigneld Skill");
//                currentSkillData = skillData;
//                owner.animator.CrossFadeInFixedTime(skillData.skillData.animationClip.name, 0.1f);
//            }
//        }
//        public void SaveSkillAI(SO_SkillData skillData)
//        {
//            currentSkillData = skillData;
//            owner.animator.CrossFadeInFixedTime(skillData.skillData.animationClip.name, 0.1f);
//        }

//        public void StartCastSkill()
//        {
//            //Break lines
//            if (currentSkillData.skillData.listSkillFX.Count <= 0) return;
//            //

//            if (C_CastSkill != null)
//            {
//                StopCoroutine(C_CastSkill);
//            }
//            C_CastSkill = StartCoroutine(CastSkill());
//        }
//        IEnumerator CastSkill()
//        {
//            int currentIndex = 0;

//            while(currentIndex < currentSkillData.skillData.listSkillFX.Count)
//            {
//                Vector3 fxPosition = owner.transform.position + owner.transform.TransformDirection(currentSkillData.skillData.listSkillFX[currentIndex].skillFXPositionOffsett);
//                if (currentSkillData.skillData.listSkillFX[currentIndex].spawnStraightToTarget)
//                {
//                    if (owner.targetingComponent.target != null) fxPosition = owner.targetingComponent.target.transform.position + owner.targetingComponent.target.transform.forward;
//                    fxPosition.y = 0;
//                }
//                Quaternion fxRotation = owner.transform.rotation * Quaternion.Euler(currentSkillData.skillData.listSkillFX[currentIndex].skillFXRotationOffsett);
//                GameObject fx = Instantiate(
//                    currentSkillData.skillData.listSkillFX[currentIndex].skillFXGameObj,
//                    fxPosition,
//                    fxRotation
//                );
//                fx.transform.localScale = currentSkillData.skillData.listSkillFX[currentIndex].skillFXScaleOffsett;
//                //-------------------------------------------------
//                //Kiểm tra Fx có phải là thuộc KriptoFX không?
//                //Nếu đúng thì gán caster
//                //if (fx.GetComponent<MagicFX5_EffectSettings>() != null) fx.GetComponent<MagicFX5_EffectSettings>().caster = owner;
//                //Nếu không đúng thì sử dụng FX của bản thân
//                if (fx.GetComponent<Projectile>() != null) fx.GetComponent<Projectile>().caster = owner;
//                //......
//                Destroy(fx, 7.0f);

//                yield return new WaitForSeconds(currentSkillData.skillData.listSkillFX[currentIndex].delayTimeToNextFX);

//                currentIndex++;
//            }
//        }

//        public void PlaySkillCastSFX(Vector3 position)
//        {
//            if (currentSkillData.skillData.skillSFX.skillAudioClip != null)
//            {
//                // Tạo một GameObject tạm thời để chứa AudioSource
//                GameObject tempAudioSourceObject = new GameObject("TempAudioSource");
//                tempAudioSourceObject.transform.position = position;

//                // Thêm AudioSource vào GameObject này
//                AudioSource audioSource = tempAudioSourceObject.AddComponent<AudioSource>();
//                audioSource.clip = currentSkillData.skillData.skillSFX.skillAudioClip;

//                // Thiết lập âm lượng cho AudioSource
//                audioSource.volume = currentSkillData.skillData.skillSFX.volume;
//                audioSource.pitch = currentSkillData.skillData.skillSFX.pitch;
//                // Phát âm thanh
//                audioSource.Play();

//                // Hủy GameObject sau khi âm thanh đã phát xong
//                Destroy(tempAudioSourceObject, currentSkillData.skillData.skillSFX.skillAudioClip.length);
//            }
//        }
//        public List<GameObject> GetAllEnemyInRange(Vector3 position)
//        {
//            List<GameObject> output = new List<GameObject>();
//            switch (currentSkillData.skillData.hitTrace.traceType)
//            {
//                case ETraceType.None:
//                    break;
//                case ETraceType.MainWeapon:
//                    break;
//                case ETraceType.Sphere:
//                    float radiusDetect = currentSkillData.skillData.hitTrace.traceRadius;
//                    Collider[] sphereHitColliders = Physics.OverlapSphere(position, radiusDetect, damageLayer);
//                    List<GameObject> characterObjectsSphere = new List<GameObject>();

//                    // Lưu vị trí và bán kính để hiển thị Gizmos
//                    lastPosition = position;
//                    lastRadius = radiusDetect;
//                    showGizmos = true;

//                    if (owner as CharacterEnemy)
//                    {
//                        foreach (Collider hitCollider in sphereHitColliders)
//                        {
//                            if (hitCollider.gameObject.GetComponent<CharacterPlayer>())
//                            {
//                                if (!characterObjectsSphere.Contains(hitCollider.gameObject))
//                                {
//                                    characterObjectsSphere.Add(hitCollider.gameObject);
//                                }
//                            }
//                        }
//                        output = characterObjectsSphere;
//                    }
//                    else if (owner as CharacterPlayer)
//                    {
//                        foreach (Collider hitCollider in sphereHitColliders)
//                        {
//                            if (hitCollider.gameObject.GetComponent<CharacterEnemy>())
//                            {
//                                if (!characterObjectsSphere.Contains(hitCollider.gameObject))
//                                {
//                                    characterObjectsSphere.Add(hitCollider.gameObject);
//                                }
//                            }
//                        }
//                        output = characterObjectsSphere;
//                    }
//                    Debug.Log("Output Sphere is: " + output.Count);
//                    break;
//                case ETraceType.Box:
//                    Vector3 boxSize = currentSkillData.skillData.hitTrace.boxSize;
//                    Collider[] hitColliders = Physics.OverlapBox(position + owner.transform.TransformDirection(currentSkillData.skillData.hitTrace.boxOffset), boxSize / 2, owner.transform.rotation, damageLayer);
//                    List<GameObject> characterObjectsBox = new List<GameObject>();

//                    // Lưu thông tin cho Gizmos
//                    lastBoxPosition = position + owner.transform.TransformDirection(currentSkillData.skillData.hitTrace.boxOffset);
//                    lastBoxSize = boxSize;
//                    lastBoxRotation = owner.transform.rotation;
//                    showBoxGizmos = true;

//                    if (owner as CharacterEnemy)
//                    {
//                        foreach (Collider hitCollider in hitColliders)
//                        {
//                            if (hitCollider.gameObject.GetComponent<CharacterPlayer>())
//                            {
//                                if (!characterObjectsBox.Contains(hitCollider.gameObject))
//                                    characterObjectsBox.Add(hitCollider.gameObject);
//                            }
//                        }
//                        output = characterObjectsBox;
//                    }
//                    else if (owner as CharacterPlayer)
//                    {
//                        foreach (Collider hitCollider in hitColliders)
//                        {
//                            if (hitCollider.gameObject.GetComponent<CharacterEnemy>())
//                            {
//                                if (!characterObjectsBox.Contains(hitCollider.gameObject))
//                                    characterObjectsBox.Add(hitCollider.gameObject);
//                            }
//                        }
//                        output = characterObjectsBox;
//                    }

//                    Debug.Log("Output Box is: " + characterObjectsBox.Count);
//                    break;
//            }
//            return output;
//        }

//        void OnDrawGizmos()
//        {
//            if (showGizmos)
//            {
//                Gizmos.color = Color.red; // Màu của Gizmos
//                Gizmos.DrawWireSphere(lastPosition, lastRadius); // Vẽ hình cầu dạng đường viền
//            }
//            if (showBoxGizmos)
//            {
//                Gizmos.color = Color.blue; // Đặt màu cho Gizmos
//                Gizmos.matrix = Matrix4x4.TRS(lastBoxPosition, lastBoxRotation, Vector3.one); // Đặt xoay và vị trí
//                Gizmos.DrawWireCube(Vector3.zero, lastBoxSize); // Vẽ hình hộp dây
//            }
//        }
//        public void ResetSkill()
//        {
//            owner.animator.applyRootMotion = false;
//            loadSkills.Clear();
//            currentSkillState = ESkillState.None;
//            if (owner as CharacterPlayer)
//                LoadSkillDatas();
//            //currentDamagedTargets.Clear();
//        }

//        //public void CastSkill()
//        //{
//        //    if (currentSkillData.skillData.listSkillFX.Count > 0)
//        //    {
//        //        //Khỏi tạo FX theo các trạng thái GameObject từ SO
//        //        foreach (var skillFX in currentSkillData.skillData.listSkillFX)
//        //        {
//        //            Vector3 fxPosition = owner.transform.position + owner.transform.TransformDirection(skillFX.skillFXPositionOffsett);
//        //            if (skillFX.spawnStraightToTarget)
//        //            {
//        //                if (owner.targetingComponent.target != null) fxPosition = owner.targetingComponent.target.transform.position + owner.targetingComponent.target.transform.forward;
//        //                fxPosition.y = 0;
//        //            }
//        //            Quaternion fxRotation = owner.transform.rotation * Quaternion.Euler(skillFX.skillFXRotationOffsett);


//        //            GameObject fx = Instantiate(
//        //                skillFX.skillFXGameObj,
//        //                fxPosition,
//        //                fxRotation
//        //            );
//        //            //-------------------------------------------------
//        //            //Kiểm tra Fx có phải là thuộc KriptoFX không?
//        //            //Nếu đúng thì gán caster
//        //            if (fx.GetComponent<MagicFX5_EffectSettings>() != null) fx.GetComponent<MagicFX5_EffectSettings>().caster = owner;
//        //            //Nếu không đúng thì sử dụng FX của bản thân

//        //            //......
//        //            Destroy(fx, 7.0f);
//        //        }
//        //    }
//        //}
//    }
//}
