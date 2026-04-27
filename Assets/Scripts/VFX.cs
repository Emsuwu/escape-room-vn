using UnityEngine;

public class VFX : MonoBehaviour
{
    [SerializeField] private GameObject _itemPickupPrefab;
    public static VFX Instance;
    private void Awake()
    {
        Instance = this;
    }
    public static void ItemPickup(Vector3 position)
    {
        Instantiate(Instance._itemPickupPrefab, position, Quaternion.identity);
    }
}
