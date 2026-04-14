using UnityEngine;

public class PC : MonoBehaviour
{
    private Animator _animator;
    private bool _isFloppyInserted;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void OnInteract()
    {
        if (_isFloppyInserted) return;
        if (!SaveManager.Data.IsFloppyCollected) return;
        _isFloppyInserted = true;
        _animator.SetTrigger("InsertFloppy");
    }
}
