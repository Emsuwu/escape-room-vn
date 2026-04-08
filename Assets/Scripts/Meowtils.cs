using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Meowtils : MonoBehaviour
{
    public static async void Defer(Action method)
    {
        await Task.Yield();
        method?.Invoke();
    }
}