using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    public bool CanPickup { get; set; }
    
    public void Pickuped();
}
