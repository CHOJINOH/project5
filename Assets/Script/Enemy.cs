using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject prfHpBar;
    public GameObject canvas;
    RectTransform hpBar;
    public string enemyName;
    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public int atkSpeed;
    public float moveSpeed = 3f;
    Image nowHpbar;
    public float height = 1.7f;
    public float detectionRange = 5f;

    Transform player;
    bool isChasing = false;

    void Start()
    {
        // ü�� �� �ʱ�ȭ (�� ���� ���� ���������� ����)
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();

        // �� ���� ����
        if (name.Equals("Enemy1"))
        {
            SetEnemyStatus("Enemy1", 100, 10, 1); // Enemy1�� ���� ����
        }
        else if (name.Equals("Enemy2"))
        {
            SetEnemyStatus("Enemy2", 80, 12, 2); // Enemy2�� ���� ���� (����)
        }
        else
        {
            // ���⿡ �ٸ� �� ���� �ʱ�ȭ �ڵ� �߰�
            SetEnemyStatus("Enemy3", 150, 8, 1); // �߰����� �� �ʱ�ȭ
        }

        // ü�� ���� �̹��� �ʱ�ȭ
        nowHpbar = hpBar.transform.GetChild(0).GetComponent<Image>();

        // �ʱ�ȭ�� �� Ȯ�� (������)
        Debug.Log($"Enemy Name: {enemyName}, Max HP: {maxHp}, Current HP: {nowHp}");

        // �÷��̾� ã��
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player object not found!");
        }
    }

    void Update()
    {
        // ü�� �� ��ġ ����
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        hpBar.position = _hpBarPos;

        // ü�� �� ���� ����
        if (nowHpbar != null) // nowHpbar�� null�� �ƴ� ��쿡�� ����
        {
            nowHpbar.fillAmount = (float)nowHp / (float)maxHp;
        }

        // �÷��̾� Ž�� �� ����
        DetectAndChasePlayer();
    }

    public void TakeDamage(int damage)
    {
        nowHp -= damage;
        Debug.Log("Damage taken: " + damage + ", Remaining HP: " + nowHp);

        if (nowHp <= 0)
        {
            Destroy(gameObject);
            Destroy(hpBar.gameObject);
        }
    }

    private void SetEnemyStatus(string _enemyName, int _maxHp, int _atkDmg, int _atkSpeed)
    {
        enemyName = _enemyName;
        maxHp = _maxHp;
        nowHp = _maxHp;
        atkDmg = _atkDmg;
        atkSpeed = _atkSpeed;
    }

    private void DetectAndChasePlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // �÷��̾ Ž�� ���� �ȿ� ���� ��쿡�� ����
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        // �÷��̾ ����
        if (isChasing)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            LookAtPlayer();
        }
    }

    private void LookAtPlayer()
    {
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
