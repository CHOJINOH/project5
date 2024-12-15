using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour
{
    public GameObject[] skillPrefabs;  // ���� �ٸ� 3���� ������
    public Transform pos;              // ���� ��ȯ ��ġ
    public float spawnInterval = 0.4f; // ������ ��ȯ ����
    public float skillDamage = 10f;    // ��ų ������
    public float knockback = 5f;       // ��ų �˹� ��
    public Vector2[] boxSizes;         // �� �������� �ڽ� ������
    public Animator animator;          // �ִϸ����� ������Ʈ (��ų �ִϸ��̼��� ����)
    public float moveSpeed = 30f;      // �ִ� �ӵ� (�ʱⰪ 80)
    private float minSpeed = 0f;       // �ּ� �ӵ� (0���� ����)
    public float speedDecay = 0.5f;    // �ӵ� ���� ��
    private bool isCastingSkill;       // ��ų ���� ���� üũ
    private int m_facingDirection = 1; // 1: ������, -1: ����

    void Update()
    {
        // ���� ��ȯ ó�� (�Է� ���� ���� ���� �Ǵ� ������)
        float inputX = Input.GetAxis("Horizontal");
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(SpawnSkillPrefabs());
        }

        // ��ų ���� ���̶�� �÷��̾� �̵�
        if (isCastingSkill)
        {
            MovePlayer(); // �̵� ó�� (Update���� ȣ��)
        }
    }

    private IEnumerator SpawnSkillPrefabs()
    {
        isCastingSkill = true;  // ��ų ���� ����

        string[] attackAnimations = { "Attack1", "Attack2", "Attack3" };

        for (int i = 0; i < skillPrefabs.Length; i++)
        {
            Vector3 spawnPosition = pos.position + new Vector3(0, 0, -1);
            if (m_facingDirection == -1)
            {
                spawnPosition.x -= 2f;
            }
            else
            {
                spawnPosition.x += 2f;
            }

            GameObject spawnedSkill = Instantiate(skillPrefabs[i], spawnPosition, Quaternion.identity);

            DamageDealer damageDealer = spawnedSkill.AddComponent<DamageDealer>();
            Vector2 boxSize = i < boxSizes.Length ? boxSizes[i] : new Vector2(1.5f, 1.5f);
            damageDealer.Initialize(skillDamage, knockback, 0.2f, 3, pos, boxSize);

            SpriteRenderer skillSprite = spawnedSkill.GetComponent<SpriteRenderer>();
            if (skillSprite != null)
            {
                skillSprite.flipX = m_facingDirection == -1;
            }

            // �ִϸ��̼� Ʈ����
            if (animator != null)
            {
                animator.SetTrigger("Attack" + (i + 1));
            }

            // �̵��� �����ϰ� ��ٸ��� ���� (Update���� �̵� ó��)
            yield return new WaitForSeconds(spawnInterval);
        }

        isCastingSkill = false;  // ��ų ���� �Ϸ�
    }

    // �÷��̾ �̵���Ű�� �ڷ�ƾ
    private void MovePlayer()
    {
        // �̵� �ӵ� ���� (�� �����Ӹ��� -0.5�� ����)
        moveSpeed = Mathf.Max(minSpeed, moveSpeed - speedDecay * Time.deltaTime);  // �ּ� �ӵ� ���Ϸ� �������� �ʵ��� ó��

        // �̵��� ��ġ ���
        transform.position += new Vector3(m_facingDirection * moveSpeed * Time.deltaTime, 0, 0);
    }
}