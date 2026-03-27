using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class VHSInteractable : Interactable
{
    public void VHSInteract()
    {
        Debug.Log("Chuck a tape in me :3");
    }
}
