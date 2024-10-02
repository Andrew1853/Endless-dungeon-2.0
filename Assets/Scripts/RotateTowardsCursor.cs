using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsCursor : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1;

    void Update()
    {
        // Получаем позицию курсора в мировых координатах
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Устанавливаем Z координату в 0, чтобы оставить её в 2D пространстве

        // Вычисляем направление от спрайта к курсору
        Vector3 direction = mousePosition - transform.position;

        // Вычисляем угол в градусах
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Интерполяция угла для плавного поворота
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


}
