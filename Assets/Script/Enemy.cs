using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer SpriteRenderer;
    RectTransform hpBar;
    Image nowHpbar;
    Transform player;
    Animator anim;

    public GameObject prfHpBar;
    public GameObject canvas;
    public string enemyName;
    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public int atkSpeed;
    public float moveSpeed = 3f; // �⺻ �̵� �ӵ�
    public float height = 1.7f;
    public float detectionRange = 5f;
    public int nextMove;
    public CameraShake cameraShake;
    public float baseKnockbackForce = 6f;

   
    bool isChasing = false;
    private Vector3 initialPosition;
    private int attackCount = 0;  // ���� Ƚ���� �����ϴ� ����
    private float currentKnockbackForce;     // ���� ����� �˹� ���� (3Ÿ �� ������ ����)


    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

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
        initialPosition = transform.position;
        // ���� ���� �� �⺻ �˹� ������ �ʱ�ȭ
        currentKnockbackForce = baseKnockbackForce;
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
        if (isChangingDirection == false)
            DetectAndChasePlayer();
    }

    private bool isKnockedBack = false;

    public void TakeDamage(int damage)
    {
        if (isKnockedBack) return; // �̹� �з����� ������ �ǰ��� ����

        nowHp -= damage;
        Debug.Log("Damage taken: " + damage + ", Remaining HP: " + nowHp);

        // ���� Ƚ�� ����
        attackCount++;

        // 3��° ���ݿ����� ī�޶� ��鸲 �߻�
        if (attackCount == 3)
        {
            cameraShake.ShakeCamera();
            // 3��° ���� �� �˹� ������ ������(��: 8)���� ����
            currentKnockbackForce = 8f;  // ������ �˹� ���� ����
            attackCount = 0; // ���� Ƚ�� ����
        }

        if (nowHp <= 0)
        {
            anim.SetBool("isHunt", false);
            // ���� �� "isDead" �ִϸ��̼� ����
            anim.SetBool("isDead", true);  // "isDead" �ִϸ��̼� Ʈ����

            // ���� �ð� �� ��ü�� �ı�
            StartCoroutine(HandleDeath());
        }
        else if (!anim.GetBool("isHunt"))// �ǰ� �� �ִϸ��̼� ó�� (isHunt�� false�� ���� ����)
        {
            anim.SetBool("isHunt", true);  // �ǰ� �� "isHunt" �ִϸ��̼� Ʈ����
        }

        
      
        


        else
        {
            // �ǰ� �� �� ���� �з�������
            if (!isKnockedBack)
            {
                Vector2 knockbackDirection = (transform.position - player.position).normalized;

                rigid.velocity = Vector2.zero;  // ���� �ӵ��� ����
                rigid.AddForce(knockbackDirection * currentKnockbackForce, ForceMode2D.Impulse); // ���� ����� �˹� ���� ���

                isKnockedBack = true;
                StartCoroutine(ResetKnockback());
            }
        }
    }

    // �˹� �ʱ�ȭ
    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.1f); // 0.1�� �� �˹� ���� �ʱ�ȭ
        isKnockedBack = false;
        anim.SetBool("isHunt", false);
        // �˹� ������ �⺻������ �ǵ���
        currentKnockbackForce = baseKnockbackForce; // �⺻ �˹� ������ �ʱ�ȭ
    }

    // ���� �� ó�� (1�� ��� �� ��ü �ı�)
    private IEnumerator HandleDeath()
    {
        // 1�� ���� �ִϸ��̼��� ���
        yield return new WaitForSeconds(0.5f);  // 1�� ���

        // ��ü�� �ı�
        Destroy(gameObject);
        Destroy(hpBar.gameObject);  // ü�� �ٵ� �ı�
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
                MoveAwayAndRetry(); // �� ������ ó���ϴ� ���� ȣ��
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

    private void MoveAwayAndRetry()
    {
        Debug.Log("Hit wall, stopping movement for a moment.");

        // �̵� �ӵ��� 0���� �����Ͽ� ����
        moveSpeed = 0f;

        // ��� ���� �� ������ �簳
        StartCoroutine(ResumeMovementAfterDelay()); // ���� �ð� �� �̵��� �簳�ϴ� �ڷ�ƾ ȣ��
    }

    private IEnumerator ResumeMovementAfterDelay()
    {
        yield return new WaitForSeconds(2f); // 1�� ���� ���

        moveSpeed = 3f; // �⺻ �̵� �ӵ� (���ϴ� ������ ����)
        transform.position = Vector3.MoveTowards(transform.position, initialPosition, Time.deltaTime * 5f);
        isChangingDirection = false; // �ٽ� ������ ������ �� �ֵ��� ���� ����
    }

    private void LookAtPlayer()
    {
        if (player.position.x > transform.position.x)
        {
            // �÷��̾ �����ʿ� ������
            transform.localScale = new Vector3(1, 1, 1); // ���������� ����
        }
        else
        {
            // �÷��̾ ���ʿ� ������
            transform.localScale = new Vector3(-1, 1, 1); // �������� ����
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}