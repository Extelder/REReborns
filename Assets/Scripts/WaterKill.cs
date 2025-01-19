using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterKill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Water>(out Water water))
        {
            PlayerDialogue.Instance.Death();
        }
    }
}
