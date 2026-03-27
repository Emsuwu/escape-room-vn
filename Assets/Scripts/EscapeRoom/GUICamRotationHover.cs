using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUICamRotationHover : MonoBehaviour
{
    EventTrigger _eventTrigger;
    Image _image;
    [SerializeField] TMPro.TMP_Text _arrows;
    [SerializeField] Vector3 _rotationDirection;


    private void Start()
{
        _image = GetComponent<Image>();
        _eventTrigger = gameObject.AddComponent<EventTrigger>();

        // Add hover listener (calls Hover method)
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { Hover(); });
        _eventTrigger.triggers.Add(entry);
        // Add unhover listener (calls Hover method)
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((eventData) => { Unhover(); });
        _eventTrigger.triggers.Add(entry);

        // Make transparent on start
        _image.CrossFadeAlpha(0f, 0f, false);
        _arrows.CrossFadeAlpha(0f, 0f, false);


    }

    private void Hover()
    {
        _image.CrossFadeAlpha(.6f, .2f, false);
        _arrows.CrossFadeAlpha(.6f, .2f, false);
        PlayerCamera.Instance.StartRotation(_rotationDirection);
    }
    private void Unhover()
    {
        _image.CrossFadeAlpha(0f, .2f, false);
        _arrows.CrossFadeAlpha(0f, .2f, false);
        PlayerCamera.Instance.StopRotation();
    }

}
