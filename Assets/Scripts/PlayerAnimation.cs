using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{


    private Player player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }







    private void Start()
    {
        player.OnPlayerMoving += Player_OnPlayerMoving;
    }



    private void Player_OnPlayerMoving(object sender, OnMovementEventArgs e)
    {

        // Debug.Log(e.moveVelocity.linearVelocity.x);



        SpriteFlip(e.moveVelocity.linearVelocity.x);
        switch (e.state)
        {
            case MovementState.idle:
                SetIdle();
                break;
            case MovementState.running:
                SetRunning(e.moveVelocity.linearVelocity.x);
                break;
            case MovementState.jumping:
                SetJumping();
                break;
        }

    }



    private void SpriteFlip(float moveDirection)
    {
        float threshold = 0.01f; // ignore tiny values

        if (moveDirection > threshold)
        {
            spriteRenderer.flipX = true;  // facing right
        }
        else if (moveDirection < -threshold)
        {
            spriteRenderer.flipX = false; // facing left
        }
        // if velocity is near 0, don't flip → keeps last direction
    }

    private void SetIdle()
    {
        animator.SetFloat("Runing", 0f);
        animator.SetBool("Jump", false);
    }



    private void SetRunning(float xVelocity)
    {
        animator.SetFloat("Runing", Mathf.Abs(xVelocity));
        animator.SetBool("Jump", false);
    }

    private void SetJumping()
    {
        animator.SetFloat("Runing", 0f);
        animator.SetBool("Jump", true);
    }






    




}
