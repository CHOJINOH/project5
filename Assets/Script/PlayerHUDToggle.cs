using UnityEngine;

public class PlayerHUDToggle : MonoBehaviour
{
    // UI�� ��Ÿ���� ���� ��� ������ (Player HUD)
    public GameObject playerHUD;

    // Update�� �� �����Ӹ��� ȣ���
    void Update()
    {
        // "i" ��ư�� ���ȴ��� Ȯ��
        if (Input.GetKeyDown(KeyCode.I))
        {
            // ���� playerHUD�� Ȱ��ȭ ���¸� ���
            playerHUD.SetActive(!playerHUD.activeSelf);
        }
    }
}