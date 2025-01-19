using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiaolgueAfterPlayeDeath : MonoBehaviour
{
    [field: SerializeField] public Dialogue Dialogue { get; private set; }
}

[System.Serializable]
public class Dialogue
{
    public AudioClip AudioClip;
    public float Length;
    public UnityEvent DialogueEndedEvent;
}