using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu()]
public class LevelData : ScriptableObject
{
    public Question[] Questions;
}

[Serializable]
public struct Question
{
    public string Title;
    public Number Correct;
    public string[] Questions;
    public Sprite Image;

    public enum Number
    {
        None = -1,
        One = 0,
        Two = 1,
        Three = 2,
        Four = 3
    }
}

