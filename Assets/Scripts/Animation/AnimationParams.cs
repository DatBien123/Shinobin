using UnityEngine;

public static class AnimationParams
{
    ////////////////////////////////1.CHARACTERS STATES //////////////////////////////////////  


    //_animIDSpeed = Animator.StringToHash("Speed");
    //    _animIDGrounded = Animator.StringToHash("Grounded");
    //    _animIDJump = Animator.StringToHash("Jump");
    //    _animIDFreeFall = Animator.StringToHash("FreeFall");
    //    _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

    public static readonly int Speed_Param = Animator.StringToHash("Speed");
    public static readonly int IsGround_Param = Animator.StringToHash("Grounded");
    public static readonly int Jump_Param = Animator.StringToHash("Jump");
    public static readonly int FreeFall_Param = Animator.StringToHash("FreeFall");
    public static readonly int Motion_Speed_Param = Animator.StringToHash("MotionSpeed");
    public static readonly int HasInputMove_Param = Animator.StringToHash("HasInputMove");







    //+ Intro States
    public static readonly int Sitting_State = Animator.StringToHash("Sitting");
    public static readonly int Sit_To_Stand_State = Animator.StringToHash("Sit To Stand");
    //+ Movement States
    
    public static readonly int Jump_Start_State = Animator.StringToHash("Jump Start");
    public static readonly int Double_Jump_Start_State = Animator.StringToHash("Double Jump Start");
    public static readonly int Locomotion_State = Animator.StringToHash("Locomotion");
    public static readonly int Dodge_State = Animator.StringToHash("Dodge");
    public static readonly int Dash_State = Animator.StringToHash("Dash");
    public static readonly int Turn_State = Animator.StringToHash("Turn");
    public static readonly int Teleport_State = Animator.StringToHash("Teleport");
    public static readonly int Stagger_State = Animator.StringToHash("Stagger");
    public static readonly int Death_State = Animator.StringToHash("Death");
    public static readonly int Recover_Stagger_State = Animator.StringToHash("Stagger_Recovery");


    public static readonly int Input_Horizontal_Param = Animator.StringToHash("Horizontal");
    public static readonly int Input_Vertical_Param = Animator.StringToHash("Vertical");

    public static readonly int Input_Horizontal_Dodge_Param = Animator.StringToHash("Dodge Horizontal");
    public static readonly int Input_Vertical_Dodge_Param = Animator.StringToHash("Dodge Vertical");

    public static readonly int Input_Vertical_Turn_Param = Animator.StringToHash("Turn Vertical");
    public static readonly int Input_Horizontal_Turn_Param = Animator.StringToHash("Turn Horizontal");

    public static readonly int Input_Horizontal_Dash_Param = Animator.StringToHash("Dash Horizontal");
    public static readonly int Input_Vertical_Dash_Param = Animator.StringToHash("Dash Vertical");



    //+ Combat State
    public static readonly int Block_Start_State = Animator.StringToHash("Block_Start");
    public static readonly int Block_End_State = Animator.StringToHash("Block_End");
    public static readonly int Parry_Trigger = Animator.StringToHash("Parry");
    public static readonly int Parry_Weak_React = Animator.StringToHash("Parry Weak");
    public static readonly int Parry_Strong_React = Animator.StringToHash("Parry Strong");


    //+ Float
    public static readonly int Dodge_Speed_Param = Animator.StringToHash("Dodge Speed");
    public static readonly int Random_Param = Animator.StringToHash("Random");
    public static readonly int Stop_Factor_Param = Animator.StringToHash("Stop Factor");

    //+ Trigger
    public static readonly int Stop_Trigger = Animator.StringToHash("Stop Trigger");
    public static readonly int Turn_Trigger = Animator.StringToHash("Turn Trigger");
    public static readonly int Dodge_Trigger = Animator.StringToHash("Dodge Trigger");

    //+ Bool
    public static readonly int Block_Param = Animator.StringToHash("isBlocking");

    ////////////////////////////////2. CAMERA STATES //////////////////////////////////////  

    public static readonly int Camera_LockOn_State = Animator.StringToHash("LockOnState");
    public static readonly int Camera_Follow_State = Animator.StringToHash("FollowState");
    public static readonly int Camera_Finisher_State = Animator.StringToHash("FinisherState");
}
