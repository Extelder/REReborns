using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private int _sceneId;

    public void Switch()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_sceneId);
    }
}