using UnityEngine;
using System.Collections;  // �ڷ�ƾ�� ����ϱ� ���� �߰�

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;  // ī�޶� ���� ��ġ
    public float shakeDuration = 0.1f; // ª�� ��鸲 ���� �ð� (���� ����)
    public float shakeAmount = 0.2f;   // ������ ��鸲 ����
    public float shakeReturnSpeed = 5f; // ���� ��ġ�� ���ư��� �ӵ�

    public Transform player;           // �÷��̾��� Transform (�÷��̾ ����)

    private void Start()
    {
        originalPosition = transform.position;  // ī�޶��� ���� ��ġ ����
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    public void ShakeCamera()
    {
        // ������ ������ ī�޶� ��鸲 �߻�
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // ī�޶��� ���� ��ġ���� ������ ��ġ�� �̵����� ������ ��鸲 ȿ�� ����
            float shakeX = Random.Range(-shakeAmount, shakeAmount);
            float shakeY = Random.Range(-shakeAmount, shakeAmount);

            // ī�޶�� �÷��̾ ���󰡸� ��鸲 ����
            transform.position = new Vector3(player.position.x + shakeX, player.position.y + shakeY, transform.position.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ��鸲�� ���� �� ī�޶�� �ٷ� �÷��̾ �����ϵ��� ����
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}