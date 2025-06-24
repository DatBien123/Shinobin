using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

public class VFXManager : MonoBehaviour
{

    public void PlayPX(ParticleSystem particleSystem, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        ParticleSystem particalEffect = Instantiate(
            particleSystem,
            position,
            Quaternion.LookRotation(rotation)
        );

        particalEffect.transform.localScale = scale;
    }
}
