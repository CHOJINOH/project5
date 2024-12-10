using UnityEngine;

public class DynamicBackground : MonoBehaviour
{
    public GameObject backgroundPrefab; // ��� ������
    public float backgroundHeight; // ��� ����
    public int poolSize = 5; // ��� Ǯ�� ũ��
    public float spawnDistance = 10f; // ��� ���� �Ÿ�

    private Transform player; // �÷��̾� Ʈ������
    private GameObject[] backgroundPool; // ��� Ǯ

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // ��� Ǯ �ʱ�ȭ
        backgroundPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            // ��� �������� �ν��Ͻ�ȭ�Ͽ� Ǯ�� ����
            backgroundPool[i] = Instantiate(backgroundPrefab, new Vector3(0, i * backgroundHeight, 0), Quaternion.identity);
            backgroundPool[i].SetActive(false); // ó������ ��Ȱ��ȭ
        }

        // ù ��° ����� Ȱ��ȭ�ϰ� ��ġ ����
        ActivateBackground(0);
    }

    void Update()
    {
        // �÷��̾ �̵��� ������ ����� üũ�ϰ� ����/����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bg = backgroundPool[i];
            if (!bg.activeSelf) continue; // ��Ȱ��ȭ�� ����� ����

            // ����� ȭ�� ������ ������ ��Ȱ��ȭ
            if (bg.transform.position.y + backgroundHeight < player.position.y - spawnDistance)
            {
                bg.SetActive(false);
            }
        }

        // �÷��̾ ���� �Ÿ� �̵����� �� ����� Ȱ��ȭ
        for (int i = 0; i < poolSize; i++)
        {
            if (!backgroundPool[i].activeSelf)
            {
                ActivateBackground(i);
                break;
            }
        }
    }

    // ����� Ȱ��ȭ�ϰ� ������ ��ġ�� ��ġ
    private void ActivateBackground(int index)
    {
        backgroundPool[index].SetActive(true);
        float yPosition = player.position.y + spawnDistance + index * backgroundHeight;
        backgroundPool[index].transform.position = new Vector3(0, yPosition, 0);
    }
}