using System.Collections.Generic;
using UnityEngine;

namespace Training
{
    [System.Serializable]
    public enum ETraceType
    {
        None = 0,
        MainWeapon = 1,
        Sphere = 2,
        Box
    }
    public class HitTraceComponent : MonoBehaviour
    {
        [Header("Character Owner")]
        public Character owner;
        [Header("Damage Layer")]
        public LayerMask damageLayer;

        public bool onTrace = false;
        public List<GameObject> hitObjects = new List<GameObject>(); // Lưu trữ các đối tượng bị va chạm


        private void Awake()
        {
            owner = GetComponent<Character>();
        }
        private void Update()
        {
            OnTrace();
        }
        public void BeginTrace()
        {
            owner.hitReactionComponent.PlaySlashFX(owner);
            owner.currentWeapon.onAttackEvent?.Invoke();
            if (owner.hitReactionComponent.currentHitReactionData.hitFeedbackData.isApplyWithoutHit) owner.hitReactionComponent.PlayHitFeedback(owner.transform.position, EReactionType.Combo);
            onTrace = true;
        }
        public void OnTrace()
        {
            if (onTrace)
            {
                switch (owner.hitReactionComponent.currentHitReactionData.hitTrace.traceType)
                {
                    case ETraceType.None:
                        break;

                    case ETraceType.MainWeapon:
                        if(owner.currentWeapon != null)
                        MainWeaponCollide();
                        break;

                    case ETraceType.Sphere:
                        SphereCollide();
                        break;

                    case ETraceType.Box:
                        //BoxCollide();
                        break;

                }
            }
        }
        public void OffTrace()
        {
            onTrace = false;
            hitObjects.Clear();
            owner.currentWeapon.offAttackEvent?.Invoke();

        }
        public void MainWeaponCollide()
        {
            // Lấy tất cả các đối tượng bị va chạm
            Collider[] colliders = Physics.OverlapCapsule(
                owner.currentWeapon.weaponStartPoint.position,
                owner.currentWeapon.weaponEndPoint.position,
                owner.currentWeapon.capsuleRadius,
                owner.currentWeapon.damageLayer
            );

            if (owner as CharacterEnemy)
            {
                foreach (Collider collider in colliders)
                {
                    if (!hitObjects.Contains(collider.gameObject) && !collider.gameObject.GetComponent<CharacterEnemy>())
                    {
                        hitObjects.Add(collider.gameObject);

                        // Xử lý logic khi va chạm
                        Character opponent = collider.GetComponent<Character>();
                        Vector3 collisionPoint = collider.ClosestPoint(owner.transform.position);
                        if (opponent != null && opponent.currentCombatState != ECombatState.Untouchable)
                        {
                            //if (opponent.atributeComponent.currentHP <= 0) return;
                            HandleAttackHit(owner, opponent, collisionPoint);
                        }
                    }
                }
            }
            else if (owner as CharacterPlayer)
            {
                foreach (Collider collider in colliders)
                {
                    if (!hitObjects.Contains(collider.gameObject) && !collider.gameObject.GetComponent<CharacterPlayer>())
                    {
                        hitObjects.Add(collider.gameObject);

                        // Xử lý logic khi va chạm
                        Character opponent = collider.GetComponent<Character>();
                        Vector3 collisionPoint = collider.ClosestPoint(owner.transform.position);
                        if (opponent != null && opponent.currentCombatState != ECombatState.Untouchable)
                        {
                            //if (opponent.atributeComponent.currentHP <= 0) return;
                            HandleAttackHit(owner, opponent, collisionPoint);
                        }
                    }
                }
            }
        }
        public void SphereCollide()
        {
            float radiusDetect = owner.hitReactionComponent.currentHitReactionData.hitTrace.traceRadius;
            Collider[] sphereHitColliders = Physics.OverlapSphere(transform.position, radiusDetect, damageLayer);

            if (owner as CharacterEnemy)
            {
                foreach (Collider hitCollider in sphereHitColliders)
                {
                    if (hitCollider.gameObject.GetComponent<CharacterPlayer>())
                    {
                        // Vector chỉ hướng đến mục tiêu
                        Vector3 directionToTarget = (hitCollider.gameObject.transform.position - owner.transform.position).normalized;

                        // Tính góc giữa Player's forward và Player-Target
                        float angleToTarget = Vector3.Angle(owner.transform.forward, directionToTarget);

                        if (!hitObjects.Contains(hitCollider.gameObject) && angleToTarget <= owner.hitReactionComponent.currentHitReactionData.hitTrace.traceAngle)
                        {
                            hitObjects.Add(hitCollider.gameObject);

                            Character opponent = hitCollider.gameObject.GetComponent<CharacterPlayer>();
                            Vector3 collisionPoint = GetComponent<Collider>().ClosestPoint(owner.transform.position);
                            if (opponent != null && opponent.currentCombatState != ECombatState.Untouchable)
                            {
                                //if (opponent.atributeComponent.currentHP <= 0) return;
                                HandleAttackHit(owner, opponent, collisionPoint);

                            }
                        }

                    }
                }
            }
            else if (owner as CharacterPlayer)
            {
                foreach (Collider hitCollider in sphereHitColliders)
                {
                    if (hitCollider.gameObject.GetComponent<CharacterEnemy>())
                    {
                        // Vector chỉ hướng đến mục tiêu
                        Vector3 directionToTarget = (hitCollider.gameObject.transform.position - owner.transform.position).normalized;

                        // Tính góc giữa Player's forward và Player-Target
                        float angleToTarget = Vector3.Angle(owner.transform.forward, directionToTarget);

                        if (!hitObjects.Contains(hitCollider.gameObject) && angleToTarget <= owner.hitReactionComponent.currentHitReactionData.hitTrace.traceAngle)
                        {
                            hitObjects.Add(hitCollider.gameObject);
                            Character opponent = hitCollider.gameObject.GetComponent<CharacterEnemy>();
                            Vector3 collisionPoint = GetComponent<Collider>().ClosestPoint(owner.transform.position);
                            if (opponent != null && opponent.currentCombatState != ECombatState.Untouchable)
                            {
                                //if (opponent.atributeComponent.currentHP <= 0) return;
                                HandleAttackHit(owner, opponent, collisionPoint);
                            }
                        }
                    }
                }


            }
            Debug.Log("Số lượng nhân vật tìm thấy trong Sphere: " + hitObjects.Count);
        }
        //public void BoxCollide()
        //{
        //    Vector3 boxSize = owner.hitReactionComponent.currentHitReactionData.hitTrace.boxSize;
        //    // Tìm tất cả các Collider trong vùng hình hộp
        //    Collider[] hitColliders = Physics.OverlapBox(transform.position, boxSize / 2, Quaternion.identity, damageLayer);

        //    if (owner as CharacterEnemy)
        //    {
        //        foreach (Collider hitCollider in hitColliders)
        //        {
        //            if (hitCollider.gameObject.GetComponent<CharacterPlayer>())
        //            {
        //                if (!hitObjects.Contains(hitCollider.gameObject))
        //                    hitObjects.Add(hitCollider.gameObject);
        //            }
        //        }
        //    }
        //    else if (owner as CharacterPlayer)
        //    {
        //        foreach (Collider hitCollider in hitColliders)
        //        {
        //            if (hitCollider.gameObject.GetComponent<CharacterEnemy>())
        //            {
        //                if (!hitObjects.Contains(hitCollider.gameObject))
        //                    hitObjects.Add(hitCollider.gameObject);
        //            }
        //        }
        //    }

        //    // Kiểm tra số lượng các đối tượng được tìm thấy
        //    Debug.Log("Số lượng nhân vật tìm thấy trong Box: " + hitObjects.Count);
        //}

        public void HandleAttackHit(Character causer, Character taker, Vector3 hitPoint)
        {
            //Damage
            //taker.atributeComponent.UpdateHealth(-300.0f);

            //
            //Assign HitReactionData
            taker.hitReactionComponent.currentHitReactionDataTake = causer.hitReactionComponent.currentHitReactionData;

            taker.hitReactionComponent.currentComboDataTake = causer.comboComponent.currentComboData.comboData;

            //Apply Hit Feedback
            //taker.hitReactionComponent.PlayHitFeedback(EReactionType.Combo);
            //Apply Hit Reaction
            Vector3 knockBackDirection = taker.gameObject.transform.position - causer.gameObject.transform.position;
            knockBackDirection.y = 0;
            knockBackDirection.Normalize();



                CharacterEnemy takerConvertToEnemy = taker as CharacterEnemy;
                if(takerConvertToEnemy != null && takerConvertToEnemy.TYPE != "Boss")taker.comboComponent.ResetCombo();
                else if(takerConvertToEnemy == null) taker.comboComponent.ResetCombo();


                //if (takerConvertToEnemy != null && takerConvertToEnemy.TYPE != "Boss") taker.skillComponent.ResetSkill();
                //else if (takerConvertToEnemy == null) taker.skillComponent.ResetSkill();



            taker.hitReactionComponent.PlayHitReactions(knockBackDirection, hitPoint, EReactionType.Combo);
            if(!taker.hitReactionComponent.currentHitReactionDataTake.hitFeedbackData.isApplyWithoutHit)
            taker.hitReactionComponent.PlayHitFeedback(hitPoint, EReactionType.Combo);

        }
        private void OnDrawGizmos()
        {
            if (owner != null && owner.currentWeapon != null)
            {
                Transform start = owner.currentWeapon.weaponStartPoint;
                Transform end = owner.currentWeapon.weaponEndPoint;
                float radius = owner.currentWeapon.capsuleRadius;

                if (start != null && end != null)
                {
                    Gizmos.color = Color.red;

                    // Vẽ hai Sphere tại hai đầu của capsule
                    Gizmos.DrawWireSphere(start.position, radius);
                    Gizmos.DrawWireSphere(end.position, radius);

                    // Vẽ line giữa 2 điểm (trục capsule)
                    Gizmos.DrawLine(start.position, end.position);

                    // ✅ Vẽ giả lập vỏ capsule (nâng cao - optional)
                    Vector3 dir = (end.position - start.position).normalized;
                    float distance = Vector3.Distance(start.position, end.position);

                    Quaternion rotation = Quaternion.LookRotation(dir);
                    Matrix4x4 oldMatrix = Gizmos.matrix;
                    Gizmos.matrix = Matrix4x4.TRS(start.position + dir * distance * 0.5f, rotation, Vector3.one);

                    // Mô phỏng thân hình trụ của capsule
                    Gizmos.DrawWireCube(Vector3.zero, new Vector3(radius * 2f, radius * 2f, distance));

                    Gizmos.matrix = oldMatrix;
                }
            }
        }

    }
}
