using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeTrup : MonoBehaviour
{
    [SerializeField] private GameObject _trupVisuals;

    public static PlayerTakeTrup Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }

        Debug.LogError("There`s one more PlayerTakeTrup");
        Destroy(this);
    }

    public void TakeTrup()
    {
        _trupVisuals.SetActive(true);
    }

    public void DropTrup()
    {
        
    }
}