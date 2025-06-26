using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

[System.Serializable]
public enum EShakeDirection
{
    None = 0,
    Left = 1,
    Right = 2,
    Up = 3,
    Down = 4,
    Front = 5,
    Back = 6
}

[System.Serializable]
public enum ECameraType
{
    FollowCamera,
    LockOnCamera,
    FinisherCamera
}

public class CameraManager : MonoBehaviour
{
    public CharacterPlayer player;

    [Header("Camera Effect")]
    public CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        // Optional: chỉ cần nếu không gán impulseSource trong Editor
        if (impulseSource == null)
            impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<CharacterPlayer>();
    }

    #region Camera Effect
    public void Shake(Vector3 direction, float intensity = 1.0f)
    {
        if (impulseSource != null)
        {
            impulseSource.DefaultVelocity = direction.normalized * intensity;
            impulseSource.GenerateImpulse();
        }
    }
    #endregion
}
