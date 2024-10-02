using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        // Максимальная скорость движения
    public float acceleration = 2f;     // Скорость набора скорости
    public float deceleration = 2f;     // Скорость замедления
    public float jumpForce = 5f;        // Сила прыжка
    public float groundCheckDistance = 1.1f; // Длина Raycast для проверки земли
    public LayerMask groundLayer;       // Слой, обозначающий землю

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
        // Получаем ввод от игрока
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        movementInput = new Vector3(moveX, 0, moveZ).normalized;

        // Проверяем, касаемся ли мы земли с помощью Raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        // Прыжок
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Обновляем движение с учетом ускорения и замедления
        if (movementInput.magnitude > 0)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, movementInput * moveSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }

        // Перемещаем персонажа
        Vector3 moveDirection = transform.TransformDirection(currentVelocity);
        rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);
    }

    void OnDrawGizmosSelected()
    {
        // Отображаем Raycast в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
