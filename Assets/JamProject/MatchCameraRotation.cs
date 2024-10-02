using UnityEngine;

public class MatchCameraRotation : MonoBehaviour
{
    // Переменная для хранения ссылки на камеру
    [SerializeField] private Camera targetCamera;

    private void Start()
    {
        // Если камера не назначена в инспекторе, используем камеру по умолчанию
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void Update()
    {
        // Синхронизируем вращение объекта с вращением камеры
        transform.rotation = targetCamera.transform.rotation;
    }
}
