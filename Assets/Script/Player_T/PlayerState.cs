using UnityEngine;
public enum playerState
{
    Idle,
    Jump,
    Run,
    Walk,
    Fly,
    Fish,
    Swim
}
public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;

    protected Animator animator;

    protected Rigidbody rb;

    protected float magnitude;

    protected float moveSpeed = GameApp.Instance.playerData.moveSpeed;

    protected Transform transform;

    public virtual void Update()
    {
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
            rb.AddForce(Vector3.up * GameApp.Instance.playerData.jumpHight, ForceMode.Impulse);
            stateMachine.TransState(playerState.Jump);
        }
        if (stateMachine.player.IsInWater())
        {
            stateMachine.TransState(playerState.Idle);
            stateMachine.TransState(playerState.Swim);
        }

    }
    public abstract void Enter();
    public abstract void Exit();
}
