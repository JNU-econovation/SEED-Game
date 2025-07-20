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
    private Vector3 moveDirection;
    private bool isJumping = false;
    private float jumpCooldown = 1.0f;
    private float jumpTimer = 0f;
    private PlayerAttack_MK attackScript;
    public bool isRolling = false;
    private float rollTimer = 0f;
    private Vector3 rollDirection;
    private Vector3 cachedInputDir = Vector3.zero;

    private CapsuleCollider capsule;
    private float originalHeight;
    private Vector3 originalCenter;
    [SerializeField] float rollHeight = 0.5f;
    [SerializeField] Vector3 rollCenter = new Vector3(0f, 0.25f, 0f);
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
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0f)
            {
                isJumping = false;
            }
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0f, v).normalized;

        Transform cam = cameraTransform;
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * input.z + camRight * input.x).normalized;

        if (moveDir.magnitude >= 0.1f)
        {
            float targetYRotation = cameraTransform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, targetYRotation, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        float inputX = Vector3.Dot(moveDir, transform.right);
        float inputZ = Vector3.Dot(moveDir, transform.forward);

        animator.SetFloat("MoveX", inputX, 0.01f, Time.deltaTime);
        animator.SetFloat("MoveZ", inputZ, 0.01f, Time.deltaTime);

        float currentSpeed = walkSpeed;
        bool isRunning = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift);

        if (isRunning)
        {
            currentSpeed *= runMultiplier;
            AudioManager.Instance.PlayPlayerRunLoop();
        }
        else if (moveDir.magnitude > 0.1f)
        {
            AudioManager.Instance.PlayPlayerWalkLoop();
        }
        else
        {
            AudioManager.Instance.StopStep();
        }

        animator.SetBool("IsRunning", isRunning);

        moveDirection = moveDir * currentSpeed;

        animator.SetFloat("Speed", moveDirection.magnitude);

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !attackScript.isAttacking)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
            isJumping = true;
            jumpTimer = jumpCooldown;

            AudioManager.Instance.PlayPlayerJump();
            AudioManager.Instance.StopStep();
        }

        cachedInputDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) cachedInputDir += transform.forward;
        if (Input.GetKey(KeyCode.S)) cachedInputDir -= transform.forward;
        if (Input.GetKey(KeyCode.A)) cachedInputDir -= transform.right;
        if (Input.GetKey(KeyCode.D)) cachedInputDir += transform.right;
        cachedInputDir.Normalize();

        if (!isRolling && Input.GetKeyDown(KeyCode.LeftControl) && !isJumping && !attackScript.isAttacking)
        {
            if (cachedInputDir == Vector3.zero)
                cachedInputDir = transform.forward;

            StartRoll(cachedInputDir);

            AudioManager.Instance.PlayPlayerRoll();
            AudioManager.Instance.StopStep();
        }
    }

    void StartRoll(Vector3 inputDir)
    {
        rollDirection = inputDir.normalized;
        isRolling = true;
        rollTimer = rollDuration;

        capsule.height = rollHeight;
        capsule.center = rollCenter;

        float leftDot = Vector3.Dot(inputDir, -transform.right);
        float rightDot = Vector3.Dot(inputDir, transform.right);
        float backDot = Vector3.Dot(inputDir, -transform.forward);

        if (leftDot > 0.7f)
            animator.SetTrigger("RollLeft");
        else if (rightDot > 0.7f)
            animator.SetTrigger("RollRight");
        else if (backDot > 0.7f)
            animator.SetTrigger("RollBackward");
        else
            animator.SetTrigger("RollForward");
    }

    void FixedUpdate()
    {
        if (attackScript != null && attackScript.IsAttackingOrBusy())
        {
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

            float rollSpeed = rollDistance / rollDuration;
            Vector3 rollVelocity = rollDirection * rollSpeed;
            rb.linearVelocity = new Vector3(rollVelocity.x, rb.linearVelocity.y, rollVelocity.z);
            return;
        }
        else
        {
            moveDirection = cachedInputDir * walkSpeed;
            rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
        }
    }
}
