using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SelectUI : MonoBehaviour
{
    public Button speedButton;      // �̵� �ӵ� ��ư
    public Button attackButton;     // ���ݷ� ��ư
    public Button healthButton;     // �ִ� ü�� ��ư
    public Button randomButton;     // ������ ��ư

    public HeroKnightUsing heroKnight;  // HeroKnightUsing ��ũ��Ʈ ����
    public RedSlimeKing boss;  // Boss ��ũ��Ʈ ����

    private List<Button> buttons;  // ��� ��ư ����Ʈ

    void Start()
    {
        // ��ư ����Ʈ �ʱ�ȭ
        buttons = new List<Button> { speedButton, attackButton, healthButton, randomButton };

        // ��ư Ŭ�� �̺�Ʈ�� �޼ҵ� ����
        speedButton.onClick.AddListener(() => OnAnyButtonClicked("speed"));
        attackButton.onClick.AddListener(() => OnAnyButtonClicked("attack"));
        healthButton.onClick.AddListener(() => OnAnyButtonClicked("health"));
        randomButton.onClick.AddListener(() => OnAnyButtonClicked("random"));

        // ������ ������ �����ϴ� �̺�Ʈ ����
        if (boss != null)
        {
            boss.OnBossDeath += HandleBossDeath;
            Debug.Log("���� �̺�Ʈ �����");
        }
    }

    private void OnAnyButtonClicked(string attribute)
    {
        // ĳ���� �Ӽ� ����
        heroKnight.SetCharacterAttribute(attribute);

        // ��� ��ư �����
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }

        // �α� ��� (������)
        Debug.Log($"{attribute} ��ư�� Ŭ���Ǿ� ��� ��ư�� ��Ȱ��ȭ�Ǿ����ϴ�.");
    }

    // ������ �׾��� �� ȣ��Ǵ� �޼ҵ�
    private void HandleBossDeath()
    {
        // ������ �׿��� �� ��ư�� �ٽ� Ȱ��ȭ
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(true);
        }

        // �α� ��� (������)
        Debug.Log("������ �׾����ϴ�. ��ư���� �ٽ� Ȱ��ȭ�Ǿ����ϴ�.");
    }

    // OnDestroy �Ǵ� �ٸ� ���� �������� �̺�Ʈ ���� (���� ����)
    void OnDestroy()
    {
        if (boss != null)
        {
            boss.OnBossDeath -= HandleBossDeath;
        }
    }
}
