using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyObjectDestroy : HitBox
{
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private PlaySound _sound;
    [SerializeField] private GameObject _hat;
    public override void Hit(Rapier rapier)
    {
        for (int i = 0; i < _objects.Length; i++)
        {
            Destroy(_objects[i]);
        }
        _hat.SetActive(false);
        _sound.PlayAudio();
        base.Hit(rapier);
    }
}
