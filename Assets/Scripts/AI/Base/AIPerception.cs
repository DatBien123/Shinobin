using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public struct PlayerBehaviorsData
{
    public int dodgeTimes;
    public bool isAlwaysKeepDistance;
    public int attackTimes;
    public int jumpTimes;
}
[System.Flags]
public enum EPlayerTaticalPrediction
{
    PrefDodge,
    PrefComboAttack,
    PrefSkill,
    PrefKeepDistance
}



public class AIPerception : MonoBehaviour
{
    public CharacterAI ownerAI;

    public Character currentTarget;

    public List<PlayerBehaviorsData> playerBehaviorsDataHistory;

    public PlayerBehaviorsData currentPlayerBehaviorsData;

    public float taticalPredictTime = 5.0f;

    Coroutine C_PredictPlayerTatical;

    private void Awake()
    {
        ownerAI = GetComponent<CharacterAI>();
        //currentTarget = ownerAI.targetingComponent.target;
    }

    public void StartTaticalPrediction()
    {
        if(C_PredictPlayerTatical != null)
        {
            StopCoroutine(C_PredictPlayerTatical);
        }
        C_PredictPlayerTatical = StartCoroutine(TaticalPredict());
    }

    IEnumerator TaticalPredict()
    {

        yield return null;
    }


}
