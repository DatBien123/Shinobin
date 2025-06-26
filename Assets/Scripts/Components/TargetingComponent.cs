using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Training
{
    public class TargetingComponent : MonoBehaviour
    {
        [Header("Targeting Settings")]
        public float targetingRadius = 10f;              // Bán kính tìm kiếm
        public float scanInterval = 1.0f;                // Thời gian delay giữa mỗi lần quét
        public LayerMask enemyLayer;                     // Layer chứa kẻ địch

        [Header("Detected Target")]
        public Transform target;

        private void Awake()
        {
            StartCoroutine(TargetScanRoutine());

        }
        IEnumerator TargetScanRoutine()
        {
            while (true)
            {
                FindEnemies();
                yield return new WaitForSeconds(scanInterval);
            }
        }

        void FindEnemies()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, targetingRadius, enemyLayer);

            float closestDistance = Mathf.Infinity;
            Transform closestEnemy = null;

            foreach (Collider hit in hits)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy != null)
            {
                target = closestEnemy;
            }
            else
            {
                target = null;
            }
        }

        #region Debug
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, targetingRadius);
        }
        #endregion
    }
}
