using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class VHSPlayerInteractable : Interactable
{
    [SerializeField] private GameObject _tapeAnim;
    private AudioSource _tapeSound;
    private bool _isTapeInserted;
    private new void Start()
    {
        base.Start();
        _tapeSound = GetComponent<AudioSource>();
    }
    public async void InsertTape()
    {
        if (_isTapeInserted) return;
        if (!SaveManager.Data.IsTapeCollected) return;
        _isTapeInserted = true;
        _tapeAnim.SetActive(true);
        _tapeSound.Play();
        await Task.Delay(1250);
        Dialog.StartDialogScene("Urbex/1", "start");
    }
}
