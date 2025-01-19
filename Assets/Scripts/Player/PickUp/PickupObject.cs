using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour, IPickupable
{
    [field: SerializeField] public bool CanPickup { get; set; } = true;


    public void Pickuped()
    {
    }
}