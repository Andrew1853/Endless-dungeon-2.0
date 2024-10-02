using UnityEngine;

public class RecoilEffect : MonoBehaviour
{
    public float recoilAmountX = 2f;  // Максимальное смещение вверх по оси X
    public float recoilAmountY = 1f;  // Максимальное смещение влево/вправо по оси Y
    public float additionalRecoilX = 5f; // Дополнительное вращение вверх по оси X
    public float recoilSpeed = 10f;   // Скорость применения отдачи
    public float returnSpeed = 5f;    // Скорость возвращения камеры в исходное положение
    public float kickBackAmount = 0.1f; // Отодвигание камеры назад
    public float kickBackSpeed = 10f; // Скорость отодвигания камеры
    public float returnKickBackSpeed = 5f; // Скорость возвращения камеры после отодвигания

    private Quaternion originalRotation;
    private Quaternion currentRotation;
    private Quaternion targetRotation;
    private Quaternion additionalRotation;

    private Vector3 originalPosition;
    private Vector3 currentPosition;
    private Vector3 targetPosition;

    private Camera mainCamera;
    private bool isRecoiling = false;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            originalRotation = mainCamera.transform.localRotation;
            originalPosition = mainCamera.transform.localPosition;
        }
        else
        {
            Debug.LogError("Main Camera not found!");
        }
    }

    void LateUpdate()
    {
        if (isRecoiling)
        {
            UpdateRecoil();
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }

    public void TriggerRecoil()
    {
        if (mainCamera == null) return;

        float recoilX = Random.Range(recoilAmountX / 2, recoilAmountX);
        float recoilY = Random.Range(-recoilAmountY, recoilAmountY);

        Quaternion recoilRotation = Quaternion.Euler(-recoilX, recoilY, 0);
        targetRotation = originalRotation * recoilRotation;

        // Добавляем дополнительное вращение вверх по оси X
        additionalRotation = Quaternion.Euler(-additionalRecoilX, 0, 0);
        targetRotation *= additionalRotation;

        targetPosition = originalPosition - Vector3.forward * kickBackAmount;

        currentRotation = mainCamera.transform.localRotation;
        currentPosition = mainCamera.transform.localPosition;

        isRecoiling = true;
    }

    private void UpdateRecoil()
    {
        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, recoilSpeed * Time.deltaTime);
        mainCamera.transform.localRotation = currentRotation;

        currentPosition = Vector3.Lerp(currentPosition, targetPosition, kickBackSpeed * Time.deltaTime);
        mainCamera.transform.localPosition = currentPosition;

        if (Quaternion.Angle(currentRotation, targetRotation) < 0.1f &&
            Vector3.Distance(currentPosition, targetPosition) < 0.01f)
        {
            isRecoiling = false;
        }
    }

    private void ReturnToOriginalPosition()
    {
        currentRotation = Quaternion.Slerp(currentRotation, originalRotation, returnSpeed * Time.deltaTime);
        mainCamera.transform.localRotation = currentRotation;

        currentPosition = Vector3.Lerp(currentPosition, originalPosition, returnKickBackSpeed * Time.deltaTime);
        mainCamera.transform.localPosition = currentPosition;
    }
}
