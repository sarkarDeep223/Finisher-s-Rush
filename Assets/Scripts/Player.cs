using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;





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
    private Collider2D collider2d;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 14f;


    // [SerializeField] private CameraShake cameraShake;
    // [SerializeField] private SpriteRenderer spriteRenderer;



    // [] private CinemachineImpulseSource impulseManager;


    private bool isColliding = false;



    public event EventHandler<OnMovementEventArgs> OnPlayerMoving;


    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        // impulseManager = GetComponent<CinemachineImpulseSource>();
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
        move = AdjustForEdges(move);
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





    private void OnCollisionStay2D(Collision2D collision)
    {
        isColliding = true;
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
        float rayLength = 0.6f;
        Vector2 boundsMin = collider2d.bounds.min;
        Vector2 boundsMax = collider2d.bounds.max;
        Vector2 center = new Vector2((boundsMin.x + boundsMax.x) / 2, boundsMin.y);
        RaycastHit2D hitCenter = Physics2D.Raycast(center, Vector2.down, rayLength, playerMask);
        Debug.DrawRay(center, Vector2.down * rayLength, Color.green);
        return hitCenter;
    }









    private Vector2 AdjustForEdges(Vector2 move)
    {
        Vector2 boundsMin = collider2d.bounds.min;
        Vector2 boundsMax = collider2d.bounds.max;
        Vector2 right = new Vector2(boundsMax.x - 0.4f, boundsMin.y);
        Vector2 left = new Vector2(boundsMin.x + 0.4f, boundsMin.y);
        RaycastHit2D hitLeft = Physics2D.Raycast(left, Vector2.down, 0.6f, playerMask);
        RaycastHit2D hitRight = Physics2D.Raycast(right, Vector2.down, 0.6f, playerMask);
        Debug.DrawRay(left, Vector2.down * 0.6f, Color.red);
        Debug.DrawRay(right, Vector2.down * 0.6f, Color.blue);
        float horizontalVelocity = move.x;
        if (isColliding)
        {
            if (horizontalVelocity > 0.01f && !hitRight)
            {
                move.x = 2 * speed; // boost forward
            }
            else if (horizontalVelocity < -0.01f && !hitLeft)
            {
                Debug.Log("Left edge - moving backward");
                move.x = -2 * speed; // boost backward
            }
        }
        return move;
    }




    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     // Debug.Log("Trigger Entered with " + collision.name);
    //     if (collision.GetComponent<Obsticel>())
    //     {
    //         GameOverSequence();
    //     }
    // }


    // private void GameOverSequence()
    // {

        // CameraShake.instance.cameraEffect(impulseManager);

        // 1️⃣ Camera shake for impact
        // if (cameraShake != null)
        // StartCoroutine(cameraShake.Shake(3f, 0.4f)); // (duration, magnitude)

        // 2️⃣ Slow down or freeze time briefly
        // Time.timeScale = 0f; 
        // yield return new WaitForSecondsRealtime(0.3f); // Wait in real time (not affected by timeScale)

        // 3️⃣ Reset time and reload the level
        // Time.timeScale = 1f;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // }






}
