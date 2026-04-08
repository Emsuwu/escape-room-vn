using UnityEngine;

public class SafeInteractable : Interactable
{
    [SerializeField] private Safe _safe;
    public void SafeInteract()
    {
        Safe.Instance.Enter();
    }
}
