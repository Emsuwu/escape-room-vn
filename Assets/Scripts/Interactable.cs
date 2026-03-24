using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    [SerializeField] UnityEvent _interactionEvent;
    private void Start()
    {
        gameObject.tag = "Interactable";
    }
    public void Interact()
    {
        _interactionEvent?.Invoke();
    }
    public void Hover()
    {
        Debug.Log(name + " hovered");
    }
    public void Unhover()
    {
        Debug.Log(name + " unhovered");
    }
}
