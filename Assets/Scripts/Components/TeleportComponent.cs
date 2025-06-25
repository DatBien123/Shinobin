using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TeleportComponent : MonoBehaviour
{
    public Character owner;
    public LayerMask groundLayer;
    public float radius = 0.5f;
    public float maxDistance = 10f;

    [Header("Events")]
    public UnityEvent onTeleportEvent;
    public UnityEvent offTeleportEvent;

    private void Awake()
    {
        owner = GetComponent<Character>();
    }

    public bool IsTeleportPositionValid(Vector3 teleportPosition)
    {
        Vector3 origin = owner.transform.position + Vector3.up * 0.5f;
        Vector3 direction = (teleportPosition - origin).normalized;
        float distance = Vector3.Distance(origin, teleportPosition);

        // Bước 1: Kiểm tra đường đi có vật cản hay không
        if (Physics.SphereCast(origin, radius, direction, out RaycastHit hit, distance, groundLayer))
        {
            Debug.Log("Teleport bị chặn bởi chướng ngại vật");
            return false; // Có vật cản
        }

        // Bước 2: Kiểm tra vị trí có tiếp xúc mặt đất không
        if (!Physics.CheckSphere(teleportPosition + Vector3.up * 0.1f, radius, groundLayer))
        {
            Debug.Log("Teleport không tiếp xúc với mặt đất");
            return false; // Không tiếp xúc mặt đất
        }

        return true;
    }

    public Vector3 GetClosestValidPosition(Vector3 targetPosition)
    {
        float searchRadius = 0.5f;
        int maxAttempts = 20;
        float offset = 0.2f;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * offset;
            Vector3 checkPosition = targetPosition + randomOffset;

            if (IsTeleportPositionValid(checkPosition))
            {
                Debug.Log($"Tìm thấy vị trí hợp lệ ở lần thử thứ {i + 1}");
                return checkPosition;
            }
        }

        Debug.LogWarning("Không tìm thấy vị trí hợp lệ");
        return targetPosition; // Trả về vị trí gốc nếu không tìm thấy
    }

    public void StartTeleport(Vector3 teleportPosition, float duration)
    {
        Vector3 finalPosition = IsTeleportPositionValid(teleportPosition)
                                ? teleportPosition
                                : GetClosestValidPosition(teleportPosition);

        StartCoroutine(TeleportAI(finalPosition, duration));
    }

    IEnumerator TeleportAI(Vector3 teleportPosition, float duration)
    {
        float elapsedTime = 0.0f;
        //(owner as CharacterAI).navMeshAgent.enabled = false;
        (owner as CharacterAI).currentBehaviorState = Training.EBehaviorState.Executing;
        owner.currentCombatState = ECombatState.Untouchable;

        //owner.lookAtComponent.LookAtEnemy(owner.targetingComponent.target.transform.position, .2f);
        //owner.animator.SetFloat(AnimationParams.Input_Horizontal_Dash_Param, 0.0f);
        //owner.animator.SetFloat(AnimationParams.Input_Vertical_Dash_Param, -1.0f);

        owner.animator.CrossFadeInFixedTime(AnimationParams.Teleport_State, .1f);

        yield return new WaitForSeconds(.44f);
        onTeleportEvent?.Invoke();


        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        owner.transform.position = teleportPosition; // Thực hiện teleport
        offTeleportEvent?.Invoke();

        //owner.lookAtComponent.LookAtEnemy(owner.targetingComponent.target.transform.position, .2f);

        owner.currentCombatState = ECombatState.None;

        //(owner as CharacterAI).navMeshAgent.enabled = true;
        (owner as CharacterAI).currentBehaviorState = Training.EBehaviorState.Finished;
    }
}