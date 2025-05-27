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
    public Transform cameraTransform;
    private bool isGrounded;
    private Vector3 moveDirection;
    private bool isJumping = false;
    private float jumpCooldown = 1.0f;
    private float jumpTimer = 0f;
    private PlayerAttack attackScript;
    public bool isRolling = false;
    private float rollTimer = 0f;
    private Vector3 rollDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        attackScript = GetComponent<PlayerAttack>();
    }

    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (attackScript != null && attackScript.isAttacking)
        {
            moveDirection = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }

        if (isRolling)
        {
            moveDirection = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }

        if (isJumping)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0f)
            {
                isJumping = false;
            }
        }

        // 입력 처리
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0f, v).normalized;

        // 카메라 기준 방향 변환
        Transform cam = cameraTransform;
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * input.z + camRight * input.x).normalized;

        // 회전 처리
        if (moveDir.magnitude >= 0.1f)
        {
            float targetYRotation = cameraTransform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, targetYRotation, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // [변경] 방향 벡터를 transform 기준으로 분해해서 애니메이션 파라미터 전달
        float inputX = Vector3.Dot(moveDir, transform.right);     // 좌우
        float inputZ = Vector3.Dot(moveDir, transform.forward);   // 앞뒤

        animator.SetFloat("MoveX", inputX, 0.1f, Time.deltaTime);
        animator.SetFloat("MoveZ", inputZ, 0.1f, Time.deltaTime);

        // [변경] W 키일 때만 달리기 가능
        float currentSpeed = walkSpeed;
        bool isRunning = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift);

        if (isRunning)
        {
            currentSpeed *= runMultiplier;
        }

        animator.SetBool("IsRunning", isRunning); // [변경] 달리기 여부 전달

        moveDirection = moveDir * currentSpeed;

        // 속도 기반 애니메이션
        animator.SetFloat("Speed", moveDirection.magnitude);

        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping && !attackScript.isAttacking)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
            isJumping = true;
            jumpTimer = jumpCooldown;
        }

        // 구르기
        if (!isRolling && Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && !attackScript.isAttacking)
        {
            Vector3 inputDir = moveDir;
            if (inputDir == Vector3.zero)
                inputDir = transform.forward;

            rollDirection = inputDir;
            isRolling = true;
            rollTimer = rollDuration;

            // [변경] 방향에 따라 다른 구르기 애니메이션 트리거
            float leftDot = Vector3.Dot(inputDir, -transform.right);
            float rightDot = Vector3.Dot(inputDir, transform.right);
            float backDot = Vector3.Dot(inputDir, -transform.forward);

            if (leftDot > 0.7f)
            {
                animator.SetTrigger("RollLeft");
            }
            else if (rightDot > 0.7f)
            {
                animator.SetTrigger("RollRight");
            }
            else if (backDot > 0.7f)
            {
                animator.SetTrigger("RollBackward");
            }
            else
            {
                animator.SetTrigger("RollForward");
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 velocity;

        if (attackScript != null && attackScript.isAttacking)
        {
            velocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
        else
        {
            velocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
        }

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

            float rollSpeed = rollDistance / rollDuration;
            Vector3 rollVelocity = rollDirection * rollSpeed;
            rb.linearVelocity = new Vector3(rollVelocity.x, rb.linearVelocity.y, rollVelocity.z);
            return;
        }
    }
}
