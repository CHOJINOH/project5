using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] float spawnLoopTime;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnOffset = 2f;  // ���� ��ġ�� ������ (�翷���� �󸶳� ��������)
    private bool isSpawning = false;  // ���� ���θ� üũ�ϴ� ����

    // ������ �����ϴ� �޼���
    public void StartSpawning()
    {
        if (!isSpawning)  // �̹� ������ ���۵� ��� �ٽ� �������� �ʵ���
        {
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }
    }

  
    private IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            // ���ʰ� ���������� �� ����
            Vector3 leftSpawnPosition = transform.position + Vector3.left * spawnOffset;
            Vector3 rightSpawnPosition = transform.position + Vector3.right * spawnOffset;

            // ���� ���ʰ� �����ʿ� ���� ����
            Instantiate(enemyPrefab, leftSpawnPosition, Quaternion.identity);
            Instantiate(enemyPrefab, rightSpawnPosition, Quaternion.identity);

            // ���� �������� ���
            yield return new WaitForSeconds(spawnLoopTime);
        }
    }
}
