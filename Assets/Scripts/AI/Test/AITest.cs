using UnityEngine;

public class AITest : MonoBehaviour
{
    public Character owner;
    private void Awake()
    {
        owner = GetComponent<Character>();
    }

    private void Start()
    {
        owner.animator.SetBool(AnimationParams.Block_Param, owner.isBlock);
    }

    private void Update()
    {
        //if(owner.isBlock != owner.animator.GetBool(AnimationParams.Block_Param))
        //{
        //    owner.animator.SetBool(AnimationParams.Block_Param, owner.isBlock);
        //}
    }


}
