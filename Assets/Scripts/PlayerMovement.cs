using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runMultiplier = 2f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Ground 체크")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.3f;
    [SerializeField] private LayerMask groundMask;

    [Header("구르기 설정")]
    [SerializeField] private float rollDistance = 5f; 
    [SerializeField] private float rollDuration = 0.8f; 
    [SerializeField] private float rollSpeed = 8f;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private Vector3 moveDirection;
    private bool isJumping = false;
    private float jumpCooldown = 1.0f;
    private float jumpTimer = 0f;
    private PlayerAttack attackScript;
    public bool isRolling = false;
    private float rollTimer = 0f;
    private Vector3 rollDirection;
    private Vector3 rollStart;
    private Vector3 rollEnd;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        attackScript = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        // 바닥 체크
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // 공격 중이면 입력 무시
        if (attackScript != null && attackScript.isAttacking)
        {
            moveDirection = Vector3.zero;
            animator.SetFloat("Speed", 0f); // 애니메이션 멈춤
            return;
        }

        // 구르는 도중 입력 차단
        if (isRolling)
        {
            moveDirection = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }

        // 쿨타임 감소
        if (isJumping)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0f)
            {
                isJumping = false;
            }
        }

        // 입력
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0f, v).normalized;

        // 회전
        if (input.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(input);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // 이동 속도 계산
        float currentSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) currentSpeed *= runMultiplier;

        moveDirection = input * currentSpeed;

        // 애니메이션 파라미터 전달
        animator.SetFloat("Speed", moveDirection.magnitude);

        // 점프 처리 
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping && !attackScript.isAttacking)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
            isJumping = true;
            jumpTimer = jumpCooldown;
        }

        //구르기 처리
        if (!isRolling && Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && !attackScript.isAttacking)
        {
            Vector3 inputDir = new Vector3(
                Input.GetAxisRaw("Horizontal"),
                0f,
                Input.GetAxisRaw("Vertical")
            ).normalized;

            if (inputDir == Vector3.zero)
                inputDir = transform.forward;

            rollDirection = inputDir;
            isRolling = true;
            rollTimer = rollDuration;

            animator.SetTrigger("Roll");
        }

        
        
    }

    void FixedUpdate()
    {
        Vector3 velocity;

        //  공격 중이면 속도 0으로 고정
        if (attackScript != null && attackScript.isAttacking)
        {
            velocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
        else
        {
            velocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
        }
        // 물리 이동 적용
        rb.linearVelocity = velocity;
        

        if (isRolling)
        {
            rollTimer -= Time.fixedDeltaTime;

            if (rollTimer <= 0f)
            {
                isRolling = false;
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
                return;
            }

            // ✔ 초당 이동 속도 계산 (거리 / 지속시간)
            float rollSpeed = rollDistance / rollDuration;
            Vector3 rollVelocity = rollDirection * rollSpeed;

            // ✔ y축 중력 유지
            rb.linearVelocity = new Vector3(rollVelocity.x, rb.linearVelocity.y, rollVelocity.z);
            return;
        }
    }
}
