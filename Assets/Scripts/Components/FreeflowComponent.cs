
using System.Collections;

using UnityEngine;
using UnityEngine.Events;


namespace Training
{

    [System.Serializable]
    public struct FreeflowData
    {
        public bool isFreeflow;
        public float duration;
        public float range;
        public float stoppingDistance;
    }
    [System.Serializable]
    public enum EDisplacementDirection
    {
        F,
        L,
        R,
        B,
        FL,
        FR,
        BL,
        BR
    }
    [System.Serializable]
    public enum EDisplacementTargetType
    {
        Self,
        Target
    }
    [System.Serializable]
    public struct DisplacementData
    {
        public bool isDisplacement;
        public float distance;
        public float duration;
        public EDisplacementTargetType targetType;
        public EDisplacementDirection direction;
    }
    public class FreeflowComponent : MonoBehaviour
    {
        [Header("Owner")]
        public Character owner;

        [Header("Freeflow Stuffs")]
        [Header("Freeflow")]
        public float freeflowRange = 3.0f;
        public float freeflowDuration = .5f;
        public float stoppingDistance = 1.5f;
        public float freeflowSpeed = 3.0f;
        public float freeflowStraightSpeed = 3.0f;

        [Header("Freeflow AI")]
        public float freeflowDurationMax = .5f;

        [Header("Freeflow Events")]
        public UnityEvent onFreeflowEvent;
        public UnityEvent offFreeflowEvent;

        Coroutine C_FreeFlow;
        Coroutine C_Displacement;
        Coroutine C_DynamicTrajectoryGenerate;
        

        private void Awake()
        {
            owner = GetComponent<Character>();
        }
        //Basic Funcs
        #region Freeflow
        public void StartFreeflow()
        {
                if (C_FreeFlow != null) StopCoroutine(C_FreeFlow);
                C_FreeFlow = StartCoroutine(Freeflow());
        }
        IEnumerator Freeflow()
        {
            //Break Lines
            if (owner.targetingComponent != null && owner.targetingComponent.target == null) yield break;

            Vector3 freeflowDirection = owner.targetingComponent.target.gameObject.transform.position - transform.position;
            freeflowDirection.Normalize();
            freeflowDirection.y = 0.0f;

            float distance = Vector3.Distance(owner.gameObject.transform.position, owner.targetingComponent.target.transform.position);

            if (distance > freeflowRange) yield break;

            //Init Coroutine Params
            //Duration
            float raito = distance / freeflowRange;
            float freeflowDuration = freeflowDurationMax * raito;
            //Direction
            float freeflowSpeed = distance / freeflowDuration;
            float elapsedTime = 0.0f;

            //if (owner.targetingComponent != null && owner.targetingComponent.target != null)
            //    owner.lookAtComponent.LookAtEnemy(owner.targetingComponent.target.transform.position, owner.lookAtComponent.lookAtDuration);
            //Start Coroutine
            onFreeflowEvent?.Invoke();
            while (elapsedTime <= freeflowDuration && owner.targetingComponent.target != null
                && owner.hitReactionComponent.currentHitReactionState != EHitReactionState.OnHit)
            {
                elapsedTime += Time.fixedDeltaTime;
                Vector3 displacement = freeflowDirection * freeflowSpeed * Time.fixedDeltaTime;
                owner.characterController.Move(displacement);

                distance = Utilities.Instance.DistanceCalculate(owner.gameObject.transform.position, owner.targetingComponent.target.transform.position, true);
                if (distance <= stoppingDistance) break;
                yield return null;
            }
            //if (owner.targetingComponent != null && owner.targetingComponent.target != null)
            //    owner.lookAtComponent.LookAtEnemy(owner.targetingComponent.target.transform.position, owner.lookAtComponent.lookAtDuration);
            //End Coroutine
            offFreeflowEvent?.Invoke();

        }
        #endregion






    }
}
