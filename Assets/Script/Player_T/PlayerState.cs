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
    private Vector3 targetDirection;
private float smoothTime = 0.1f;
    protected Animator animator;

    protected Rigidbody rb;

    protected float magnitude;

    protected float moveSpeed = GameApp.Instance.playerData.moveSpeed;

    protected Transform transform;

    public virtual void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical); // 直接用输入值构造向量
        if (direction.sqrMagnitude > 0.01f) // 避免零向量
        {
            direction.Normalize();
            direction *= moveSpeed;
        }
        else
        {
            direction = Vector3.zero; // 无输入时停止移动
        }
        Vector3 currentDirection = stateMachine.player.transform.forward * vertical +
                          stateMachine.player.transform.right * horizontal;
        targetDirection = Vector3.Lerp(targetDirection, currentDirection, Time.deltaTime * 10f);

        if (targetDirection.sqrMagnitude > 0.01f)
        {
            direction = targetDirection.normalized * moveSpeed;
        }
        else
        {
            direction = Vector3.zero;
        }
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
