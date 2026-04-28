using UnityEngine;
using UnityEngine.UI;

public class TitleScreenImageUpdater : MonoBehaviour
{
    [SerializeField] private Image _titleScreenImage;
    [SerializeField] private Sprite _urbexCompletionSprite;
    [SerializeField] private Sprite _unicornCompletionSprite;
    [SerializeField] private Sprite _unicornDealMadeSprite;
    [SerializeField] private Sprite _adventureCompletionSprite;
    
    void Start()
    {
        if (SaveManager.HasFlag("UnicornDealMade")) { _titleScreenImage.sprite = _unicornDealMadeSprite; }
        else if (SaveManager.HasFlag("AdventureComplete")) { _titleScreenImage.sprite = _adventureCompletionSprite; }
        else if (SaveManager.HasFlag("UnicornStoryComplete")) { _titleScreenImage.sprite = _unicornCompletionSprite; }
        else if (SaveManager.HasFlag("UrbexStoryComplete")) { _titleScreenImage.sprite = _urbexCompletionSprite; }
    }
}
