using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer SpriteRenderer;
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
    public int nextMove;

    Transform player;
    bool isChasing = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Start()
    {

        SpriteRenderer = GetComponent<SpriteRenderer>();
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
            isChasing = true; // �÷��̾ ���� �ȿ� ������ ���� ����
        }
        else
        {
            isChasing = false; // ���� ���̸� ���� ����
        }

        // �÷��̾ ����
        if (isChasing)
        {
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 3, LayerMask.GetMask("Platform"));

            if (rayHit.collider == null && !isChangingDirection) // �� ���� ���� ��� (�ݺ����� �ʵ���)
            {
                StartCoroutine(MoveAwayAndRetry()); // �� ������ ó���ϴ� �ڷ�ƾ ȣ��
                isChangingDirection = true; // �ݴ� �������� �̵� ������ ǥ��
            }
            else
            {
                // ���� ����
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                LookAtPlayer();
            }
        }
    }

    private bool isChangingDirection = false; // �� ���� ���� �� �̵� ���� ����

    private IEnumerator MoveAwayAndRetry()
    {
        Debug.Log("Hit wall, changing direction.");

        // ���� ��ȯ
        nextMove *= -1;
        SpriteRenderer.flipX = nextMove == 1;

        // �ݴ� �������� �̵��� �Ÿ�
        float moveDistance = 1f; // �ݴ� �������� �̵��� �Ÿ�
        float moveTime = 0.5f; // �ݴ� �������� �̵��� �ð�
        float elapsedTime = 0f;

        // �ݴ� �������� �̵��ϱ� (�̵� �Ÿ��� �������� �̵�)
        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            float moveAmount = Mathf.Lerp(0, moveDistance, elapsedTime / moveTime); // �ð��� ���� �̵� �Ÿ� ���
            transform.Translate(Vector2.right * nextMove * moveAmount * Time.deltaTime); // �̵�
            yield return null;
        }

        // �̵� �� ������ �簳
        isChangingDirection = false; // �̵��� �������Ƿ� ������ �簳
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
