using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Boss : Enemy
{
    public float specialAttackCooldown = 5f; // Ư�� ���� ��ٿ�
    private bool canUseSpecialAttack = true;

    // Start �޼��带 override�� ����
    protected override void Start()
    {
        base.Start(); // �θ� Ŭ������ Start ȣ��

        // RedBoss ������ ���� ����
        SetEnemyStatus("���庸�� ŷ", 500, 20); // ���� �ʱ�ȭ
        Debug.Log("RedBoss Initialized");
    }

    void Update()
    {
        // �⺻ Enemy�� Update ��� ����
        base.Update();

        // ���� Ư�� �ൿ �߰�
        if (canUseSpecialAttack && !isEnemyDead)
        {
            StartCoroutine(UseSpecialAttack());
        }
    }

    private IEnumerator UseSpecialAttack()
    {
        canUseSpecialAttack = false;

        // Ư�� ���� ���� (��: ���� ����)
        Debug.Log("RedBoss uses a special attack!");
        PerformAreaAttack();

        // ��ٿ� ���
        yield return new WaitForSeconds(specialAttackCooldown);
        canUseSpecialAttack = true;
    }

    private void PerformAreaAttack()
    {
        // ���� ���� �ִ� ��� �÷��̾� Ž��
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, detectionRange, LayerMask.GetMask("Player"));

        foreach (Collider2D player in hitPlayers)
        {
            HeroKnightUsing playerScript = player.GetComponent<HeroKnightUsing>();
            if (playerScript != null && !playerScript.isDead)
            {
                playerScript.TakeDamage(atkDmg * 2); // ������ Ư�� ������ 2�� ������
                Debug.Log($"RedBoss dealt {atkDmg * 2} damage with a special attack!");
            }
        }
    }

    // Gizmo: Ư�� ���� ������ �ð������� ǥ��
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();  // �⺻ Enemy�� �ð��� ���� ǥ��
        Gizmos.color = Color.yellow;  // Ư�� ���� ���� ����
        Gizmos.DrawWireSphere(transform.position, detectionRange);  // Ư�� ���� ����
    }

    // ���� �׾��� �� ȣ��Ǵ� �Լ�
    protected override void HandleWhenDead()
    {
        base.HandleWhenDead();  // �⺻ Enemy�� ���� ó��

        // RedBoss ������ ���� ȿ��
        Debug.Log("RedBoss defeated! Special loot dropped.");
        DropSpecialLoot();
    }

    // ���� ���� Ư�� ������ ��� �Լ�
    private void DropSpecialLoot()
    {
        // ���� ������ ������Ʈ ���� (��: Instantiation�� ���� ������ ���)
        Debug.Log("Special loot is dropped!");
        // ����: Instantiate(lootPrefab, transform.position, Quaternion.identity);
    }
}
