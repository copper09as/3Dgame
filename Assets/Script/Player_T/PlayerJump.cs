using UnityEngine;

public class PlayerJump : PlayerState
{
    private float jumpCooldown = 0.1f;
    private float cooldownTimer;
    public PlayerJump(PlayerStateMachine stateMachine, Animator animator, Rigidbody rb)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
        this.rb = rb;
        transform = stateMachine.player.transform;
    }
    public override void Enter()
    {
        animator.SetBool("IsJumping", true);
        cooldownTimer = jumpCooldown; // 初始化冷却计时器
    }

    public override void Exit()
    {
        animator.SetBool("IsJumping", false);
    }

    public override void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space) && GameApp.Instance.inventoryManager.ContainsItem(3))
        {
            stateMachine.TransState(playerState.Fly);

        }
        if (stateMachine.player.IsGrounded() &&
            rb.velocity.y < 0.1f)
        {
            stateMachine.ReturnState();
        }
    }
}