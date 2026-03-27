using UnityEngine;

public class CharacterNameBox : MonoBehaviour
{
    RectTransform _rectTransform;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        _rectTransform.position = new Vector3(0 + _rectTransform.rect.width/3.5f , _rectTransform.position.y, _rectTransform.position.z);
    }
}
