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

[System.Serializable]
public struct TargetGroupData
{
    public float weightPlayer;
    public float radiusPlayer;
    public float weightEnemy;
    public float radiusEnemy;
}

public class CameraManager : MonoBehaviour
{
    public CharacterPlayer player;

    public CinemachineCamera currentCineCam;
    public ECameraType currentCameraType = ECameraType.FollowCamera;

    [Header("Follow Camera")]
    public CinemachineCamera followCineCam;
    [Header("Finisher Camera")]
    public CinemachineCamera finisherCineCam;
    public CinemachineTargetGroup finisherTargetGroup;
    public TargetGroupData targetGroupData;
    [Header("Lock On Camera")]
    public CinemachineCamera lockOnCineCam;


    private void Awake()
    {
        
    }

    private void Start()
    {
    }

    public void SwitchCamera(ECameraType type)
    {

        ResetCameraState();
        switch (type)
        {
            case ECameraType.FollowCamera:
                currentCineCam = followCineCam;
                currentCameraType = ECameraType.FollowCamera;
                followCineCam.Priority = 1;

                break;
            case ECameraType.FinisherCamera:
                currentCineCam = finisherCineCam;
                currentCameraType = ECameraType.FinisherCamera;
                finisherCineCam.Priority= 1;

                finisherTargetGroup.AddMember(player.characterRigComponent.cameraFinisherSocket, targetGroupData.radiusPlayer, targetGroupData.weightPlayer);
                finisherTargetGroup.AddMember(player.targetingComponent.target.gameObject.GetComponent<Character>().characterRigComponent.cameraFinisherSocket, targetGroupData.radiusEnemy, targetGroupData.weightEnemy);
                break;
            case ECameraType.LockOnCamera:
                currentCineCam = lockOnCineCam;
                currentCameraType = ECameraType.LockOnCamera;
                lockOnCineCam.Priority = 1;

                lockOnCineCam.Follow = player.characterRigComponent.cameraLookAtSocket;
                lockOnCineCam.LookAt = player.targetingComponent.target.gameObject.GetComponent<Character>().characterRigComponent.cameraLookAtSocket;

                //player.isApplyingLock = true;
                break;

        }
    }

    public void ResetCameraState()
    {
        //Reset All priority
        followCineCam.Priority = 0;

        finisherCineCam.Priority = 0;
        finisherTargetGroup.Targets.Clear();

        lockOnCineCam.Priority = 0;
        //player.isApplyingLock = false;
    }




    #region [Camera Effect]
    public void Shake(Vector3 direction, float intensity = 1.0f)
    {
        if (currentCineCam.GetComponent<CinemachineImpulseSource>() != null)
        {
            currentCineCam.GetComponent<CinemachineImpulseSource>().DefaultVelocity = direction.normalized * intensity;
            currentCineCam.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        }
    }
    #endregion
}
