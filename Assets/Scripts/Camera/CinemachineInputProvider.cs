using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CinemachineInputProvider : MonoBehaviour
{
    [Header("Cinemachine Axis Controls")]
    public CinemachineFreeLookModifier freeLookCamera; // Tham chiếu tới FreeLook Camera
    public Vector2 lookInput;

    void Awake()
    {
        freeLookCamera = GetComponent<CinemachineFreeLookModifier>();
    }

}