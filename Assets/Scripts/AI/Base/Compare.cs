using System;
using System.Collections.Generic;
using UnityEngine;

namespace Training
{
    [Serializable]
    public enum EBoolType
    {
        None = 0,

        Ground,
        TargetGround,
        HasTarget,
        Attackable,
        Dodgeable,
        Jumpable,
        Blockable,
        Shootable,
        Walkable,
        Runable

    }
    [Serializable]
    public enum EIntType
    {
        None = 0
    }
    [Serializable]
    public enum EFloatType
    {
        None = 0,

        Distance,
        CurrentHP,
        AngleToTarget
    }
    [Serializable]
    public enum  ECompareType
    {
        None = 0,

        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        Equal
    }

    [Serializable]
    public struct BoolValue
    {
        [Header("Bool Value")]
        public EBoolType variableType;
        public ECompareType compareType;
        public bool value;
    }
    [Serializable]
    public struct IntValue
    {
        [Header("Bool Value")]
        public EIntType variableType;
        public ECompareType compareType;
        public int value;
    }
    [Serializable]
    public struct FloatValue
    {
        [Header("Float Value")]
        public EFloatType variableType;
        public ECompareType compareType;
        public float value;
    }

    [Serializable]
    public struct Condition
    {
        [Header("[Condition]")]
        public List<BoolValue> boolValue;
        public List<IntValue> intValue;
        public List<FloatValue> floatValue;
    }
}
