using UnityEngine;

public class GameInput : MonoBehaviour
{

    public static GameInput Instance { get; private set; }



    private InputActions inputActions;

    private void Awake()
    {
        Instance = this;
        inputActions = new InputActions();
        inputActions.Enable();
    }


    public bool IsUpPressed()
    {
        return inputActions.Player.Up.IsInProgress();
    }


    public bool IsRightPressed()
    {
        return inputActions.Player.Right.IsInProgress();
    }


    public bool IsLeftPressed()
    {
        return inputActions.Player.Left.IsInProgress();
    }
    
    public bool IsJumpPressed()
    {
        return inputActions.Player.Jump.WasPressedThisFrame();
    }



}
