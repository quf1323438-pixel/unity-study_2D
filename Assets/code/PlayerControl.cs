using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    private PlayerControls controls;

    public Vector2 MoveInput {get; private set;}
    
    public bool JumpHeld {get; private set;}
    private bool jumpPressed; //애는왜 private?

    void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable() //호출시 활성화
    {
        controls.Player.Enable(); //입력갑지 시작
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;

        controls.Player.Jump.started += OnJumpStarted;
        controls.Player.Jump.performed += OnJumpPerformed;
        controls.Player.Jump.canceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        controls.Player.Disable(); //입력갑지 종료
        controls.Player.Move.performed -= OnMovePerformed; //구독 해제
        controls.Player.Move.canceled -= OnMoveCanceled;

        controls.Player.Jump.started -= OnJumpStarted;
        controls.Player.Jump.performed -= OnJumpPerformed;
        controls.Player.Jump.canceled -= OnJumpCanceled;   
    }


    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        MoveInput = Vector2.zero;
    }

    private void OnJumpStarted(InputAction.CallbackContext ctx)
    {
        jumpPressed = true;
    }
 
    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        JumpHeld = true;
    }
 
    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        JumpHeld = false;
    }

    public bool ConsumeJumpPressed()
    {
        bool value = jumpPressed;
        jumpPressed = false;
        return value;
    }
}