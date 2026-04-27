using UnityEngine;

public class VisualEffect : MonoBehaviour
{
    [SerializeField] private GameObject _itemPickupPrefab;
    public static VisualEffect Instance;
    private void Awake()
    {
        Instance = this;
    }
    public static void ItemPickup(Vector3 position)
    {
        Instantiate(Instance._itemPickupPrefab, position, Quaternion.identity);
    }
}
