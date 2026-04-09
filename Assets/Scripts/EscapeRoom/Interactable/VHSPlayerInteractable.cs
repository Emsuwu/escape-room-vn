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
    public void InsertTape()
    {
        if (_isTapeInserted) return;
        _isTapeInserted = true;
        _tapeAnim.SetActive(true);
        _tapeSound.Play();
    }
}
