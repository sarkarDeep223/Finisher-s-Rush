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





    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    // [SerializeField] private SpriteRenderer spriteRenderer;





    public event EventHandler<OnMovementEventArgs> OnPlayerMoving;






    // [SerializeField] Animator animator;
    private bool isGrounded;


    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        IsGrounded();
        HandelJump();
        UpdateMovementState();
        // Jump();
    }




    private void FixedUpdate()
    {
        float moveDirection;
        Vector2 move;
        if (GameInput.Instance.IsRightPressed())
        {
            moveDirection = 1f;
            move = new Vector2(moveDirection * speed, rigidbody2d.linearVelocity.y);
            MoveVertical(move);
        }
        else if (GameInput.Instance.IsLeftPressed())
        {
            moveDirection = -1f;
            move = new Vector2(moveDirection * speed, rigidbody2d.linearVelocity.y);
            MoveVertical(move);
        }
    }



    private void UpdateMovementState()
    {
        MovementState state;

        if (!isGrounded)
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
        if (isGrounded && GameInput.Instance.IsJumpPressed())
        {
            rigidbody2d.linearVelocity = new Vector2(rigidbody2d.linearVelocity.x, jumpForce);
        }
    }

    private void IsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, playerMask);
    }







}
