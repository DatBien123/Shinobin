
using DG.Tweening;
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

        public float height = 5.0f;

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

                // Tiếp tục kiểm tra mỗi frame
                yield return null;
            }
        }



        public void MoveCurveToBackward()
        {
            Transform player = owner.transform;
            Transform enemy = owner.targetingComponent.target.transform;

            // Vị trí bắt đầu
            Vector3 startPos = player.position;

            // Vị trí phía sau enemy
            Vector3 behindPos = enemy.position - enemy.forward * 1.0f; // 2 đơn vị phía sau

            // Tính điểm giữa lệch sang bên phải enemy
            Vector3 midPos = (startPos + behindPos) / 2f + -enemy.right * 1.5f;

            Vector3[] path = new Vector3[] { startPos, midPos, behindPos };

            player.DOPath(path, 0.53f, PathType.CatmullRom)
                  .SetEase(Ease.InOutSine)
                  .OnComplete(() =>
                  {
                      Debug.Log("Hoàn thành di chuyển đường cong ra sau kẻ địch.");
                  });
        }

        public void StartDashToTarget(float duration)
        {
                Transform player = owner.transform;
                Transform enemy = owner.targetingComponent.target.transform;

                // Vị trí bắt đầu
                Vector3 startPos = player.position;

                // Vị trí phía sau enemy
                Vector3 behindPos = enemy.position + enemy.forward * 1.0f; // 2 đơn vị phía sau

                // Tính điểm giữa lệch sang bên phải enemy
                Vector3 midPos = (startPos + behindPos) / 2f + enemy.right * 1.5f;

                Vector3[] path = new Vector3[] { startPos, midPos, behindPos };

                player.DOPath(path, duration, PathType.CatmullRom)
                      .SetEase(Ease.InOutSine)
                      .OnComplete(() =>
                      {
                          Debug.Log("Hoàn thành di chuyển đường cong ra sau kẻ địch.");
                      });
        }

        //public void MoveAlongParabola(float duration)
        //{
        //    Vector3 startPos = transform.position;
        //    Vector3 endPos = owner.targetingComponent.target.transform.position;

        //    DOTween.To(() => 0f, t =>
        //    {
        //        // Di chuyển tuyến tính trên X và Z
        //        Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);

        //        // Parabol trên Y
        //        float parabolaY = -4 * height * (t - 0.5f) * (t - 0.5f) + height;

        //        currentPos.y += parabolaY;

        //        transform.position = currentPos;

        //    }, 1f, duration)
        //    .SetEase(Ease.Linear)
        //    .OnComplete(() => Debug.Log("Di chuyển hoàn tất!"));
        //}


        public float lookSpeed = 10f;
        private Tween moveTween;

        public void MoveAlongParabola(float duration)
        {
            if (moveTween != null && moveTween.IsActive())
                moveTween.Kill();

            Vector3 startPos = transform.position;

            moveTween = DOTween.To(() => 0f, t =>
            {
                // Lấy vị trí target realtime
                Vector3 currentTargetPos = owner.targetingComponent.target.transform.position;

                // Di chuyển tuyến tính từ start đến target hiện tại
                Vector3 linearPos = Vector3.Lerp(startPos, currentTargetPos, t);

                // Thêm đường cong parabol vào trục Y
                float parabolaY = -4 * height * (t - 0.5f) * (t - 0.5f) + height;
                linearPos.y += parabolaY;

                transform.position = linearPos;

                // Quay mặt về target liên tục
                Vector3 dir = (currentTargetPos - transform.position).normalized;
                if (dir != Vector3.zero)
                {
                    Quaternion lookRot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * lookSpeed);
                }

            }, 1f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Debug.Log("Di chuyển hoàn tất!");
            });
        }

        public void StopMove()
        {
            if (moveTween != null && moveTween.IsActive())
                moveTween.Kill();
        }

    }
}
