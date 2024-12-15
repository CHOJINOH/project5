using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    [Header("Golem Stats")]
    public float attackAreaRadius = 3f;  // �� �պκ��� ���� ���� (������)
    public float downAttackRange = 1.5f;

    protected override void Update()
    {
        // �ǰ� ���°� �ƴϰ�, ���������, �÷��̾ �����Ѵٸ� �߰�
        if (!isInDamageState && nowHp > 0 && player != null&& !anim.GetBool("isAttack"))
            DetectAndChasePlayer();


        // ü�� �� ��ġ,���� ����
        if (hpBar != null)
        {
            Vector3 _hpBarPos = Camera.main.WorldToScreenPoint
                (new Vector3(transform.position.x, transform.position.y + height, 0));
            hpBar.position = _hpBarPos;
            nowHpbar.fillAmount = nowHp / maxHp;
        }

        // ���� �ִϸ��̼� ���°� �ƴϰ�, �÷��̾���� �Ÿ��� ���� ���� ���̸� ����
        if (player != null && !isEnemyDead && !isInDamageState && !anim.GetBool("isAttack"))
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackAreaRadius)
            {
                Attack();  // ���� ȣ��
            }
        }
    }

    protected virtual void Attack() // ���� ���ֵθ��� ����
    {
        Debug.Log("����ȣ��");

        // ���� �ִϸ��̼� ����
        anim.SetBool("isAttack", true);

        // ���� �ִϸ��̼��� ���� ������ ���
        StartCoroutine(WaitForAttackAnimation(1.4f));  // �ִϸ��̼� ����(2��)�� ���缭 ����
    }

    private IEnumerator WaitForAttackAnimation(float animationLength)
    {
        yield return new WaitForSeconds(animationLength);  // �ִϸ��̼��� ���� ������ ���

        // ���� ���� �� �÷��̾�� ���ظ� �ֱ�
        Vector2 attackPosition = new Vector2(transform.position.x, transform.position.y - downAttackRange); 
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPosition, attackAreaRadius);

        foreach (var collider in hitColliders)
        {
            if (collider != null && collider.CompareTag("Player"))
            {
                HeroKnightUsing playerScript = collider.GetComponent<HeroKnightUsing>();
                if (playerScript != null)
                {
                    playerScript.TakeDamage(atkDmg);  // �÷��̾�� ���ݷ¸�ŭ ���� ������
                }
            }
        }


        // �ִϸ��̼� ������ ���� ���� ����
        anim.SetBool("isAttack", false);
        yield return new WaitForSeconds(1.7f);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.blue;
        // ���� ���� �ð�ȭ (Y������ 2��ŭ ���� ��ġ�� ���� �׸�)
        Vector2 gizmoPosition = new Vector2(transform.position.x, transform.position.y - downAttackRange);
        Gizmos.DrawWireSphere(gizmoPosition, attackAreaRadius); 
    }
}
