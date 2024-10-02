using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerBody;     // Ссылка на объект персонажа
    public Vector3 offset = new Vector3(0f, 1.5f, 0f); // Смещение камеры относительно персонажа
    public float mouseSensitivity = 100f;   // Чувствительность мыши
    public float verticalLookLimit = 90f;   // Ограничение вертикального обзора (в градусах)

    private float xRotation = 0f;    // Текущий угол вращения по оси X (вверх-вниз)

    void Start()
    {
        // Скрываем курсор мыши и блокируем его в центре экрана
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Получаем движение мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Ограничиваем вращение камеры вверх и вниз
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalLookLimit, verticalLookLimit);

        // Применяем вращение вверх/вниз к камере (по оси X)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Вращаем персонажа влево/вправо (по оси Y)
        playerBody.Rotate(Vector3.up * mouseX);

        // Обновляем позицию камеры с учетом смещения
        transform.position = playerBody.position + offset;

        // Применяем вращение камеры по горизонтали вместе с игроком
        transform.rotation = playerBody.rotation * Quaternion.Euler(xRotation, 0f, 0f);
    }
}
