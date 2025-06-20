
using System.Collections;

using UnityEngine;
using UnityEngine.Events;


namespace Training
{
    public class FreeflowComponent : MonoBehaviour
    {
        public Character owner;

        public float stoppingDistance = 3.0f;

        Coroutine C_LOL;

        private void Awake()
        {
            owner = GetComponent<Character>();
        }

        public void StartLOL()
        {
            if(C_LOL != null) StopCoroutine(C_LOL);
            C_LOL = StartCoroutine(LOL());
        }
        IEnumerator LOL()
        {
            // Nếu không có mục tiêu → không làm gì cả
            if (owner.targetingComponent.target == null)
                yield break;

            Transform target = owner.targetingComponent.target;

            // Quay về phía mục tiêu ngay từ đầu
            Vector3 targetDirection = target.position - owner.transform.position;
            targetDirection.y = 0;



            // Bắt đầu theo dõi trong quá trình combo diễn ra
            while (owner.comboComponent.currentComboState == EComboState.Playing)
            {
                if (target == null) yield break;

                float distance = Utilities.Instance.DistanceCalculate(owner.transform.position, target.position);

                if (targetDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, targetRotation, Time.deltaTime * 10f);
                }
                if (distance < stoppingDistance) owner.animator.applyRootMotion = false;

                // Tiếp tục kiểm tra mỗi frame
                yield return null;
            }
        }

    }
}
