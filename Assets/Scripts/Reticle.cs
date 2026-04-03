using UnityEngine;

public class Reticle : MonoBehaviour
{
    public static Reticle Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void Move(float x, float y)
    {
        transform.position += new Vector3(x, y, 0);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, 0, Screen.width), Mathf.Clamp(transform.position.y, 0, Screen.height), 0
            );
    }
}
