using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoint : MonoBehaviour
{
    [field: SerializeField] public float StayLenght { get; private set; }
    [field: SerializeField] public bool StayRandomTime { get; private set; }
    [field: SerializeField] public bool RotateToPointRotation { get; private set; }
}