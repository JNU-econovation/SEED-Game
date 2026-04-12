using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runMultiplier = 2f;
    [SerializeField] private float jumpForce = 5f;

    [Header("구르기 설정")]
    [SerializeField] private float rollDistance = 5f;
    [SerializeField] private float rollDuration = 0.8f;
    [SerializeField] private float rollSpeed = 8f;

    private Rigidbody rb;
    private Animator animator;
    public Transform cameraTransform;

    // FixedUpdate에서 적용할 수평 속도
    private Vector3 moveDirection;

    // 점프: Update에서 입력 감지 → FixedUpdate에서 velocity 적용
    private bool pendingJump = false;
    private bool isJumping = false;
    private float jumpCooldown = 1.0f;
    private float jumpTimer = 0f;

    private PlayerAttack_MK attackScript;
    public bool isRolling = false;
    private float rollTimer = 0f;
    private Vector3 rollDirection;

    private CapsuleCollider capsule;
    private float originalHeight;
    private Vector3 originalCenter;
    [SerializeField] private float rollHeight = 0.5f;
    [SerializeField] private Vector3 rollCenter = new Vector3(0f, 0.25f, 0f);

    public bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        attackScript = GetComponent<PlayerAttack_MK>();
        capsule = GetComponent<CapsuleCollider>();
        originalHeight = capsule.height;
        originalCenter = capsule.center;
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            AudioManager.Instance.StopStep();
            return;
        }

        if (isDead)
        {
            AudioManager.Instance.StopStep();
            animator.SetFloat("Speed", 0f);
            return;
        }

        if (attackScript != null && attackScript.IsAttackingOrBusy())
        {
            moveDirection = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            AudioManager.Instance.StopStep();
            return;
        }

        if (isRolling)
        {
            moveDirection = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            AudioManager.Instance.StopStep();
            return;
        }

        if (isJumping)
        {
            AudioManager.Instance.StopStep();
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0f) isJumping = false;
        }

        // ── 이동 입력 처리 ──────────────────────────────────────────────────
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 카메라 기준 평면 방향 계산 (매 프레임 캐싱)
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight   = cameraTransform.right;
        camForward.y = 0f;
        camRight.y   = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * v + camRight * h).normalized;

        // 이동 방향으로 회전 (sqrMagnitude: sqrt 연산 생략)
        if (moveDir.sqrMagnitude >= 0.01f)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // 애니메이터 블렌드트리용 로컬 입력값
        float inputX = Vector3.Dot(moveDir, transform.right);
        float inputZ = Vector3.Dot(moveDir, transform.forward);
        animator.SetFloat("MoveX", inputX, 0.01f, Time.deltaTime);
        animator.SetFloat("MoveZ", inputZ, 0.01f, Time.deltaTime);

        // 달리기 판정 및 사운드
        bool isRunning = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? walkSpeed * runMultiplier : walkSpeed;

        if (isRunning)
            AudioManager.Instance.PlayPlayerRunLoop();
        else if (moveDir.sqrMagnitude > 0.01f)
            AudioManager.Instance.PlayPlayerWalkLoop();
        else
            AudioManager.Instance.StopStep();

        animator.SetBool("IsRunning", isRunning);

        // FixedUpdate로 전달할 이동 벡터 설정
        moveDirection = moveDir * currentSpeed;
        animator.SetFloat("Speed", moveDirection.magnitude);

        // ── 점프 입력 (FixedUpdate에서 velocity 적용) ──────────────────────
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !attackScript.isAttacking)
        {
            pendingJump = true;
            isJumping   = true;
            jumpTimer   = jumpCooldown;
            animator.SetTrigger("Jump");
            AudioManager.Instance.PlayPlayerJump();
            AudioManager.Instance.StopStep();
        }

        // ── 구르기 입력 ──────────────────────────────────────────────────────
        if (!isRolling && Input.GetKeyDown(KeyCode.LeftControl) && !isJumping && !attackScript.isAttacking)
        {
            // 구르기 방향: 입력이 없으면 정면
            Vector3 rollDir = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) rollDir += transform.forward;
            if (Input.GetKey(KeyCode.S)) rollDir -= transform.forward;
            if (Input.GetKey(KeyCode.A)) rollDir -= transform.right;
            if (Input.GetKey(KeyCode.D)) rollDir += transform.right;
            if (rollDir == Vector3.zero) rollDir = transform.forward;

            StartRoll(rollDir.normalized);
            AudioManager.Instance.PlayPlayerRoll();
            AudioManager.Instance.StopStep();
        }
    }

    void StartRoll(Vector3 inputDir)
    {
        rollDirection = inputDir;
        isRolling = true;
        rollTimer = rollDuration;

        capsule.height = rollHeight;
        capsule.center = rollCenter;

        float leftDot  = Vector3.Dot(inputDir, -transform.right);
        float rightDot = Vector3.Dot(inputDir,  transform.right);
        float backDot  = Vector3.Dot(inputDir, -transform.forward);

        if (leftDot > 0.7f)       animator.SetTrigger("RollLeft");
        else if (rightDot > 0.7f) animator.SetTrigger("RollRight");
        else if (backDot > 0.7f)  animator.SetTrigger("RollBackward");
        else                       animator.SetTrigger("RollForward");
    }

    void FixedUpdate()
    {
        // 점프: AddForce 대신 velocity 직접 설정 → 즉각적인 스냅 점프감
        if (pendingJump)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            pendingJump = false;
        }

        if (attackScript != null && attackScript.IsAttackingOrBusy())
        {
            // 공격 중 수평 이동 즉시 정지
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
        else if (isRolling)
        {
            rollTimer -= Time.fixedDeltaTime;
            if (rollTimer <= 0f)
            {
                isRolling = false;
                capsule.height = originalHeight;
                capsule.center = originalCenter;
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
                return;
            }
            float speed = rollDistance / rollDuration;
            rb.linearVelocity = new Vector3(rollDirection.x * speed, rb.linearVelocity.y, rollDirection.z * speed);
        }
        else
        {
            // velocity 직접 제어: 키를 떼는 순간 즉시 감속
            rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
        }
    }
}
