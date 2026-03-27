using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Interactable : MonoBehaviour
{
    public static Interactable CurrentlyHovered;

    [SerializeField, FormerlySerializedAs("hoverOutlines")] GameObject[] _hoverOutlines;
    [SerializeField] UnityEvent _interactionEvent;
    private void Start()
    {
        gameObject.tag = "Interactable";
        SetOutlineVisibility(false);
    }
    public void Interact()
    {
        _interactionEvent?.Invoke();
    }
    public void Hover()
    {
        SetOutlineVisibility(true);
        CurrentlyHovered = this;
    }
    public void Unhover()
    {
        SetOutlineVisibility(false);
        if (CurrentlyHovered == this) CurrentlyHovered = null;
    }
    private void SetOutlineVisibility(bool visible)
    {
        if (_hoverOutlines == null) return;
        foreach (GameObject go in _hoverOutlines) { go.SetActive(visible); }
    }
}
