using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle : MonoBehaviour
{
    public static Puzzle CurrentPuzzle;
    public Camera Camera;
    public bool IsPuzzleActive { get; private set; }
    public void EnablePuzzleScreen()
    {
        IsPuzzleActive = true;
        CurrentPuzzle = this;
        PlayerCamera.Instance.Camera.enabled = false;
        Camera.enabled = true;
        PlayerInputHandler.Instance.IsInputDisabled = true;
    }
    public void DisablePuzzleScreen()
    {
        IsPuzzleActive = false;
        CurrentPuzzle = null;
        PlayerCamera.Instance.Camera.enabled = true;
        Camera.enabled = false;
        PlayerInputHandler.Instance.IsInputDisabled = false;
    }
    public virtual void PuzzleProcess() { }
    public void SendInput(InputAction up, InputAction down, InputAction left, InputAction right)
    {
        if (up.WasPressedThisFrame()) Up();
        if (down.WasPressedThisFrame()) Down();
        if (left.WasPressedThisFrame()) Left();
        if (right.WasPressedThisFrame()) Right();
    }
    public virtual void Up() { }
    public virtual void Down() { }
    public virtual void Left() { }
    public virtual void Right() { }
    private void Update()
    {
        if (!IsPuzzleActive) return;

        PuzzleProcess();
    }
}
