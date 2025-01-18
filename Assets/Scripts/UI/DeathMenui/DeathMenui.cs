using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenui : MonoBehaviour
{
    [SerializeField] private GameObject _deathPanel;

    public static DeathMenui Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }

        Debug.LogError("There`s one more DeathMenu");
    }

    public void Death()
    {
        _deathPanel.SetActive(true);
        Time.timeScale = 0;
    }
}