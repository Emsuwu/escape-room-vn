using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeImage : MonoBehaviour
{
    public static SceneFadeImage Instance { get; private set; }
    [SerializeField] private bool _fadeOnStart;
    private Image _image;
    private void Awake()
    {
        Instance = this;
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        if (_fadeOnStart)
        {
            FadeFromBlack(1.5f, 1f);
        }
        else
        {
            FadeFromBlack(0f);
        }
    }
    public async void FadeFromBlack(float duration, float delay = 0)
    {
        await Task.Delay((int)(delay * 1000));
        _image.CrossFadeAlpha(0, duration, true);
    }
    public void FadeToBlack(float duration)
    {
        _image.CrossFadeAlpha(1, duration, true);
    }
    public async void FadeToBlackThenExecute(Action method, float duration, float executionDelay = -1)
    {
        if (executionDelay == -1) { executionDelay = duration; }
        _image.CrossFadeAlpha(1, duration, true);
        await Task.Delay((int)(executionDelay * 1000));
        method?.Invoke();
    }
}
