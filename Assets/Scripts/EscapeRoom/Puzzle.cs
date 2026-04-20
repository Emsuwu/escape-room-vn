using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle : MonoBehaviour
{
    public static Puzzle CurrentPuzzle;
    [Header("Puzzle")]
    public bool Enabled = true;
    public Camera Camera;
    public bool IsPuzzleActive { get; private set; }
    public void Enter()
    {
        if (!Enabled) return;
        IsPuzzleActive = true;
        CurrentPuzzle = this;
        PlayerCamera.Instance.Camera.enabled = false;
        Camera.enabled = true;
        PlayerInputHandler.Instance.IsInputDisabled = true;
    }
    public void Exit()
    {
        IsPuzzleActive = false;
        CurrentPuzzle = null;
        PlayerCamera.Instance.Camera.enabled = true;
        Camera.enabled = false;
        Meowtils.Defer( () => PlayerInputHandler.Instance.IsInputDisabled = false );
    }
    public virtual void PuzzleProcess() { }
    public void SendInput(
        InputAction up, 
        InputAction down, 
        InputAction left, 
        InputAction right, 
        InputAction back)
    {
        if (up.WasPressedThisFrame()) Up();
        if (down.WasPressedThisFrame()) Down();
        if (left.WasPressedThisFrame()) Left();
        if (right.WasPressedThisFrame()) Right();
        if (back.WasPressedThisFrame()) Back();
    }
    public virtual void Up() { }
    public virtual void Down() { }
    public virtual void Left() { }
    public virtual void Right() { }
    public virtual void Back() { }
    private void Update()
    {
        if (!IsPuzzleActive) return;
        PuzzleProcess();
    }
}
