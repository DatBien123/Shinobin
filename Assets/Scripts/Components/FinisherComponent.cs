using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Training;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

public class FinisherComponent : MonoBehaviour
{
    public Character owner;


    public SO_Finisher currentFinisherData;

    [Header("Loaded Finisher")]
    public List<SO_Finisher> loadedFinisher;
    Coroutine C_Finsher;
    public AudioClip deathBlowSound;


    private void Awake()
    {
        owner = GetComponent<Character>();
        LoadFinisher();
    }
    public void LoadFinisher()
    {
        if (owner.currentWeapon != null)
        {
            var finisherDatas = Resources.LoadAll<SO_Finisher>($"Finisher/{owner.currentWeapon.weaponType}");
            foreach (var data in finisherDatas)
            {
                loadedFinisher.Add(data);
            }
        }
        else
        {
            var finisherDatas = Resources.LoadAll<SO_Finisher>($"Finisher/None");
            foreach (var data in finisherDatas)
            {
                loadedFinisher.Add(data);
            }
        }
    }

    private SO_Finisher FilterFinisher()
    {
        List<SO_Finisher> filterFinishers = loadedFinisher;

        foreach (var finisher in filterFinishers)
        {
            //Ke dich co target vao nguoi choi
            if (owner.targetingComponent.target.gameObject.GetComponent<Character>().targetingComponent.target != null)
            {
                if(finisher.data.finisherType == EFinisherType.Execute) return finisher;
            }
            else
            {
                //Ke dich KHONG co target vao nguoi choi
                if (finisher.data.finisherType == EFinisherType.Ambush) return finisher;
            }
        }

        return null;
    }
    #region [Test]
    public void StartPlayFinisher(Character taker)
    {
        currentFinisherData = FilterFinisher();
        if (currentFinisherData == null) return;
        if(C_Finsher == null)C_Finsher = StartCoroutine(PlayFinisher(taker));
    }

    IEnumerator PlayFinisher(Character taker)
    {
        Sequence seq = DOTween.Sequence();

        (owner as CharacterPlayer)._input.playerControls.Disable();
        (taker as CharacterAI).behavDecesion.StopExecuteBehaviors();

        //SFX
        owner.hitReactionComponent.PlaySoundUnscaled(deathBlowSound, transform.position, 1, 1.5f);
        //camera
        owner.hitReactionComponent.cameraManager.SwitchCamera(ECameraType.FinisherCamera);
        Vector3 takerTargetPos = owner.transform.position
                                + owner.transform.forward * currentFinisherData.data.takerData.takerPositionOffset.z
                                + owner.transform.right * currentFinisherData.data.takerData.takerPositionOffset.x
                                + owner.transform.up * currentFinisherData.data.takerData.takerPositionOffset.y;

        seq
            .Join(transform.DOLookAt(taker.transform.position, 0.1f))
            .Join(taker.transform.DOLookAt(owner.transform.position, 0.1f))
            .Join(taker.transform.DOMove(takerTargetPos, 0.1f))
            .OnComplete(() =>
            {
                Debug.Log("Đã di chuyển và xoay xong!");
                owner.animator.CrossFadeInFixedTime(currentFinisherData.data.causerData.executionClip.name, 0.2f);
                taker.animator.CrossFadeInFixedTime(currentFinisherData.data.takerData.executedClip.name, 0.2f);
            });

        yield return new WaitForSeconds(currentFinisherData.data.causerData.executionClip.length + .5f);

        owner.hitReactionComponent.cameraManager.SwitchCamera(ECameraType.FollowCamera);

        (owner as CharacterPlayer)._input.playerControls.Enable();
        (taker as CharacterAI).behavDecesion.StartExecuteBehaviors();

        yield return new WaitForSeconds(1.0f);
        C_Finsher = null;
    }
    #endregion

}
