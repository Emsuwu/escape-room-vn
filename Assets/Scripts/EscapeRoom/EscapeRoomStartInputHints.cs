using System.Collections;
using UnityEngine;

public class EscapeRoomStartInputHints : MonoBehaviour
{
    private static bool _hasShownThisSession;
    void Start()
    {
        if (_hasShownThisSession) return;
        Meowtils.Defer(() => StartCoroutine(CoStartHints()));
        _hasShownThisSession = true;
    }

    IEnumerator CoStartHints()
    {
        if (Settings.IsUsingGamepad) InputHint.Instance.Show(InputHint.Instance.GamepadRightStick, "Look around");
        else InputHint.Instance.Show(InputHint.Instance.KeyboardRightWASD, "Look around");

        yield return new WaitForSeconds(8);

        InputHint.Instance.Hide(0.5f);
        yield return new WaitForSeconds(0.5f);

        if (!Settings.IsUsingGamepad) InputHint.Instance.Show(InputHint.Instance.MouseMove, "Interact with objects with your mouse");
        else
        {
            InputHint.Instance.Show(InputHint.Instance.GamepadLeftStick, "Move the reticle");
            yield return new WaitForSeconds(8);
            InputHint.Instance.Hide(0.5f);
            yield return new WaitForSeconds(0.5f);
            InputHint.Instance.Show(InputHint.Instance.GamepadSouth, "Interact with objects with the reticle");
        }
        yield return new WaitForSeconds(10);
        InputHint.Instance.Hide();
    }
}
