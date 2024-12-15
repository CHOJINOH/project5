using UnityEngine;
using System.Collections;

public class CircularSlash : MonoBehaviour
{
    public GameObject skillPrefab; // ��ų ������
    public float spawnInterval = 0.4f; // ��ȯ ����
    public float skillDamage = 10f; // ��ų ������
    public Vector3 effectSize = new Vector3(1, 1, 1); // ����Ʈ ũ�� (ũ�⸦ ������ ����)

    private Transform pos; // ��ų ��ȯ ��ġ
    public float skillRadius = 3f; // ���� ���� �ݰ� (������ ���õ� ��)

    private bool isSkillActive = false; // ��ų Ȱ��ȭ ����
    private float damageTimer = 0f; // ����� ó�� �ð� ����
    private Coroutine skillCoroutine; // �ڷ�ƾ�� ������ ����

    void Start()
    {
        // pos�� �ڵ����� �Ҵ� (�÷��̾��� ��ġ)
        pos = transform; // ��ų�� �ߵ��ϴ� ��ü�� �� ��ũ��Ʈ�� �پ� �ִ� ���� ������Ʈ��� ����
    }

    void Update()
    {
        // G Ű�� ������ �� ��ų �ߵ�
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartSkill();
        }

        // G Ű�� ������ �� ��ų ����
        if (Input.GetKeyUp(KeyCode.G))
        {
            StopSkill();
        }

        // ��ų�� Ȱ��ȭ�� ���¿��� ��� ����� ������
        if (isSkillActive)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= 0.2f)  // 0.2�ʸ��� ����� ó��
            {
                damageTimer = 0f;  // Ÿ�̸� �ʱ�ȭ
                ApplyDamage();
            }
        }
    }

    private void StartSkill()
    {
        // ��ų�� Ȱ��ȭ�ϰ� ����Ʈ�� ��ȯ
        if (!isSkillActive)
        {
            isSkillActive = true;  // ��ų Ȱ��ȭ
            skillCoroutine = StartCoroutine(SpawnSkillPrefab());  // ����Ʈ ���� ����
        }
    }

    private void StopSkill()
    {
        isSkillActive = false;  // ��ų ��Ȱ��ȭ
        damageTimer = 0f;  // Ÿ�̸� �ʱ�ȭ

        // ����Ʈ ���� �ڷ�ƾ�� ����
        if (skillCoroutine != null)
        {
            StopCoroutine(skillCoroutine);
            skillCoroutine = null;
        }
    }

    private void ApplyDamage()
    {
        // ������ ����� ó��
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(pos.position, skillRadius);
        foreach (Collider2D collider in hitEnemies)
        {
            if (collider.CompareTag("Enemy")) // "Enemy" �±׸� ���� ��ü�鸸 ó��
            {
                // Enemy ��ũ��Ʈ�� �ִٸ� ����� ������
                Enemy enemyScript = collider.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    // ParameterPlayerAttack ��ü ����
                    ParameterPlayerAttack attackParams = new ParameterPlayerAttack
                    {
                        damage = skillDamage,
                    };

                    // TakeDamage �Լ� ȣ��
                    enemyScript.TakeDamage(attackParams);
                }
            }
        }
    }

    private IEnumerator SpawnSkillPrefab()
    {
        // ��ų ����Ʈ�� Ȱ��ȭ�� ���� ��� ����
        while (isSkillActive)
        {
            // ��ų ������ ��ȯ
            Vector3 spawnPosition = pos.position + new Vector3(0, 0, -1);
            GameObject spawnedSkill = Instantiate(skillPrefab, spawnPosition, Quaternion.identity);

            // ����Ʈ ũ�� ����
            spawnedSkill.transform.localScale = effectSize;

            // ��� ��� (��ȯ ���� ���)
            yield return new WaitForSeconds(0.2f);  // 0.2�ʸ��� ����Ʈ ����
        }
    }

    // Gizmos�� ����� ���� ������ �ð������� Ȯ��
    void OnDrawGizmosSelected()
    {
        // ����� ���� ����
        Gizmos.color = Color.red;

        // ���� ���� �ð�ȭ
        Gizmos.DrawWireSphere(transform.position, skillRadius);
    }
}
