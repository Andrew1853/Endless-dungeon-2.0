using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        // ������������ �������� ��������
    public float acceleration = 2f;     // �������� ������ ��������
    public float deceleration = 2f;     // �������� ����������
    public float jumpForce = 5f;        // ���� ������
    public float groundCheckDistance = 1.1f; // ����� Raycast ��� �������� �����
    public LayerMask groundLayer;       // ����, ������������ �����

    private Rigidbody rb;
    private Vector3 movementInput;
    private Vector3 currentVelocity;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // �������� ���� �� ������
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        movementInput = new Vector3(moveX, 0, moveZ).normalized;

        // ���������, �������� �� �� ����� � ������� Raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        // ������
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // ��������� �������� � ������ ��������� � ����������
        if (movementInput.magnitude > 0)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, movementInput * moveSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }

        // ���������� ���������
        Vector3 moveDirection = transform.TransformDirection(currentVelocity);
        rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);
    }

    void OnDrawGizmosSelected()
    {
        // ���������� Raycast � ���������
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
