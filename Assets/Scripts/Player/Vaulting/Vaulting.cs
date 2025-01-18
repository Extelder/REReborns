using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Vaulting : MonoBehaviour
{
    [SerializeField] private LayerMask _checkLayerMask;
    [SerializeField] private Collider _checkVaultingCollider;

    private CharacterController _characterController;
    private CompositeDisposable _disposable = new CompositeDisposable();

    public Camera cam;
    private float playerHeight = 2.2f;
    private float playerRadius = 0.5f;

    private bool _vaulitng;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vault();
    }

    private void OnEnable()
    {
        _checkVaultingCollider.OnTriggerStayAsObservable().Subscribe(other =>
        {
            if (other != null)
            {
                if (!_vaulitng)
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        print("vaultable in front");

                        if (Physics.Raycast(
                            transform.position + (cam.transform.forward * playerRadius) + (Vector3.up * playerHeight),
                            Vector3.down * 2, out var secondHit, playerHeight))
                        {
                            _vaulitng = true;
                            print("found place to land");
                            StartCoroutine(LerpVault(secondHit.point, 0.5f));
                        }
                    }
            }
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }


    private void Vault()
    {
    }

    private IEnumerator LerpVault(Vector3 targetPosition, float duration)
    {
        _characterController.enabled = false;
        float time = 0;
        Vector3 startPosition = transform.position;

        targetPosition.y += playerHeight / 2;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        _vaulitng = false;

        _characterController.enabled = true;
    }
}