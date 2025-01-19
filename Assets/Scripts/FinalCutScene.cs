using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCutScene : MonoBehaviour
{
    [SerializeField] private Animator _cutSceneAnimator;
    [SerializeField] private GameObject _tites;
    [SerializeField] private GameObject[] _deadObjectToFinale;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Final", 0) == 1)
        {
            _tites.SetActive(true);
        }

        StartCoroutine(CheckingForFinal());
    }

    private IEnumerator CheckingForFinal()
    {
        yield return new WaitUntil(() => Dead());
        Final();
    }

    private bool Dead()
    {
        for (int i = 0; i < _deadObjectToFinale.Length; i++)
        {
            if (_deadObjectToFinale[i] != null)
                return false;
        }

        return true;
    }

    public void Final()
    {
        PlayerDialogue.Instance.Death();
        _cutSceneAnimator.SetBool("Final", true);
        PlayerPrefs.SetInt("Final", 1);
    }
}