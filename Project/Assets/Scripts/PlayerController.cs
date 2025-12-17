using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 15f;

    [Header("Visuals")]
    [SerializeField] private Transform modelTransform;
    [SerializeField] private float rotationSpeed = 360f;

    [Header("Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.25f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isGravityInverted = false;
    private bool isBallMode = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
#else
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
#endif

        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
        {
            if (isGrounded)
            {
                if (isBallMode)
                {
                    SwitchGravity();
                    if (AudioManager.Instance != null) AudioManager.Instance.PlayGravityFlip();
                }
                else
                {
                    Jump();
                }
            }
        }

        HandleRotation();
    }

    public void SetBallMode(bool active)
    {
        isBallMode = active;

        if (!active && isGravityInverted)
        {
            SwitchGravity();
        }
    }

    private void HandleRotation()
    {
        if (modelTransform == null) return;

        if (isGrounded)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, Mathf.Round(modelTransform.localEulerAngles.z / 90) * 90);
            modelTransform.localRotation = Quaternion.RotateTowards(modelTransform.localRotation, targetRotation, rotationSpeed * Time.deltaTime * 2f);
        }
        else
        {
            float direction = isGravityInverted ? 1f : -1f;
            modelTransform.Rotate(Vector3.forward * direction * rotationSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    private void Jump()
    {
#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
#else
        rb.velocity = new Vector2(rb.velocity.x, 0);
#endif

        float direction = isGravityInverted ? -1f : 1f;
        rb.AddForce(Vector2.up * jumpForce * direction, ForceMode2D.Impulse);

        if (AudioManager.Instance != null) AudioManager.Instance.PlayJump();
    }

    public void SwitchGravity()
    {
        isGravityInverted = !isGravityInverted;
        rb.gravityScale *= -1;

        if (groundCheck != null)
        {
            Vector3 newPos = groundCheck.localPosition;
            newPos.y *= -1;
            groundCheck.localPosition = newPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Input.GetKey(KeyCode.I))
        {
            transform.position += Vector3.right * 1.5f;
            return;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
            return;
        }

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.x < -0.5f)
            {
                Die();
                return;
            }
        }
    }

    private void Die()
    {
        if (Input.GetKey(KeyCode.I)) return;

        if (AudioManager.Instance != null) AudioManager.Instance.PlayDeath();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}