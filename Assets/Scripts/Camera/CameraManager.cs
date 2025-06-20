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
    [Header("Virtuals Cameras")]

    public Animator cameraAnimator;
    public CinemachineCamera currentCamera;
    public CinemachineCamera followCamera;
    public CinemachineCamera lockOnCamera;
    public CinemachineCamera finisherCamera;

    public ECameraType currentCameraType;

    [Header("Camera Effect")]
    public CinemachineImpulseSource impulseSource;
    private void Awake()
    {
        cameraAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<CharacterPlayer>();
        SwitchCamera(ECameraType.FollowCamera);
    }
    #region Camera
    public void SwitchCamera(ECameraType cameraType)
    {
        switch (cameraType) { 
            case ECameraType.FollowCamera:
                FollowCameraStuff();
                break;
            case ECameraType.LockOnCamera:
                LockOnTargetStuffs();
                break;
            case ECameraType.FinisherCamera:
                FinisherCameraStuffs();
                break;
            default:

                break;

        }
    }
    public void FixedUpdate()
    {
        currentCamera = finisherCamera;
    }
    public void FollowCameraStuff()
    {
        currentCamera = followCamera;
        currentCamera.Follow = player.characterRigComponent.cameraFollowSocket;
        currentCamera.LookAt = player.characterRigComponent.cameraLookAtSocket;
        cameraAnimator.CrossFadeInFixedTime(AnimationParams.Camera_Follow_State, .1f);
        currentCameraType = ECameraType.FollowCamera;
    }
    public void LockOnTargetStuffs()
    {
        //CharacterEnemy target = player.targetingComponent.DetectHitEnemy();
        Transform lookAtTarget = null;
        //if(target != null)
        //{
        //    lookAtTarget = target.characterRigComponent.cameraLookAtSocket.transform;
        //}

        if (lookAtTarget != null && currentCameraType != ECameraType.LockOnCamera) {
            currentCamera = lockOnCamera;
            currentCamera.Follow = player.characterRigComponent.cameraFollowSocket;
            currentCamera.LookAt = lookAtTarget;
            cameraAnimator.CrossFadeInFixedTime(AnimationParams.Camera_LockOn_State, .1f);
            currentCameraType = ECameraType.LockOnCamera;

            //player.targetingComponent.StartValidatedLockOnTarget();
        }
        else 
        {
            SwitchCamera(ECameraType.FollowCamera);
        }
    }
    public void FinisherCameraStuffs()
    {
        currentCamera = finisherCamera;

        currentCamera.Follow = player.characterRigComponent.cameraFollowSocket;
        //currentCamera.LookAt = player.targetingComponent.target.characterRigComponent.cameraFinisherSocket;
        
        cameraAnimator.CrossFadeInFixedTime(AnimationParams.Camera_Finisher_State, .1f);
        currentCameraType = ECameraType.FinisherCamera;
    }
    #endregion

    #region Camera Effect
    //Camera Shake
    public void Shake(Vector3 direction, float intensity = 1.0f)
    {
        impulseSource = currentCamera.GetComponent<CinemachineImpulseSource>();
        if (impulseSource != null)
        {
            impulseSource.DefaultVelocity = direction * intensity;
            impulseSource.GenerateImpulse();
        }
    }
    #endregion
}
