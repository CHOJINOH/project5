using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    public Button speedButton;      // �̵� �ӵ� ��ư
    public Button attackButton;     // ���ݷ� ��ư
    public Button healthButton;     // �ִ� ü�� ��ư
    public Button randomButton;     // ������ ��ư

    public HeroKnightUsing heroKnight;  // HeroKnightUsing ��ũ��Ʈ ����

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
}
