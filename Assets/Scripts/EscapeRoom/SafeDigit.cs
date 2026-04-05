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
}
