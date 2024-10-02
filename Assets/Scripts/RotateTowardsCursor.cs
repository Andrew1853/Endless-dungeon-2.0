using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsCursor : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1;

    void Update()
    {
        // �������� ������� ������� � ������� �����������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // ������������� Z ���������� � 0, ����� �������� � � 2D ������������

        // ��������� ����������� �� ������� � �������
        Vector3 direction = mousePosition - transform.position;

        // ��������� ���� � ��������
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // ������������ ���� ��� �������� ��������
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


}
