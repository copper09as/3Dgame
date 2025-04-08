using UnityEngine;

public class PlayerJump : PlayerState
{
    private float jumpCooldown = 0.1f; // ��������Ծ��ȴʱ��
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
        cooldownTimer = jumpCooldown; // ��ʼ����ȴ��ʱ��
    }

    public override void Exit()
    {
        animator.SetBool("IsJumping", false);
    }

    public override void Update()
    {
        // ��������ȴ�ڼ䲻���ӵ�
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        // �޸ļ���߼���ֻ�е���ֱ�ٶȷ�������ʱ�ż��ӵ�
        if (stateMachine.player.IsGrounded() &&
            stateMachine.player.rb.velocity.y < 0.1f)
        {
            stateMachine.ReturnState();
        }
    }
}