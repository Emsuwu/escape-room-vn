using System.Collections;
using UnityEngine;

public class MidscreenHint : MonoBehaviour
{
    private TMPro.TMP_Text _text;
    public static MidscreenHint Instance;

    private void Awake()
    {
        Instance = this;
        _text = GetComponent<TMPro.TMP_Text>();
        _text.CrossFadeAlpha(0, 0, true);
    }

    public static void Show(string text, float duration, float fadeTime = 0.5f)
    {
        MidscreenHint.Instance.StartCoroutine(
            MidscreenHint.Instance.ShowText(text, duration, fadeTime)
            );
    }
    public IEnumerator ShowText(string text, float duration, float fadeTime = 0.5f)
    {
        _text.text = text;
        _text.CrossFadeAlpha(1, fadeTime, false);
        yield return new WaitForSeconds(fadeTime);
        yield return new WaitForSeconds(duration);
        _text.CrossFadeAlpha(0, fadeTime, false);
        yield return new WaitForSeconds(fadeTime);
        _text.text = string.Empty;
    }
}
