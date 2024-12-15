using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BFGolem : Golem
{
    public Text attackMessageText;                       // UI �ؽ�Ʈ�� ������ ����

    public delegate void BossDeathHandler();
    public event BossDeathHandler OnBossDeath;           // ������ �׾��� �� ȣ��� �̺�Ʈ

    private bool canUseSpecialAttack = false;             // �������� ��� ���� ����
    public float damageOutsideRange = 10f;                // ���� ���� �ۿ� ������ �� ���� �����
    public float skillRange = 10f;                        // �� �ֺ��� ū �� ���� (������)
    public GameObject attackEffectPrefab;                 // ����Ʈ Prefab
    public Vector3 effectScale = new Vector3(1f, 1f, 1f); // ����Ʈ�� ũ�� ����

    private Coroutine damageCoroutine;                    // ������� �ִ� �ڷ�ƾ�� ������ ����
    private GameObject currentAttackEffect;               // ���� Ȱ��ȭ�� ���� ����Ʈ
    private Coroutine effectCoroutine;                     // ����Ʈ �ݺ� ȣ���� ���� �ڷ�ƾ

    protected override void Start()
    {
        base.Start(); // �θ� Ŭ������ Start ȣ��
        Debug.Log("BF���� ���� �����̱� �����մϴ� ");
    }

    protected override void Update()
    {
        base.Update();  // �θ� Ŭ������ Update ȣ��

        // ���� �ۿ� ������ ����� ������
        if (canUseSpecialAttack && !isEnemyDead)
            ApplyDamageOutsideRange();
    }

    protected override void DetectAndChasePlayer()
    {
        if (player == null) return;

        HeroKnightUsing playerScript = player.GetComponent<HeroKnightUsing>();
        if (playerScript != null && playerScript.isDead) // �÷��̾� ����� �߰�����
        {
            isChasing = false;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            if (!isChasing) // �������Ͻ���
            {
                SpawnMark();
                canUseSpecialAttack = true;
                Debug.Log("���� ���� ����");
                ShowAttackMessage("���̼�Ʈ ���� ������ �����մϴ�!!");
            }
            isChasing = true;

            anim.SetBool("isWalk", true);
            Vector3 direction = (player.position - transform.position).normalized;  // �÷��̾ �߰�
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            LookAtPlayer();
        }
        else
        {
            isChasing = false;
        }
    }

    // �� ���� �ۿ� ������ ������� ������ �Լ�
    private void ApplyDamageOutsideRange()
    {
        ShowAttackEffect();  // ��ų ����Ʈ ǥ��
        // ���� �ۿ� �ִ� "Player" �±׸� ���� �÷��̾� ã��
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, skillRange);

        // ���� �ۿ� �ִ� �÷��̾ ����� ����
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Player") && !IsWithinSkillRange(collider))
            {
                HeroKnightUsing playerScript = collider.GetComponent<HeroKnightUsing>();
                if (playerScript != null)
                {
                    // ���� �ۿ� ������ 1�ʸ��� ������� ����
                    if (damageCoroutine == null)
                    {
                        damageCoroutine = StartCoroutine(DealDamageOverTime(playerScript));
                    }
                }
            }
        }
    }

    // ���� ���� �ִ��� üũ
    private bool IsWithinSkillRange(Collider2D collider)
    {
        return Vector2.Distance(transform.position, collider.transform.position) <= skillRange;
    }

    // ���� �ۿ� ���� �� 1�ʸ��� ������� ������ �ڷ�ƾ
    private IEnumerator DealDamageOverTime(HeroKnightUsing playerScript)
    {
        while (playerScript != null && !IsWithinSkillRange(playerScript.GetComponent<Collider2D>()))
        {
            playerScript.TakeDamage(damageOutsideRange); // ����� ������
            Debug.Log("���� �ۿ��� 1�ʸ��� ����� ����!");
            yield return new WaitForSeconds(1f); // 1�ʸ��� �����
        }

        damageCoroutine = null; // �ڷ�ƾ ���� �� null�� ����
    }

    // ��ų ����Ʈ ǥ�� (0.5�ʸ��� �ݺ� ȣ��)
    private void ShowAttackEffect()
    {
        if (attackEffectPrefab != null)
        {
            // 0.5�ʸ��� ����Ʈ�� ���� ����
            if (effectCoroutine == null)
            {
                effectCoroutine = StartCoroutine(SpawnEffect());
            }
        }
    }

    // 0.5�ʸ��� ����Ʈ�� ���� �����ϴ� �ڷ�ƾ
    private IEnumerator SpawnEffect()
    {
        while (canUseSpecialAttack && !isEnemyDead)
        {
            // ���ο� ����Ʈ�� ����
            GameObject newEffect = Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);
            newEffect.transform.localScale = effectScale; // ����Ʈ ũ�� ����

            // ����Ʈ�� ������ �� 0.5�ʸ��� �ݺ�
            yield return new WaitForSeconds(0.3f);
        }

        // ��ų�� ����Ǹ� ����Ʈ�� ��� ����
        EndSpecialAttack();
    }

    // ��ų�� ������ ����Ʈ�� ��Ȱ��ȭ�ϴ� �Լ�
    private void EndSpecialAttack()
    {
        // ��ų ���� �� �ݺ� �ڷ�ƾ�� ���߰�, ������ ����Ʈ�� ��� ����
        if (effectCoroutine != null)
        {
            StopCoroutine(effectCoroutine); // �ݺ� �ڷ�ƾ ����
            effectCoroutine = null;
        }

        // �߰��� ����Ʈ���� �ʹ� �������� �ʵ��� ����
        foreach (Transform child in transform)
        {
            if (child.CompareTag("AttackEffect"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    // ���� �׾��� �� ȣ��Ǵ� �Լ�
    protected override void HandleWhenDead()
    {
        ShowAttackMessage("���̼�Ʈ ���� óġ�Ͽ����ϴ�!!");
        base.HandleWhenDead();  // �⺻ Enemy�� ���� ó��

        OnBossDeath?.Invoke();
        Debug.Log("���� ���� �̺�Ʈ ȣ���");

        DropSpecialLoot();
        EndSpecialAttack();  // ���� ���� �� ��ų ���� ó��
    }

    // ���� ���� Ư�� ������ ��� �Լ�
    private void DropSpecialLoot()
    {
        // ���� ������ ������Ʈ ���� (��: Instantiation�� ���� ������ ���)
        Debug.Log("Special loot is dropped!");
    }

    // ���� ���� �ð�ȭ (������)
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // �� �ֺ��� ū �� ���� �׸��� (������� ������ ����)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, skillRange);  // skillRange ��� (�� �ֺ� ū ��)
    }

    private void ShowAttackMessage(string message)
    {
        // UI �ؽ�Ʈ�� �޽����� ����
        if (attackMessageText != null)
            attackMessageText.text = message;
    }
}
