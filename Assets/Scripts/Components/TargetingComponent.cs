using UnityEngine;
using System.Collections.Generic;
using System.Collections;
namespace Training
{
    public class TargetingComponent : MonoBehaviour
    {
        [Header("Owner and Target")]
        public Character owner;
        public Character target;
        public LayerMask enemyLayer;

        [Header("Targeting Radius")]
        public float targetingRadius;
        public bool isTargetInRange = false;

        [Header("Lastest Position Target")]
        public float timeUpdated = 0.0f;
        public float timeUpdatePositonDelay = .25f;
        public Vector3 lastestPositonTarget = Vector3.zero;

        public CameraManager cameraManager;
        [Header("Target Lock Params")]
        public float lockAgnle;

        Coroutine C_ValidateLockOn;
        private void Awake()
        {
            owner = GetComponent<Character>();

            cameraManager = FindObjectOfType<CameraManager>();
        }

        public void StartValidatedLockOnTarget()
        {
            if (cameraManager == null) return;
            if(C_ValidateLockOn != null)
            {
                StopCoroutine(C_ValidateLockOn);
            }
            StartCoroutine(ValidatedCurrentLockOnTarget());
        }
        private void Update()
        {
            if (cameraManager != null)
            {
                if (owner as CharacterPlayer && cameraManager.currentCameraType != ECameraType.LockOnCamera) FindNearestEnemy();
                else if (owner as CharacterEnemy) FindNearestEnemy();
            }
            UpdateLastestPosition();
            if(target != null)
            Debug.Log("Distance To Player is: " + Vector3.Distance(owner.transform.position, target.transform.position));
        }

        public void UpdateLastestPosition()
        {
            if (target != null)
            {
                if (Time.time > timeUpdated + timeUpdatePositonDelay)
                {
                    timeUpdated = Time.time;
                    lastestPositonTarget = target.transform.position;
                }
            }
        }
        //Basics
        public void FindNearestEnemy()
        {
            // Tìm tất cả các đối tượng trong khoảng bán kính "targetingRadius" trên layer enemyLayer
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, targetingRadius, enemyLayer);

            float closestDistance = Mathf.Infinity; // Khoảng cách gần nhất
            Character nearestTarget = null;

            if (owner as CharacterPlayer)
            {
                foreach (var hitCollider in hitColliders)
                {
                    // Kiểm tra xem collider có phải là một CharacterEnemy hay không
                    CharacterEnemy enemy = hitCollider.GetComponent<CharacterEnemy>();
                    if (enemy != null)
                    {
                        float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                        if (distanceToEnemy < closestDistance)
                        {
                            closestDistance = distanceToEnemy;
                            if(enemy.atributeComponent.currentHP >=0)nearestTarget = enemy;
                        }
                    }
                }
            }
            else if (owner as CharacterEnemy)
            {
                foreach (var hitCollider in hitColliders)
                {
                    // Kiểm tra xem collider có phải là một CharacterEnemy hay không
                    CharacterPlayer player = hitCollider.GetComponent<CharacterPlayer>();
                    if (player != null)
                    {
                        float distanceToEnemy = Vector3.Distance(transform.position, player.transform.position);
                        if (distanceToEnemy < closestDistance)
                        {
                            closestDistance = distanceToEnemy;
                            if (player.atributeComponent.currentHP >= 0) nearestTarget = player;
                        }
                    }
                }
            }

            // Gán mục tiêu gần nhất
            if(owner as CharacterPlayer)
            {
                target = nearestTarget;
            }
            else if(owner as CharacterEnemy)
            {
                if (target == null || target.atributeComponent.currentHP <= 0.0f) target = nearestTarget;
            }

            if (target != null)
            {
                if (Vector3.Distance(target.transform.position, owner.transform.position) <= targetingRadius)
                {
                    isTargetInRange = true;
                }
                else
                {
                    isTargetInRange = false;
                }
            }
            else { isTargetInRange = false; }
        }

        //Player Target Lock
        public CharacterEnemy DetectHitEnemy()
        {
            // Tìm tất cả các đối tượng trong khoảng bán kính "targetingRadius" trên layer enemyLayer
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, targetingRadius, enemyLayer);

            float minAngle = 10000;
            CharacterPlayer player = owner as CharacterPlayer;
            List<CharacterEnemy> enemies = new List<CharacterEnemy>();
            Vector3 cameraForwardDirection = player._mainCamera.transform.forward;
            if (player)
            {
                foreach (var hitCollider in hitColliders)
                {
                    // Kiểm tra xem collider có phải là một CharacterEnemy hay không
                    CharacterEnemy enemy = hitCollider.GetComponent<CharacterEnemy>();
                    if (enemy != null)
                    {
                        Vector3 directionToPlayer = enemy.transform.position - owner.transform.position;
                        float angle = Vector3.Angle(directionToPlayer, cameraForwardDirection);
                        if (angle < minAngle && angle < lockAgnle)
                        {
                            minAngle = angle;
                            enemies.Add(enemy);

                        }
                    }
                }
            }

            if (enemies.Count <= 0)
            {
                target = null;
                return null;
            }
            //If has enemy in range
            foreach (CharacterEnemy enemy in enemies)
            {
                Vector3 directionToPlayer = enemy.transform.position - owner.transform.position;
                float angle = Vector3.Angle(directionToPlayer, cameraForwardDirection);

                if (Mathf.Approximately(angle, minAngle)) {
                    target = enemy;
                    return enemy;
                }

            }
            target = null;
            return null;

        }
        IEnumerator ValidatedCurrentLockOnTarget()
        {
            float distance = Utilities.Instance.DistanceCalculate(transform.position, target.transform.position);

            while (distance < targetingRadius)
            {
                if(target != null) 
                distance = Utilities.Instance.DistanceCalculate(transform.position, target.transform.position);
                Debug.Log("Still in Lock On");
                yield return null;
            }

            cameraManager.SwitchCamera(ECameraType.FollowCamera);
        }
        #region Debug
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, targetingRadius);
        }

        void DrawSphere(Vector3 center, float radius, Color color)
        {
            // Sử dụng Gizmos để vẽ trong Game View
            Gizmos.color = color;
            Gizmos.DrawWireSphere(center, radius);
        }
        #endregion
    }
}
