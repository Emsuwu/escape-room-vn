using System.Threading.Tasks;
//using UnityEditor.Overlays;
using UnityEngine;

public class DoorInteractable : Interactable
{
    [SerializeField] Door _doorScript;
    [SerializeField] GameObject _doorOutline;
    [SerializeField] GameObject _voidWind;
    [SerializeField] GameObject _leaveHover;
    public async void OnDoorInteract()
    {
        if (SaveManager.HasFlag("KeyCollected")) {
            _doorScript.Open();
            
            _doorOutline.SetActive(false);
            _voidWind.SetActive(true);
            Enabled = false;
            await Task.Delay(2000);
            _leaveHover.SetActive(true);
        }
        else
        {
            MidscreenHint.Show("The door is locked...", 4f, 0.5f);
        }
    }
}
