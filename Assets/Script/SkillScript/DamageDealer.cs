using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class DamageDealer : MonoBehaviour
{
    private float damage;
    private float knockback;
    private float attackDuration;
    private int attackCount;
    private Transform target;  // ������ ��� (�÷��̾�)
    private Vector2 boxSize;   // ���� �ڽ� ũ��
    private int currentAttack = 0;
    private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();

    public void Initialize(float damage, float knockback, float attackDuration, int attackCount, Transform target, Vector2 boxSize)
    {
        this.damage = damage;
        this.knockback = knockback;
        this.attackDuration = attackDuration;
        this.attackCount = attackCount;
        this.target = target;
        this.boxSize = boxSize;

        // ��� ù ��° ������ ����
        DealDamage();

        // �ڷ�ƾ ����
        StartCoroutine(DealDamageCoroutine());
    }

    private void Update()
    {
        if (target != null)
        {
            // ����� ��ġ�� ���� �̵�
            transform.position = Vector2.Lerp(transform.position, target.position, Time.deltaTime * 5f);
        }
    }

    private IEnumerator DealDamageCoroutine()
    {
        while (currentAttack < attackCount)
        {

            // ���� ���� ���� ���
            currentAttack++;
            yield return new WaitForSeconds(attackDuration);
        }

        Destroy(gameObject); // ������ ���� �� ������ ����
    }

    private void DealDamage()
    {
        // ���� �浹 üũ
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") && !hitEnemies.Contains(collider))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    // ������ �� �˹� ó��
                    Vector2 knockbackDirection = (collider.transform.position - transform.position).normalized;
                    ParameterPlayerAttack attackParams = new ParameterPlayerAttack
                    {
                        damage = damage,
                        knockback = knockback,
                        knockbackDirection = knockbackDirection
                    };

                    enemy.TakeDamage(attackParams); // ������ ������ �� ���� ����
                    hitEnemies.Add(collider);
                }
            }
        }

        // Ÿ�� ��� �ʱ�ȭ
        hitEnemies.Clear();
    }

    private void OnDrawGizmos()
    {
        // ���� ������ �ð�ȭ
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}