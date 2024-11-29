using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public GameObject skillPrefab; // Skill1 �������� �Ҵ��� ����
    public float spawnRange = 5.0f; // �������� ��ȯ�� ��ġ�� ����
    public float destroyTime = 1.0f; // �������� ������� �ð� (1��)
    public float spawnInterval = 0.05f; // ������ ��ȯ ���� ���� (0.1��)

    void Update()
    {
        // T Ű�� ������ 10�� �ݺ��ؼ� ���� ��ġ�� Skill1 ����Ʈ�� ��ȯ
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(SpawnSkillEffect());
        }
    }

    // �ڷ�ƾ�� �̿��Ͽ� 10�� �ݺ��ؼ� ���� ��ġ�� Skill1 ����Ʈ�� ��ȯ
    IEnumerator SpawnSkillEffect()
    {
        // Player �±׸� ���� HeroKnight ������Ʈ ã��
        GameObject heroKnight = GameObject.FindGameObjectWithTag("Player");

        if (heroKnight != null && skillPrefab != null)
        {
            for (int i = 0; i < 10; i++)
            {
                // HeroKnight�� ��ġ�� �������� ������ ��ġ ����
                float randomX = Random.Range(-spawnRange, spawnRange);
                float randomY = 0; // Y ���� �������Ѽ� ��鿡 ��ȯ
                float randomZ = Random.Range(-spawnRange, spawnRange);

                // HeroKnight�� ��ġ�� �������� ������ �������� ����
                Vector3 spawnPosition = heroKnight.transform.position + new Vector3(randomX, randomY, randomZ);

                // Skill1 �������� �ش� ��ġ�� ��ȯ
                GameObject skillEffect = Instantiate(skillPrefab, spawnPosition, Quaternion.identity);

                // ���� �ð� �Ŀ� ��ȯ�� �������� ����
                Destroy(skillEffect, destroyTime);

                // ������ �� ������ �α� ���� ���
                yield return new WaitForSeconds(spawnInterval); // ��ȯ �� ���� (0.5��)
            }
        }
        else
        {
            Debug.LogWarning("HeroKnight �Ǵ� Skill1 �������� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}