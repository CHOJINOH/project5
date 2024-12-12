using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Mark : MonoBehaviour
{
    public Transform enemy; // ���ʹ̸� �����ϱ� ���� ����
    public float followSpeed = 3f; // ��Ŀ�� ���ʹ̸� �����ϴ� �ӵ�
    private float followTime = 1f; // ��Ŀ�� ������ �ð� (1��)

    private void Start()
    {
        // ���ʹ̸� �����ϱ� �����ϰ�, 1�� �Ŀ� ��Ŀ ����
        StartCoroutine(DestroyMarkAfterDelay());
    }

    private void Update()
    {
        if (enemy != null)
        {
            // ���ʹ̸� �����ϴ� ���� (Y���� ����)
            Vector3 targetPosition = new Vector3(enemy.position.x, transform.position.y, enemy.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    private IEnumerator DestroyMarkAfterDelay()
    {
        yield return new WaitForSeconds(followTime); // 1�� ��� ��
        Destroy(gameObject); // ��Ŀ ����
    }
}