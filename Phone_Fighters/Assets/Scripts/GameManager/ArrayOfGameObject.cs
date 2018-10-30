using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ArrayOfGameObject : IEnumerable
{
    [SerializeField]
    GameObject[] obj = new GameObject[0];
    [SerializeField]
    AudioClip audio;
    public AudioClip Audio { get { return audio; } }
    public int Count { get { return obj.Length; } }

    public IEnumerator GetEnumerator()
    {
        return obj.GetEnumerator();
    }
}
