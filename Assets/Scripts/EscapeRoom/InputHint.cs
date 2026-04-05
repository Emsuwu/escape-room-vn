using UnityEngine;

public class InputHint : MonoBehaviour
{
    public static InputHint Instance { get; private set; }

    public Sprite GamepadEast;

    private void Awake()
    {
        Instance = this;
    }
}
