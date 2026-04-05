using System.Collections;
using UnityEngine;

public class SafeDigit : MonoBehaviour
{
    public int Value { get; private set; } = 0;
    private const float EachIncrement = 36f;
    private const float TurnSpeed = 75f;
    private Coroutine _coTurnInstance;
    public void Increment()
    {
        //if (_coTurnInstance != null) return;
        //_coTurnInstance = StartCoroutine(CoTurn());
        transform.Rotate(EachIncrement, 0, 0);
        Value -= 1;
        if (Value<0) Value = 9;
    }
    public void Decrement()
    {
        //if (_coTurnInstance != null) return;
        //_coTurnInstance = StartCoroutine(CoTurn(-1));
        transform.Rotate(-EachIncrement, 0, 0);
        Value = (Value + 1) % 10;
    }

    //private IEnumerator CoTurn(int turnDir = 1)
    //{
    //    //float totalAmountTurned = 0f;

    //    //while (Mathf.Abs(totalAmountTurned) < EachIncrement)
    //    //{
    //    //    float turnAmount = turnDir * Time.fixedDeltaTime * TurnSpeed;
    //    //    totalAmountTurned += turnAmount;
    //    //    transform.Rotate(turnAmount, 0, 0);
    //    //    yield return new WaitForFixedUpdate();
    //    //}

    //    //CurrentNum -= turnDir % 10;
    //    //if (CurrentNum < 0) { CurrentNum = 9; }
    //    //Debug.Log($"Rot: {transform.rotation}, Local Rot: {transform.localRotation}");
    //    //Debug.Log(CurrentNum);
    //    Debug.Log("YES");
    //    transform.Rotate(EachIncrement * turnDir, 0, 0);
    //    _coTurnInstance = null;
    //    yield return null;
    //}
}
