using UnityEngine;

public class RapierPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _playerRapier;

    public void Interact()
    {
        _playerRapier.SetActive(true);
        Destroy(gameObject);
    }
}