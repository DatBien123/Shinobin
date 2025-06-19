using UnityEngine;
using System.Collections.Generic;

namespace Training
{
    [System.Serializable]
    public struct SoundData
    {
        public string Name;
        public AudioClip audioClip;
        public Transform soundPlace;
        public float volume;
        public float pitch;
    }
    [CreateAssetMenu(fileName = "SO_SoundFXData", menuName = "Sound /Sound Data")]
    public class SO_SoundFXData : ScriptableObject
    {
        public List<SoundData> soundDatas;
    }
}
