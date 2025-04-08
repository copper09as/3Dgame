using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;

    protected Animator animator;

    protected Rigidbody rb;

    protected float magnitude;

    protected float moveSpeed = 5f;

    protected Transform transform;
    public virtual void Update()
    {

        if(rb==null)
        {
            rb = stateMachine.player.rb;
        }
        float horizontal = Input.GetAxis("Horizontal");

        float vertical = Input.GetAxis("Vertical");


        Vector3 direction = stateMachine.player.transform.forward * vertical;
        direction += stateMachine.player.transform.right * horizontal;
        direction.Normalize();
        magnitude = direction.magnitude;
        direction *= moveSpeed;
        direction.y = 0f;
        stateMachine.player.Move(direction);


        if (Input.GetKeyDown(KeyCode.Space) && stateMachine.player.IsGrounded())
        {
            rb.AddForce(Vector3.up * 6f, ForceMode.Impulse);
            stateMachine.TransState(new PlayerJump(stateMachine,animator));
        }

    }
    public abstract void Enter();
    public abstract void Exit();
}
