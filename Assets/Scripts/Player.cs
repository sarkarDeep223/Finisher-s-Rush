using System;
using UnityEngine;





public enum MovementState
{
    idle,
    running,
    jumping,
    falling
}


public class OnMovementEventArgs : EventArgs
{
    public Rigidbody2D moveVelocity;
    public MovementState state;
}





public class Player : MonoBehaviour
{


    private Rigidbody2D rigidbody2d;

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 14f;
    // [SerializeField] private SpriteRenderer spriteRenderer;





    public event EventHandler<OnMovementEventArgs> OnPlayerMoving;


    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        HandelJump();
        UpdateMovementState();
    }




    private void FixedUpdate()
    {
        float moveDirection = 0f;
        if (GameInput.Instance.IsRightPressed())
        {
            moveDirection = 1f;
        }
        else if (GameInput.Instance.IsLeftPressed())
        {
            moveDirection = -1f;
        }
        Vector2 move = new Vector2(moveDirection * speed, rigidbody2d.linearVelocity.y);
        MoveVertical(move);
    }



    private void UpdateMovementState()
    {
        MovementState state;

        if (!IsGrounded())
        {
            if (rigidbody2d.linearVelocity.y > 0.01f)
            {
                state = MovementState.jumping;
            }
            else
            {
                state = MovementState.falling;
            }
        }
        else
        {
            if (Mathf.Abs(rigidbody2d.linearVelocity.x) > 0.01f)
            {
                state = MovementState.running;
            }
            else
            {
                state = MovementState.idle;
            }
        }
        

        OnPlayerMoving?.Invoke(this, new OnMovementEventArgs()
        {
            moveVelocity = rigidbody2d,
            state = state
        });
    }











    private void MoveVertical(Vector2 move)
    {
        rigidbody2d.linearVelocity = move;
    }


    private void HandelJump()
    {
        if (IsGrounded() && GameInput.Instance.IsJumpPressed())
        {
            rigidbody2d.linearVelocity = new Vector2(rigidbody2d.linearVelocity.x, jumpForce);
        }
    }

    private bool IsGrounded()
    {
        bool hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, playerMask);
        Debug.DrawRay(transform.position, Vector2.down * 0.6f, Color.red);
        return hit;
    }







}
