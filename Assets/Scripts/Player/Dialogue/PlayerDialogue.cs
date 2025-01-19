using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private float _checkRange;
    [SerializeField] private LayerMask _checkLayerMask;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private PlayerInteract _interact;

    [SerializeField] private Transform _deathCamera;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _deathMenu;

    private bool _death;

    public static PlayerDialogue Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }

        Debug.LogError("There`s one more PlayerDialogue");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_checkPoint.position, _checkRange);
    }

    public void Death()
    {
        if (_death)
            return;
        _interact.enabled = false;
        Collider[] others = Physics.OverlapSphere(_checkPoint.position, _checkRange, _checkLayerMask);
        _deathCamera.gameObject.SetActive(true);
        _deathCamera.SetParent(null);
        _deathCamera.parent = null;
        _player.SetActive(false);
        for (int i = 0; i < others.Length; i++)
        {
            if (others[i] == null)
                continue;
            if (others[i].TryGetComponent<DiaolgueAfterPlayeDeath>(out DiaolgueAfterPlayeDeath DiaolgueAfterPlayeDeath))
            {
                _death = true;
                StartCoroutine(Dialoguing(DiaolgueAfterPlayeDeath.Dialogue));
                return;
            }
        }

        Invoke(nameof(DeathMenuInvoke), 1f);
    }

    private void DeathMenuInvoke()
    {
        _deathMenu.SetActive(true);
    }

    private IEnumerator Dialoguing(Dialogue dialogue)
    {
        _audioSource.clip = dialogue.AudioClip;
        _audioSource.Play();
        yield return new WaitForSeconds(dialogue.Length);
        dialogue.DialogueEndedEvent.Invoke();
        _deathMenu.SetActive(true);
    }
}