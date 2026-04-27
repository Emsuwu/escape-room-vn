using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyAfterTimeEnabled : MonoBehaviour
{
    [SerializeField] private float _secondsUntilDestroy;
    private void OnEnable()
    {
        StartCoroutine(DisableAfterSeconds(_secondsUntilDestroy));
    }

    IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
