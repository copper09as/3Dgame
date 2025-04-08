using UnityEngine;

public class PlayerJump : PlayerState
{
    private float jumpCooldown = 0.1f; // 新增：跳跃冷却时间
    private float cooldownTimer;
    public PlayerJump(PlayerStateMachine stateMachine, Animator animator)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
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
        // 新增：冷却期间不检测接地
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        // 修改检测逻辑：只有当垂直速度方向向下时才检测接地
        if (stateMachine.player.IsGrounded() &&
            stateMachine.player.rb.velocity.y < 0.1f)
        {
            stateMachine.ReturnState();
        }
    }
}