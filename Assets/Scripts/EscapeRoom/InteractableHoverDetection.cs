using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableHoverDetection : MonoBehaviour
{
    private Interactable cachedInteractable;
    void Update()
    {
        CheckRaycast();
    }

    void CheckRaycast()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit)) return;

        if (hit.collider.CompareTag("Interactable"))
        {
            HoverInteractable(hit);
        }
        else
        {
            UnhoverInteractable();
        }
        if (cachedInteractable != hit.collider.gameObject.GetComponent<Interactable>())
        {
            UnhoverInteractable();
        }
    }

    void HoverInteractable(RaycastHit hit)
    {
        if (cachedInteractable == null)
        {
            cachedInteractable = hit.collider.gameObject.GetComponent<Interactable>();
        }
        cachedInteractable.Hover();
    }
    void UnhoverInteractable()
    {
        if (cachedInteractable != null)
        {
            cachedInteractable.Unhover();
            cachedInteractable = null;
        }
    }
}
