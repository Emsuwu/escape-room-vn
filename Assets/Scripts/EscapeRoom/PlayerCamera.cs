using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;
    Coroutine _rotationCoroutine;

    float RotationClampXMin = -13f;
    float RotationClampXMax = 30f;
    float _horizontalRotation;
    float _verticalRotation;
    private void Awake()
    {
        Instance = this;

        _verticalRotation = transform.localEulerAngles.x;
        _horizontalRotation = transform.localEulerAngles.y;
    }
    public void StartRotation(Vector3 dir)
    {
        _rotationCoroutine = StartCoroutine(CoStartRotation(dir));
    }
    public void StopRotation()
    {
        if (_rotationCoroutine != null) StopCoroutine(_rotationCoroutine);
    }

    private IEnumerator CoStartRotation(Vector3 dir)
    {
        while (true) {

            _verticalRotation += dir.x * Time.fixedDeltaTime;
            _verticalRotation = Mathf.Clamp( _verticalRotation, RotationClampXMin, RotationClampXMax );
            _horizontalRotation += dir.y * Time.fixedDeltaTime;

            transform.localRotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0f);

            yield return new WaitForFixedUpdate();
        }
    }
}
